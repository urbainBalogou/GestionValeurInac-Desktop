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
    /// Logique d'interaction pour Commune.xaml
    /// </summary>
    public partial class CommuneForm : Window
    {
        private DataContext _context;
        private Commune _commune;
        public CommuneForm()
        {
            InitializeComponent();
            rechercher.Text = null;
            _context = new DataContext();
            LoadCommunes();
        }
       


        private void Window_Activated(object sender, EventArgs e)
        {

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

        private void btnajout_Click(object sender, RoutedEventArgs e)
        {
            ajout_commune ajc = new ajout_commune(_context,_commune);
            ajc.Owner = this;
            ajc.Show();
        }
        private void LoadCommunes()
        {
            List<Commune> communes = _context.Communes.ToList();
            listViewCommunes.ItemsSource = communes;
        }

        private void listViewCommunes_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            if (listViewCommunes.SelectedItem is Commune selectedCommune)
            {
                ajout_commune ajout = new ajout_commune(_context,selectedCommune);
                ajout.ShowDialog();
                // Recharger les données après la fermeture du formulaire si nécessaire
                LoadCommunes();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadCommunes();
        }
        private void FilterCommunes(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                // Si la recherche est vide, afficher toutes les communes
                LoadCommunes();
            }
            else
            {
                // Filtrer les communes dont le nom contient le texte recherché
                var filteredCommunes = _context.Communes
                    .ToList()
                    .Where(c => c.Nom.ToLower().Contains(searchText.ToLower()) || c.CodeCommune.ToLower().Contains(searchText.ToLower()));
                listViewCommunes.ItemsSource = filteredCommunes;
            }
        }

        private System.Threading.Timer _searchTimer;
        private void rechercher_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                // Annuler le timer précédent s'il existe
                _searchTimer?.Dispose();

                // Créer un nouveau timer qui se déclenchera après 300ms
                _searchTimer = new System.Threading.Timer(_ =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        FilterCommunes(textBox.Text);
                    });
                }, null, 300, System.Threading.Timeout.Infinite);
            }
        }
    }
}
