using gvi.Data;
using gvi.Models;
using System;
using System.Linq;
using System.Windows;

namespace gvi.AddForms
{
    /// <summary>
    /// Logique d'interaction pour ajout_fonction.xaml
    /// </summary>
    public partial class ajout_fonction : Window
    {
        private DataContext _context;
        private Fonction _fonction;
        private bool champEstRempli;

        // Constructeur modifié pour gérer la modification
        public ajout_fonction(DataContext context,Fonction fonction)
        {
            InitializeComponent();
            _context = context;
            _fonction = fonction;
            champEstRempli = false;

            if (_fonction != null)
            {
                RemplirChamp(); // Pré-remplir les champs si modification
            }
            else
            {
                textBoxInit(); // Initialiser les champs vides pour l'ajout
            }
        }

        private void RemplirChamp()
        {
            txtannuler.Content = "Supprimer";
            txtentete.Content = "Modification";
            txtcodef.Text = _fonction.CodeFonction;
            txtnomf.Text = _fonction.Libelle;
            champEstRempli = true;
        }

        private void textBoxInit()
        {
            txtcodef.Text = "";
            txtnomf.Text = "";
        }

        private void btnenregistrer_Click(object sender, RoutedEventArgs e)
        {
            String codeFonction = txtcodef.Text.Trim();
            String nomFonction = txtnomf.Text.Trim();

            if (String.IsNullOrEmpty(codeFonction) || String.IsNullOrEmpty(nomFonction))
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Vérifier l'unicité du CodeFonction
            bool existe = _context.Fonctions.Any(c => c.CodeFonction == codeFonction);

            if (existe && !champEstRempli)
            {
                MessageBox.Show("Le Code Fonction existe déjà. Veuillez en choisir un autre.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (existe && champEstRempli)
            {
                btnModifier_Click(); // Appeler la fonction de modification
                return;
            }

            // Ajouter une nouvelle fonction
            Fonction nouvelleFonction = new Fonction { CodeFonction = codeFonction, Libelle = nomFonction };
            _context.Fonctions.Add(nouvelleFonction);

            try
            {
                _context.SaveChanges();
                MessageBox.Show("Fonction ajoutée avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                textBoxInit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'enregistrement : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnModifier_Click()
        {
            _fonction.CodeFonction = txtcodef.Text.Trim();
            _fonction.Libelle = txtnomf.Text.Trim();

            try
            {
                _context.SaveChanges();
                MessageBox.Show("Fonction modifiée avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close(); // Fermer la fenêtre après modification
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la modification : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtannuler_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (champEstRempli)
            {
                // Récupérer la commune correspondant au code
                var fonc = _context.Fonctions.FirstOrDefault(f => f.CodeFonction == txtcodef.Text);

                if (fonc != null)
                {
                    var result = MessageBox.Show("Confirmer la suppression", "SUPPRESSION", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        // Supprimer la commune
                        _context.Fonctions.Remove(fonc);

                        // Sauvegarder les modifications
                        _context.SaveChanges();

                        MessageBox.Show("La fonction a été supprimée avec succès.");
                        textBoxInit();
                    }
                }
                else
                {
                    //MessageBox.Show("Aucune fonction trouvée avec ce code", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);(
                    textBoxInit();
                }
            }

            }
        }
}
