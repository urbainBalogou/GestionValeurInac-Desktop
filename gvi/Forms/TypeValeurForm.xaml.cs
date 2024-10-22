using gvi.Data;
using gvi.Models;
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
    /// Logique d'interaction pour TypeValeur.xaml
    /// </summary>
    public partial class TypeValeurForm : Window
    {
        private DataContext _context;
        private TypeValeur _type;
        public TypeValeurForm()
        {
            InitializeComponent();
            _context = new DataContext();
            LoadType();
        }


        private void txtvaleur_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ValeurInactiveForm valeur = new ValeurInactiveForm();
            valeur.Owner = this;
            valeur.Show();
        }

        private void txtcommune_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CommuneForm com = new CommuneForm();
            com.Owner = this;
            com.Show();
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
        private void txtentree_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            EntréesForm entre = new EntréesForm();
            entre.Owner = this;
            entre.Show();
        }

        private void btntype_Click(object sender, RoutedEventArgs e)
        {
            ajout_type entre = new ajout_type(_context, _type);
            entre.Owner = this;
            entre.Show();
        }
        public void LoadType()
        {
            List<TypeValeur> typeValeurs = _context.TypeValeurs.ToList();
            listViewType.ItemsSource = typeValeurs;
           
        }

        private void listViewCommunes_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
           if (listViewType.SelectedItem is TypeValeur selectedType)
           {
               ajout_type ajout = new ajout_type(_context, selectedType);
               ajout.ShowDialog();
               // Recharger les données après la fermeture du formulaire si nécessaire
               LoadType();
           }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadType();
        }
    }
}
