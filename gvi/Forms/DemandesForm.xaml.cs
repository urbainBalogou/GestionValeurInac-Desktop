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

namespace gvi
{
    /// <summary>
    /// Logique d'interaction pour Demandes.xaml
    /// </summary>
    public partial class DemandesForm : Window
    {
        public DemandesForm()
        {
            InitializeComponent();
        }

        private void type_val_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TypeValeurForm type_valeur = new TypeValeurForm();
            type_valeur.Owner = this;
            type_valeur.Show();
        }

        private void txtvaleur_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ValeurInactiveForm valeur = new ValeurInactiveForm();
            valeur.Owner = this;
            valeur.Show();
        }

        private void txtentree_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            EntréesForm entrees = new EntréesForm();
            entrees.Owner = this;
            entrees.Show();
        }

        private void txtdemandes_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DemandesForm demandes = new DemandesForm();
            demandes.Owner = this;
            demandes.Show();
        }

        private void sorties_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SortiesForm sortie = new SortiesForm();
            sortie.Owner = this;
            sortie.Show();
        }

        private void employe_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            EmployesForm employe = new EmployesForm();
            employe.Owner = this;
            employe.Show();
        }
        private void txtfonc_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FonctionForm fonc = new FonctionForm();
            fonc.Owner = this;
            fonc.Show();
        }

        private void txtcommune_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CommuneForm com = new CommuneForm();
            com.Owner = this;
            com.Show();
        }

        private void ajd_Click(object sender, RoutedEventArgs e)
        {
            ajout_demande ajd = new ajout_demande();
            ajd.Owner = this;
            ajd.Show();
        }
    }
}
