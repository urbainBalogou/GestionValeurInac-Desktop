using gvi.AddForms;
using gvi.Data;
using gvi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace gvi
{
    /// <summary>
    /// Logique d'interaction pour Fonction.xaml
    /// </summary>
    public partial class FonctionForm : Window
    {
        private DataContext _context;
        private Fonction _fonction;
        private DispatcherTimer _timer;
       
        public FonctionForm()
        {
            InitializeComponent();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(5); // Définir l'intervalle à 5 secondes
            //_timer.Tick += Button_Click; // Ajouter un événement Tick
            _timer.Start();
            _context = new DataContext();
            LoadFonctions();
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
        private void txtentree_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            EntréesForm ent = new EntréesForm();
            ent.Owner = this;
            ent.Show();
        }

        private void ajf_Click(object sender, RoutedEventArgs e)
        {
            ajout_fonction ajf = new ajout_fonction(_context,_fonction);
            ajf.Owner = this;
            ajf.Show();
        }
        private void LoadFonctions()
        {
            List<Fonction> fonctions = _context.Fonctions.ToList();
            listViewFonctions.ItemsSource = fonctions;
        }
       

        private void listViewFonctions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listViewFonctions.SelectedItem is Fonction selectedFonction)
            {
                ajout_fonction ajout = new ajout_fonction(_context,selectedFonction);
                ajout.ShowDialog();
                // Recharger les données après la fermeture du formulaire si nécessaire
                LoadFonctions();
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {

            LoadFonctions();
        }
    }
}
