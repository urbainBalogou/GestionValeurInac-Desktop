using gvi.Data;
using System;
using System.Linq;
using System.Windows;

namespace gvi.Authentification
{
    public partial class login : Window
    {
        private DataContext _context;
        private Utilisateur _utilisateur;

        public login()
        {
            InitializeComponent();
            _context = new DataContext();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string enteredUsername = UsernameTextBox.Text;
            string enteredPassword = PasswordTextBox.Password;

            // Rechercher l'utilisateur dans la base de données
            Utilisateur utilisateur = _context.Utilisateurs.SingleOrDefault(u => u.Username == enteredUsername);

            if (utilisateur != null && utilisateur.VerifyPassword(enteredPassword))
            {
                MessageBox.Show("Connexion réussie !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                // Rediriger selon le rôle de l'utilisateur
                if (utilisateur.Role == "Admin")
                {
                    // Rediriger vers le dashboard admin
                    MainWindow adminDashboard = new MainWindow(_context,utilisateur);
                    adminDashboard.Show();
                }
                else
                {
                    // Rediriger vers la page utilisateur
                    MainWindow userDashboard = new MainWindow(_context,utilisateur);
                    userDashboard.Show();
                }

                this.Close();
            }
            else
            {
                MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Label_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void txtannuler_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            init();
        }
        void init()
        {
            UsernameTextBox.Text = null;
            PasswordTextBox.Password = null;
        }
    }
}
