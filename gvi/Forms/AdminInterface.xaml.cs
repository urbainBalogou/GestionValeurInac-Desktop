using gvi.Authentification;
using gvi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace gvi.Forms
{
    /// <summary>
    /// Logique d'interaction pour AdminInterface.xaml
    /// </summary>
    public partial class AdminInterface : Window

    {
        private DataContext _context;
        public AdminInterface()
        {
            InitializeComponent();
            _context = new DataContext();
            LoadUser();
        }
        public void LoadUser()
        {
           var utilisateurs = _context.Utilisateurs.ToList();
            UserListGrid.ItemsSource = utilisateurs;
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            var signup = new signup();
            signup.Show();
        }
        void delUser(Utilisateur user)
        {
            _context.Utilisateurs.Remove(user);
            _context.SaveChanges();
        }

        private void UserListGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)

        {
            var selectedUser = (Utilisateur)UserListGrid.SelectedItem;
            var result = MessageBox.Show("Confirmer la suppression", "SUPPRESSION", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                delUser(selectedUser);
                MessageBox.Show("L'utilisateur a été supprimé avec succès.");
                LoadUser();
            }
           
           

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadUser();
        }
    }
}
