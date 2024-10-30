using gvi.Authentification;
using gvi.Data;
using gvi.Forms;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace gvi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window

    {
        private DataContext _context;
        private Utilisateur _utilisateur;
        public MainWindow(DataContext context, Utilisateur utilisateur)
        {
            InitializeComponent();
            
            _context = context;
            _utilisateur = utilisateur;
            if(_context != null && _utilisateur !=null)
            {
                username.Content = _utilisateur.Username;

                if (_utilisateur.Role != null && _utilisateur.Role == "administrateur")
                {
                    txtcreate.Visibility = Visibility.Visible;
                }
            }

           

        }

        private void btncommune_Click(object sender, RoutedEventArgs e)
        {
            CommuneForm commune = new CommuneForm();
            commune.Owner = this;
            commune.Show();

        }

        private void btnfonction_Click(object sender, RoutedEventArgs e)
        {
            FonctionForm fonction = new FonctionForm();
            fonction.Owner = this;
            fonction.Show();

        }

        private void btnemployes_Click(object sender, RoutedEventArgs e)
        {
            EmployesForm employe = new EmployesForm();
            employe.Owner = this;
            employe.Show();
        }



        private void btnvaleur_Click(object sender, RoutedEventArgs e)
        {
            ValeurInactiveForm valeur = new ValeurInactiveForm();
            valeur.Owner = this;
            valeur.Show();
        }

        private void btnentree_Click(object sender, RoutedEventArgs e)
        {
            EntréesForm entrees = new EntréesForm();
            entrees.Owner = this;
            entrees.Show();
        }

        private void btndemande_Click(object sender, RoutedEventArgs e)
        {
            DemandesForm demandes = new DemandesForm();
            demandes.Owner = this;
            demandes.Show();
        }

        private void btnsortie_Click(object sender, RoutedEventArgs e)
        {
            SortiesForm sortie = new SortiesForm();
            sortie.Owner = this;
            sortie.Show();
        }

        private void btntype_Click(object sender, RoutedEventArgs e)
        {
            TypeValeurForm type_valeur = new TypeValeurForm();
            type_valeur.Owner = this;
            type_valeur.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            login log = new login();
            log.Owner = this;
            log.Show();
            this.Hide();
        }

        private void txtcreate_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AdminInterface log = new AdminInterface();
            log.Owner = this;
            log.Show();
          
        }

        private void btnstockage_Click(object sender, RoutedEventArgs e)
        {
            StockageView log = new StockageView();
          
            log.Show();
        }
    }

}

