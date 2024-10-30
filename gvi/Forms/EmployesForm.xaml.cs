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
using Microsoft.EntityFrameworkCore;


namespace gvi
{
    /// <summary>
    /// Logique d'interaction pour Employes.xaml
    /// </summary>
    public partial class EmployesForm : Window
    {
        private DataContext _context;
        private Employe _employe;
        public EmployesForm()
        {
            InitializeComponent();
            _context = new DataContext();
            var employes = _context.Employes
    .Include(e => e.Commune)
    .Include(e => e.Fonction)
    .ToList();
            LoadEmployes();
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

        private void txtemplo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

        private void aje_Click(object sender, RoutedEventArgs e)
        {
            ajout_employe com = new ajout_employe(_context, _employe);
            com.Owner = this;
            com.Show();
        }

        private void listViewFonctions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            if (listViewEmployes.SelectedItem is Employe selectedEmploye)
            {
                ajout_employe ajout = new ajout_employe(_context, selectedEmploye);
                ajout.ShowDialog();
                // Recharger les données après la fermeture du formulaire si nécessaire
                LoadEmployes();
            }
        }
        private void LoadEmployes()
        {
            List<Employe> employes = _context.Employes.ToList();
            listViewEmployes.ItemsSource = employes;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadEmployes();
        }
        private void filteredEmployes(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                // Si la recherche est vide, afficher toutes les communes
                LoadEmployes();
            }
            else
            {
                // Filtrer les communes dont le nom contient le texte recherché
                var filteredEmployes = _context.Employes
                    .ToList()
                    .Where(c => c.Nom.ToLower().Contains(searchText.ToLower()) || c.Prenom.ToLower().Contains(searchText.ToLower()) || c.Commune.Nom.ToLower().Contains(searchText.ToLower()));
                listViewEmployes.ItemsSource = filteredEmployes;
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
                         filteredEmployes(textBox.Text);
                    });
                }, null, 300, System.Threading.Timeout.Infinite);
            }
        }
    }
}
