using gvi.Data;
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

namespace gvi.PrintForms
{
    /// <summary>
    /// Logique d'interaction pour SortiePrint.xaml
    /// </summary>
    public partial class SortiePrint : Window
    {
        DataContext _context;
        public SortiePrint()
        {
            InitializeComponent();
            _context = new DataContext();
            LoadCommune();
        }

        private void ExportToPDF_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {

        }
        void LoadCommune()
        {
            var communes = _context.Communes.ToList();
            comboCommune.ItemsSource = communes;
        }
    }
}
