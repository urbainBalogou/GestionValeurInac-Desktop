using gvi.Data;
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

namespace gvi.Forms
{
    /// <summary>
    /// Logique d'interaction pour StockageView.xaml
    /// </summary>
    public partial class StockageView : Window
    {
        private DataContext _context;
        public StockageView()
        {
            InitializeComponent();
            _context = new DataContext();
            var valeurs = _context.Valeurs
    .Include(v => v.TypeValeur) // Inclure `TypeValeur` pour éviter les références nulles
    .ToList();

            ViewStockage();
        }
        void ViewStockage()
        {
            var stock = _context.Stockages
                .Include(s => s.Commune)
                .Include(s => s.Valeur)
                .AsEnumerable() // Convertir en IEnumerable pour pouvoir utiliser l'opérateur de propagation null en mémoire
                .Select(s => new
                {
                    Commune = s.Commune?.Nom ?? "Commune non spécifiée", // Gérer le cas où Commune est null
                    ValeurNom = s.Valeur?.Nom ?? "Valeur non spécifiée",  // Gérer le cas où Valeur est null
                    Quantite = s.QuantiteDisponible
                })
                .ToList();

            StockListGrid.ItemsSource = stock;
        }
        private void FilterStockages(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                // Si la recherche est vide, afficher tous les stockages
                ViewStockage();
            }
            else
            {
                var filteredStockage = _context.Stockages
                   .Include(s => s.Commune)
                   .Include(s => s.Valeur)
                   .ThenInclude(v => v.TypeValeur) // Inclure TypeValeur pour le nom de valeur
                   .AsEnumerable()
                   .Where(s =>
                       (s.Commune?.Nom?.ToLower().Contains(searchText.ToLower()) ?? false) ||
                       (s.Valeur?.Nom?.ToLower().Contains(searchText.ToLower()) ?? false)
                   )
                   .Select(s => new
                   {
                       Commune = s.Commune?.Nom ?? "Commune non spécifiée",
                       ValeurNom = s.Valeur?.Nom ?? "Valeur non spécifiée",
                       Quantite = s.QuantiteDisponible
                   })
                   .ToList();

                StockListGrid.ItemsSource = filteredStockage;
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
                        FilterStockages(textBox.Text);
                    });
                }, null, 300, System.Threading.Timeout.Infinite);
            }
        }

   
    }
}