using gvi.Data;
using gvi.Models;
using Microsoft.EntityFrameworkCore;
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
    /// Logique d'interaction pour ValeurInactive.xaml
    /// </summary>
    public partial class ValeurInactiveForm : Window
    {
        private DataContext _context;
        private Valeur _valeur;
        public ValeurInactiveForm()
        {

            InitializeComponent();
            
            _context= new DataContext();
            var valeur = _context.Valeurs
     .Include(v => v.TypeValeur)
     
     .ToList();
            LoadValeur();
        }
        private void type_val_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TypeValeurForm valeur = new TypeValeurForm();
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

        private void btnvi_Click(object sender, RoutedEventArgs e)
        {
            ajout_valeur entre = new ajout_valeur(_context,_valeur);
            entre.Owner = this;
            entre.Show();
        }
        private void LoadValeur()
        {
        
           
                List<Valeur> valeurs = _context.Valeurs.ToList();
                listViewValeurs.ItemsSource = valeurs;
            
           
        }
        
       

        private void listViewValeurs_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            if (listViewValeurs.SelectedItem is Valeur selectedValeur)
            {
                ajout_valeur ajout = new ajout_valeur(_context, selectedValeur);
                ajout.ShowDialog();
                // Recharger les données après la fermeture du formulaire si nécessaire
                LoadValeur();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadValeur();
        }
    }
}
