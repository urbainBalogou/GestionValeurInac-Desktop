using gvi.Data;
using gvi.Models;
using System;
using System.Linq;
using System.Windows;

namespace gvi
{
    /// <summary>
    /// Logique d'interaction pour ajout_employe.xaml
    /// </summary>
    public partial class ajout_employe : Window
    {
        private DataContext _context;
        private Employe _employe;
        private bool champEstRempli = false;

        public ajout_employe(DataContext context, Employe employe)
        {
            InitializeComponent();
            _context = context;
            _employe = employe;
            InitializeComboBoxes();
            if (_context != null && _employe!= null)
            {
                
                RemplirChamp();
            }
        }

        private void InitializeComboBoxes()
        {
            // Initialisation des ComboBoxes avec les Communes et Fonctions
            cbCommune.ItemsSource = _context.Communes.ToList();
            cbCommune.DisplayMemberPath = "Nom";
            cbCommune.SelectedValuePath = "Id";

            cbFonction.ItemsSource = _context.Fonctions.ToList();
            cbFonction.DisplayMemberPath = "Libelle";
            cbFonction.SelectedValuePath = "Id";
        }

        private void RemplirChamp()
        {
            txtannuler.Content = "Supprimer";
            txtentete.Content = "Modification";
            txtNom.Text = _employe.Nom;
            txtPrenom.Text = _employe.Prenom;
            txtEmail.Text = _employe.Email;
            txtTelephone.Text = _employe.Telephone;
            cbCommune.SelectedValue = _employe.CommuneId;
            cbFonction.SelectedValue = _employe.FonctionId;
            champEstRempli = true;
        }

        private void btnEnregistrer_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer et valider les données saisies
            if (!ValiderChamps())
                return;

            // Vérifier si l'email est déjà utilisé
            string email = txtEmail.Text.Trim();
            if (_context.Employes.Any(e => e.Email == email) && !champEstRempli)
            {
                MessageBox.Show("Cet email est déjà utilisé par un autre employé.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (champEstRempli)
            {
                btnModifier_Click();
                return;
            }

            // Créer un nouvel employé
            Employe nouvelEmploye = new Employe
            {
                Nom = txtNom.Text.Trim(),
                Prenom = txtPrenom.Text.Trim(),
                Email = email,
                Telephone = txtTelephone.Text.Trim(),
                CommuneId = (int)cbCommune.SelectedValue,
                FonctionId = (int)cbFonction.SelectedValue
            };

            // Ajouter l'employé à la base de données
            AjouterEmploye(nouvelEmploye);
        }

        private bool ValiderChamps()
        {
            // Validation des champs requis
            string nom = txtNom.Text.Trim();
            string prenom = txtPrenom.Text.Trim();
            string email = txtEmail.Text.Trim();
            string telephone = txtTelephone.Text.Trim();

            if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(prenom) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(telephone) ||
                cbCommune.SelectedValue == null || cbFonction.SelectedValue == null)
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Validation du numéro de téléphone
            if (!System.Text.RegularExpressions.Regex.IsMatch(telephone, @"^\d{8}$"))
            {
                MessageBox.Show("Le numéro de téléphone doit être numérique et contenir 8 chiffres.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void AjouterEmploye(Employe nouvelEmploye)
        {
            _context.Employes.Add(nouvelEmploye);
            try
            {
                _context.SaveChanges();
                MessageBox.Show("Employé ajouté avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                
                ClearFields();
              
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'enregistrement : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnModifier_Click()
        {
            // Mettre à jour les champs de l'employé
            _employe.Nom = txtNom.Text.Trim();
            _employe.Prenom = txtPrenom.Text.Trim();
            _employe.Email = txtEmail.Text.Trim();
            _employe.Telephone = txtTelephone.Text.Trim();
            _employe.CommuneId = (int)cbCommune.SelectedValue;
            _employe.FonctionId = (int)cbFonction.SelectedValue;

            try
            {
                _context.SaveChanges();
                MessageBox.Show("Employé modifié avec succès.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la modification : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearFields()
        {
            txtNom.Text = "";
            txtPrenom.Text = "";
            txtEmail.Text = "";
            txtTelephone.Text = "";
            cbCommune.SelectedIndex = -1;
            cbFonction.SelectedIndex = -1;
        }

        private void txtannuler_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (champEstRempli)
            {
                SupprimerEmploye();
            }
            else
            {
                ClearFields();
            }
        }

        private void SupprimerEmploye()
        {
            var emp = _context.Employes.FirstOrDefault(c => c.Email == txtEmail.Text);
            if (emp != null)
            {
                var result = MessageBox.Show("Confirmer la suppression", "SUPPRESSION", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    
                    try
                    {
                        _context.Employes.Remove(emp);
                        _context.SaveChanges();
                        ClearFields();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show($"Erreur lors de la suppression {ex.Message}","ERREUR",MessageBoxButton.OK);
                        return;
                    }
                    
                    MessageBox.Show("L'employé a été supprimé avec succès.");
                   
                }
            }
            else
            {
                MessageBox.Show("Aucun employé trouvé", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
