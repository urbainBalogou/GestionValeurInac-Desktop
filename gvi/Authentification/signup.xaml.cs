using gvi.Data;
using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace gvi.Authentification
{
    public partial class signup : Window

    {
        private DataContext _context;
        public string role = "";
        public signup()
        {
            InitializeComponent();
            _context = new DataContext();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {

            // Récupérer les valeurs des champs du formulaire
            string username = UsernameTextBox.Text;
            string password = PasswordTextBox.Password;
            if ((RoleComboBox.SelectedItem) != null)
            {
                role = ((ComboBoxItem)RoleComboBox.SelectedItem).Content.ToString();
            }
     
            

            // Validation basique
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Veuillez entrer un nom d'utilisateur, un mot de passe et assigner un role.");
                return;
            }

            // Créer un nouvel utilisateur
            Utilisateur nouvelUtilisateur = new Utilisateur
            {
                Username = username,
                Role = role
            };

            // Hacher le mot de passe et le définir
            nouvelUtilisateur.SetPassword(password);
            _context.Utilisateurs.Add(nouvelUtilisateur);

            // Enregistrer l'utilisateur (par exemple dans une base de données ou une liste)
            // Ici, on va simplement simuler l'enregistrement dans une liste statique (ou utilisez une base de données)
            try
            {
                _context.SaveChanges();
                MessageBox.Show($"Utilisateur crée avec succès", "CREATION D'UTILISATEUR", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la création de l'utilisateur: {ex.Message}","ERREUR",MessageBoxButton.OK,MessageBoxImage.Error);
            }
           
        }

        private void btnfermer_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
