using gvi.Data;
using gvi.Models;
using gvi.PrintForms;
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
    /// Logique d'interaction pour Sorties.xaml
    /// </summary>
    public partial class SortiesForm : Window
    {
        private DataContext _context;
        private Sortie _sortie;
        public SortiesForm()
        {
            InitializeComponent();
            _context = new DataContext();
            LoadSorties();
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

        private void btnsort_Click(object sender, RoutedEventArgs e)
        {
            ajout_sorties com = new ajout_sorties(_context, _sortie);
            com.Owner = this;
            com.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadSorties();
        }
        private void LoadSorties()
        {
            var groupedSorties = _context.Sorties
                .Include(d => d.Commune) // Inclure la commune
                .Include(d => d.Employe)                .Include(d => d.Valeurs) // Inclure les valeurs associées
                .ThenInclude(dv => dv.Valeur) // Inclure les détails de la valeur inactive
                .ThenInclude(v => v.TypeValeur) // Inclure les détails du type de valeur
                .AsEnumerable() // Convertir en IEnumerable pour utiliser LINQ en mémoire
                .Select(s => new
                {
                    Sortie = s,
                    Commune = s.Commune?.Nom ?? "Non spécifiée", // Gérer le cas où Commune est null
                    Employe = s.Employe?.Nom ?? "Non Inexistant",
                    Valeurs = string.Join(", ", s.Valeurs
                        .Select(v => v.Valeur?.TypeValeur?.Nature ?? "Valeur non spécifiée")), // Gérer les nullités
                    Quantites = string.Join(", ", s.Valeurs
                        .Select(v => v.Quantite.ToString())), // Assurez-vous que Quantite est un entier
                    DateSortie = s.DateSortie
                })
                .ToList();

            listViewSorties.ItemsSource = groupedSorties;
        }

       

        private void FilterEntrees(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                // Si la recherche est vide, afficher toutes les communes
                LoadSorties();
            }
            else
            {
                // Filtrer les communes dont le nom contient le texte recherché
                var FilterSorties = _context.Sorties
                .Include(d => d.Commune) // Inclure la commune
                .Include(d => d.Employe).Include(d => d.Valeurs) // Inclure les valeurs associées
                .ThenInclude(dv => dv.Valeur) // Inclure les détails de la valeur inactive
                .ThenInclude(v => v.TypeValeur) // Inclure les détails du type de valeur
                .AsEnumerable() // Convertir en IEnumerable pour utiliser LINQ en mémoire
                .Select(s => new
                {
                    Sortie = s,
                    Commune = s.Commune?.Nom ?? "Non spécifiée", // Gérer le cas où Commune est null
                    Employe = s.Employe?.Nom ?? "Non Inexistant",
                    Valeurs = string.Join(", ", s.Valeurs
                        .Select(v => v.Valeur?.TypeValeur?.Nature ?? "Valeur non spécifiée")), // Gérer les nullités
                    Quantites = string.Join(", ", s.Valeurs
                        .Select(v => v.Quantite.ToString())), // Assurez-vous que Quantite est un entier
                    DateSortie = s.DateSortie
                })
                .ToList().Where(d => d.Commune.ToLower().Contains(searchText.ToLower()) || d.Valeurs.ToLower().Contains(searchText.ToLower()));
                listViewSorties.ItemsSource = FilterSorties;
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
                        FilterEntrees(textBox.Text);
                    });
                }, null, 300, System.Threading.Timeout.Infinite);
            }
        }
        private void FilterByDates()
        {
            var startDate = datePickerStart.SelectedDate;
            var endDate = datePickerEnd.SelectedDate;

            var filteredSorties = _context.Sorties
                .Include(d => d.Commune)
                .Include(d => d.Valeurs)
                .ThenInclude(dv => dv.Valeur) // Inclure les détails de la valeur inactive
                   .ThenInclude(v => v.TypeValeur) // Inclure les détails du type de valeur
                   .AsEnumerable() // Convertir en IEnumerable pour utiliser LINQ en mémoire
                .Where(d => (!startDate.HasValue || d.DateSortie >= startDate.Value) &&
                            (!endDate.HasValue || d.DateSortie <= endDate.Value))
                .Select(d => new
                {
                    Commune = d.Commune?.Nom ?? "Non spécifiée",
                    Valeurs = string.Join(", ", d.Valeurs.Select(v => v.Valeur?.TypeValeur?.Nature ?? "Non spécifiée")),
                    Quantites = string.Join(", ", d.Valeurs.Select(v => v.Quantite.ToString())),
                    DateSortie = d.DateSortie.ToString("dd/MM/yyyy")
                })
                .ToList();

            listViewSorties.ItemsSource = filteredSorties;
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
             SortiePrint sortiePrint = new SortiePrint();
            sortiePrint.ShowDialog();
        }

        private void listViewSorties_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = listViewSorties.SelectedItem;

            if (selectedItem != null)
            {
                var sortie = ((dynamic)selectedItem).Sortie; // Récupérer l'objet Sortie
                ajout_sorties ajout = new ajout_sorties(_context, sortie);
                ajout.ShowDialog();
                LoadSorties(); // Recharger la liste après modifications éventuelles
            }
        }

        private void btnfiltre_Click(object sender, RoutedEventArgs e)
        {
            FilterByDates();
        }
    }
}
