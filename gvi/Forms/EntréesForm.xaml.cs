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

namespace gvi
{
    /// <summary>
    /// Logique d'interaction pour Entrées.xaml
    /// </summary>
    public partial class EntréesForm : Window
    {
        private DataContext _context;
        private Entree _entree;

        public EntréesForm()
        {
            InitializeComponent();
            _context = new DataContext();
            var valeursEntree = _context.Entrees.Include(e => e.Commune).ToList();
            var valeur = _context.Valeurs
   .Include(v => v.TypeValeur)

   .ToList();
            LoadEntree();
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
        private void txtfonc_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FonctionForm fonc = new FonctionForm();
            fonc.Owner = this;
            fonc.Show();
        }

        private void aje_Click(object sender, RoutedEventArgs e)
        {
            ajout_entree fonc = new ajout_entree(_context, _entree);
            fonc.Owner = this;
            fonc.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void LoadEntree()
        {
            var groupedEntrees = _context.Entrees
    .Include(e => e.Commune) // Inclure la commune
    .Include(e => e.Valeurs) // Inclure les valeurs associées
    .ThenInclude(ev => ev.Valeur) // Inclure les détails de la valeur inactive
    .AsEnumerable() // Convertir en IEnumerable pour utiliser LINQ en mémoire
    .Select(e => new
    {
        Entree = e, // Ajouter l'objet `Entree` ici
        Commune = e.Commune.Nom,
        Valeurs = string.Join(", ", e.Valeurs.Select(v => v.Valeur.TypeValeur.Nature)),
        Quantites = string.Join(", ", e.Valeurs.Select(v => v.Quantite)),
        DateEntree = e.DateEntree
    })
    .ToList();
            listViewEntrees.ItemsSource = groupedEntrees;
        }

        private void btnfresh_Click(object sender, RoutedEventArgs e)
        {
            var valeursEntree = _context.Entrees.Include(e => e.Commune).ToList();
            var valeur = _context.Valeurs
   .Include(v => v.TypeValeur)

   .ToList();
            LoadEntree();
        }

        private void listViewEntrees_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = listViewEntrees.SelectedItem;

            if (selectedItem != null)
            {
                var entree = ((dynamic)selectedItem).Entree; // Récupérer l'objet Entree
                ajout_entree ajout = new ajout_entree(_context, entree);
                ajout.ShowDialog();
                LoadEntree(); // Recharger la liste après modifications éventuelles
            }
        }

    }

}
