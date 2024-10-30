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
    /// Logique d'interaction pour Demandes.xaml
    /// </summary>
    public partial class DemandesForm : Window
    {
        DataContext _context;
        private Demande _demande;
        public DemandesForm()
        {
            InitializeComponent();
            _context = new DataContext();
            LoadDemandes();
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
            ajout_demande ajd = new ajout_demande(_context,_demande);
            ajd.Owner = this;
            ajd.Show();
        }
        private void LoadDemandes()
        {
            var groupedDemandes = _context.Demandes
                .Include(d => d.Commune) // Inclure la commune
                .Include(d => d.Valeurs) // Inclure les valeurs associées
                .ThenInclude(dv => dv.Valeur) // Inclure les détails de la valeur inactive
                .ThenInclude(v => v.TypeValeur) // Inclure les détails du type de valeur
                .AsEnumerable() // Convertir en IEnumerable pour utiliser LINQ en mémoire
                .Select(d => new
                {
                    Demande = d,
                    Commune = d.Commune?.Nom ?? "Non spécifiée", // Gérer le cas où Commune est null
                    Valeurs = string.Join(", ", d.Valeurs
                        .Select(v => v.Valeur?.TypeValeur?.Nature ?? "Valeur non spécifiée")), // Gérer les nullités
                    Quantites = string.Join(", ", d.Valeurs
                        .Select(v => v.Quantite.ToString())), // Assurez-vous que Quantite est un entier
                    DateDemande = d.DateDemande
                })
                .ToList();

            listViewDemandes.ItemsSource = groupedDemandes;
        }


        private void btnfresh_Click(object sender, RoutedEventArgs e)
        {
            var valeursEntree = _context.Demandes.Include(e => e.Commune).ToList();
            var valeur = _context.Valeurs
   .Include(v => v.TypeValeur)

   .ToList();
            LoadDemandes();
        }

        private void listViewDemandes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = listViewDemandes.SelectedItem;

            if (selectedItem != null)
            {
                var demande = ((dynamic)selectedItem).Demande; // Récupérer l'objet Entree
                ajout_demande ajout = new ajout_demande(_context, demande);
                ajout.ShowDialog();
                LoadDemandes(); // Recharger la liste après modifications éventuelles
            }
        }
        private void FilterDemandes(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                // Si la recherche est vide, afficher toutes les communes
                LoadDemandes();
            }
            else
            {
                // Filtrer les communes dont le nom contient le texte recherché
                var filteredDemandes = _context.Demandes
                   .Include(d => d.Commune) // Inclure la commune
                   .Include(d => d.Valeurs) // Inclure les valeurs associées
                   .ThenInclude(dv => dv.Valeur) // Inclure les détails de la valeur inactive
                   .ThenInclude(v => v.TypeValeur) // Inclure les détails du type de valeur
                   .AsEnumerable() // Convertir en IEnumerable pour utiliser LINQ en mémoire
                   .Select(d => new
                   {
                       Demande = d,
                       Commune = d.Commune?.Nom ?? "Non spécifiée", // Gérer le cas où Commune est null
                       Valeurs = string.Join(", ", d.Valeurs
                           .Select(v => v.Valeur?.TypeValeur?.Nature ?? "Valeur non spécifiée")), // Gérer les nullités
                       Quantites = string.Join(", ", d.Valeurs
                           .Select(v => v.Quantite.ToString())), // Assurez-vous que Quantite est un entier
                       DateDemande = d.DateDemande
                   })
                   .ToList().Where(d => d.Commune.ToLower().Contains( searchText.ToLower()) || d.Valeurs.ToLower() .Contains( searchText.ToLower()));
                listViewDemandes.ItemsSource = filteredDemandes;
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
                         FilterDemandes(textBox.Text);
                    });
                }, null, 300, System.Threading.Timeout.Infinite);
            }
        }
        private void btnFiltrerDates_Click(object sender, RoutedEventArgs e)
        {
            FilterByDates();
        }

        private void FilterByDates()
        {
            var startDate = datePickerStart.SelectedDate;
            var endDate = datePickerEnd.SelectedDate;

            var filteredDemandes = _context.Demandes
                .Include(d => d.Commune)
                .Include(d => d.Valeurs)
                .ThenInclude(dv => dv.Valeur) // Inclure les détails de la valeur inactive
                   .ThenInclude(v => v.TypeValeur) // Inclure les détails du type de valeur
                   .AsEnumerable() // Convertir en IEnumerable pour utiliser LINQ en mémoire
                .Where(d => (!startDate.HasValue || d.DateDemande >= startDate.Value) &&
                            (!endDate.HasValue || d.DateDemande <= endDate.Value))
                .Select(d => new
                {
                    Commune = d.Commune?.Nom ?? "Non spécifiée",
                    Valeurs = string.Join(", ", d.Valeurs.Select(v => v.Valeur?.TypeValeur?.Nature ?? "Non spécifiée")),
                    Quantites = string.Join(", ", d.Valeurs.Select(v => v.Quantite.ToString())),
                    DateDemande = d.DateDemande.ToString("dd/MM/yyyy")
                })
                .ToList();

            listViewDemandes.ItemsSource = filteredDemandes;
        }
    }
}

        