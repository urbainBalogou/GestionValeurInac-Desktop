using System;
using System.Windows;
using gvi.Models; // Assurez-vous que le namespace est correct pour accéder à votre modèle
using System.Linq;
using gvi.Data;

namespace gvi
{
    public partial class ajout_valeur : Window
    {
        private DataContext _context;
        private Valeur _valeur;
        public int valfaciale;
        private bool champEstRempli;
        public ajout_valeur(DataContext context, Valeur valeur)
        {
            InitializeComponent();
            _context = new DataContext();
            _valeur = valeur;

            InitializeComboBoxes();
            if (_context != null && _valeur!=null)
            {
                _valeur = valeur;
                _context = context;
                RemplirChamp();

            }
        }

        private void RemplirChamp()
        {
            txtannuler.Content = "Supprimer";
            ValeurFeuilletTextBox.Text = _valeur.nombre_de_valeur_par_feuillet_ou_carnet.ToString();
            NombreFeuilletsTextBox.Text = _valeur.nombre_de_feuillets.ToString();

            TypeValeurComboBox.SelectedValue = _valeur.typeValeurId;

            champEstRempli = true;
        }
        private void InitializeComboBoxes()
        {

            var type = _context.TypeValeurs.ToList();

            TypeValeurComboBox.ItemsSource = type;
            TypeValeurComboBox.DisplayMemberPath = "NatureValeurFaciale";
            // Le nom de la commune à afficher
            TypeValeurComboBox.SelectedValuePath = "Id";  // Utilisé pour récupérer l'ID de la commune
        }
        private void btnModifier_Click()
        {
            // Mettre à jour les champs de l'employé
            _valeur.nombre_de_valeur_par_feuillet_ou_carnet = int.Parse(ValeurFeuilletTextBox.Text.Trim());
            _valeur.nombre_de_feuillets = int.Parse(NombreFeuilletsTextBox.Text.Trim());

            TypeValeur selectedTypeValeur = (TypeValeur)TypeValeurComboBox.SelectedItem;
            int vf = selectedTypeValeur.ValeurFaciale;
            // Mettre à jour les identifiants des relations
            _valeur.typeValeurId = (int)TypeValeurComboBox.SelectedValue;
            _valeur.montant = vf * _valeur.nombre_de_valeur_par_feuillet_ou_carnet * _valeur.nombre_de_feuillets;// Mettre à jour l'ID de la commune


            try
            {
                // Sauvegarder les modifications dans le contexte
                _context.SaveChanges();
                MessageBox.Show("Valeur modifiée avec succès.");
                this.Close(); // Fermer la fenêtre après modification
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la modification : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // Méthode appelée lorsqu'on clique sur le bouton Enregistrer
        private void EnregistrerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Vérifier si tous les champs sont remplis
                if (string.IsNullOrWhiteSpace(ValeurFeuilletTextBox.Text) ||
                    string.IsNullOrWhiteSpace(NombreFeuilletsTextBox.Text) ||

                    TypeValeurComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Veuillez remplir tous les champs.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }


                // Récupérer les données du formulaire
                int nombreValeurFeuillets = int.Parse(ValeurFeuilletTextBox.Text);
                int nombreFeuillets = int.Parse(NombreFeuilletsTextBox.Text);

                TypeValeur selectedTypeValeur = (TypeValeur)TypeValeurComboBox.SelectedItem;
                bool existe = _context.Valeurs.Any(v => v.typeValeurId == selectedTypeValeur.Id && v.nombre_de_feuillets == nombreFeuillets && v.nombre_de_valeur_par_feuillet_ou_carnet == nombreValeurFeuillets);
                if (!existe && !champEstRempli)
                {
                    Valeur nouvelleValeur = new Valeur
                    {
                        nombre_de_valeur_par_feuillet_ou_carnet = nombreValeurFeuillets,
                        nombre_de_feuillets = nombreFeuillets,
                        typeValeurId = selectedTypeValeur.Id,

                        // Assurez-vous que l'Id du TypeValeur est correctement récupéré
                    };
                    nouvelleValeur.CalculMontant(selectedTypeValeur.ValeurFaciale, nombreValeurFeuillets, nombreFeuillets);
                    // Enregistrer dans la base de données
                    try
                    {
                        _context.Valeurs.Add(nouvelleValeur);
                        _context.SaveChanges();
                        MessageBox.Show("Valeur enregistrée avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erreur lors de l'enregistrement:{ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    // Réinitialiser les champs
                    ValeurFeuilletTextBox.Clear();
                    NombreFeuilletsTextBox.Clear();

                    TypeValeurComboBox.SelectedIndex = -1;
                }
                else if (existe && !champEstRempli)
                {
                    MessageBox.Show("Erreur une valeur inactive avec les mêmes propriétés existent déjà", "Erreur", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                else
                {
                    btnModifier_Click();
                    return;
                }

                // Créer une nouvelle instance de Valeur




            }
            catch (Exception ex)
            {
                // Gérer les exceptions
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtannuler_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (champEstRempli)
            {
                TypeValeur selectedTypeValeur = (TypeValeur)TypeValeurComboBox.SelectedItem;
                // Récupérer la commune correspondant au code
                var com = _context.Valeurs.FirstOrDefault(c => c.nombre_de_valeur_par_feuillet_ou_carnet == int.Parse(ValeurFeuilletTextBox.Text) && c.nombre_de_feuillets == int.Parse(NombreFeuilletsTextBox.Text) && c.typeValeurId == selectedTypeValeur.Id);

                if (com != null)
                {
                    var result = MessageBox.Show("Confirmer la suppression", "SUPPRESSION", MessageBoxButton.YesNo, MessageBoxImage.Warning);


                    if (result == MessageBoxResult.Yes)
                    {
                        // Supprimer la commune
                        _context.Valeurs.Remove(com);

                        // Sauvegarder les modifications
                        _context.SaveChanges();

                        MessageBox.Show("La valeur a été supprimée avec succès.");
                        ValeurFeuilletTextBox.Clear();
                        NombreFeuilletsTextBox.Clear();

                        TypeValeurComboBox.SelectedIndex = -1;

                    }
                }
                else
                {
                    MessageBox.Show("Aucune valeur trouvée!", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            ValeurFeuilletTextBox.Clear();
            NombreFeuilletsTextBox.Clear();

            TypeValeurComboBox.SelectedIndex = -1;
        }
    }
}
