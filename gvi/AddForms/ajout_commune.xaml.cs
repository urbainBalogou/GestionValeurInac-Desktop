using gvi.Data;
using gvi.Models;
using Microsoft.EntityFrameworkCore; // Assurez-vous d'ajouter cette directive
using System;
using System.Linq;
using System.Windows;

namespace gvi
{
    /// <summary>
    /// Logique d'interaction pour ajout_commune.xaml
    /// </summary>
    public partial class ajout_commune : Window
    {
        private DataContext _context;
        private Commune _commune;
        private bool champEstRempli;

        // Constructeur modifié pour accepter un DataContext et une Commune
        public ajout_commune(DataContext context, Commune commune)
        {
            InitializeComponent();
            champEstRempli = false;
            _context = context;
            _commune = commune;
            if (_commune != null)
            {
                RemplirChamp();  // Appeler cette méthode pour remplir les champs
            }
        }

        private void RemplirChamp()
        {
            txtanuler.Content = "Supprimer";
            txtcode.Text = _commune.CodeCommune;
            txtnom.Text = _commune.Nom;
            txtmail.Text = _commune.Mail;
            txtadresse.Text = _commune.Adresse;
            champEstRempli = true;
        }

        private void btnenregistrer_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer les valeurs des TextBoxes
            string codeCommune = txtcode.Text.Trim();
            string nom = txtnom.Text.Trim();
            string email = txtmail.Text.Trim();
            string adresse = txtadresse.Text.Trim();

            // Validation de base
            if (string.IsNullOrEmpty(codeCommune) || string.IsNullOrEmpty(nom) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(adresse))
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validation du format de l'email
            var emailAttribute = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();
            if (!emailAttribute.IsValid(email))
            {
                MessageBox.Show("Veuillez entrer une adresse email valide.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Vérifier l'unicité du CodeCommune
            bool existe;
            if (champEstRempli) // Modification
            {
                // Exclure la commune actuelle lors de la vérification de l'unicité
                existe = _context.Communes.Any(c => c.CodeCommune == codeCommune && c.Id != _commune.Id);
            }
            else // Ajout
            {
                existe = _context.Communes.Any(c => c.CodeCommune == codeCommune);
            }

            if (existe)
            {
                MessageBox.Show("Le Code Commune existe déjà. Veuillez en choisir un autre.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (champEstRempli) // Modification
            {
                btnModifier_Click();
                return;
            }

            // Créer une nouvelle instance de Commune
            Commune nouvelleCommune = new Commune { CodeCommune = codeCommune, Nom = nom, Mail = email, Adresse = adresse };

            // Ajouter la nouvelle commune au contexte
            _context.Communes.Add(nouvelleCommune);
            

            try
            {
                // Sauvegarder les changements dans la base de données
                _context.SaveChanges();
                MessageBox.Show("Commune ajoutée avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close(); // Fermer la fenêtre après l'ajout
            }
            catch (Exception ex)
            {
                // Gérer les erreurs éventuelles
                MessageBox.Show($"Erreur lors de l'enregistrement : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnModifier_Click()
        {
            
            _commune.Nom = txtnom.Text.Trim();
            _commune.Mail = txtmail.Text.Trim();
            _commune.Adresse = txtadresse.Text.Trim();

            try
            {
                // L'entité _commune est déjà suivie par le contexte, donc simplement appeler SaveChanges()
                _context.SaveChanges();
                MessageBox.Show("Commune modifiée avec succès.");
                this.Close(); // Fermer après modification
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'enregistrement : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtanuler_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
            if (champEstRempli)
            {
                // Récupérer la commune correspondant au code
                var com = _context.Communes.FirstOrDefault(c => c.CodeCommune == txtcode.Text);

                if (com != null)
                {
                    var result = MessageBox.Show("Confirmer la suppression", "SUPPRESSION", MessageBoxButton.YesNo, MessageBoxImage.Warning);


                    if (result == MessageBoxResult.Yes)
                    {
                        // Supprimer la commune
                        _context.Communes.Remove(com);

                        // Sauvegarder les modifications
                        _context.SaveChanges();

                        MessageBox.Show("La commune a été supprimée avec succès.");
                        initialisation();
                    }
                }
                else
                {
                    MessageBox.Show("Aucune commune trouvée avec ce code.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }




            }
            else
            {
                initialisation();
            }



    }
        private void initialisation()
        {
            txtnom.Text = "";
            txtcode.Text = "";
            txtmail.Text = "";
            txtadresse.Text = "";
        }
    }
}
