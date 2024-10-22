using gvi.Data;
using gvi.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace gvi
{
    public partial class ajout_type : Window
    {
        private DataContext _context;
        private TypeValeur _type;

        public ajout_type(DataContext context, TypeValeur type = null)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _type = type;

            if (_type != null)
            {
                RemplirChamp();
                txtannuler.Content = "Supprimer";
            }
            else
            {
                txtannuler.Content = "Annuler";
            }
        }

        private void RemplirChamp()
        {
            txtnature.Text = _type.Nature;
            txtvf.Text = _type.ValeurFaciale.ToString();
        }

        private void Init()
        {
            txtnature.Text = string.Empty;
            txtvf.Text = string.Empty;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!TryGetInputs(out string nature, out int vf))
            {
                MessageBox.Show("Veuillez remplir tous les champs avec des valeurs correctes!", "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_type != null) // Modification
            {
                UpdateTypeValeur(nature, vf);
            }
            else // Création
            {
                CreateTypeValeur(nature, vf);
            }
        }

        private bool TryGetInputs(out string nature, out int vf)
        {
            nature = txtnature.Text.Trim();
            bool isVfValid = int.TryParse(txtvf.Text, out vf);
            return !string.IsNullOrEmpty(nature) && isVfValid;
        }

        private void CreateTypeValeur(string nature, int vf)
        {
            if (_context.TypeValeurs.Any(t => t.Nature == nature && t.ValeurFaciale == vf))
            {
                MessageBox.Show("Un type de valeur de même nature et valeur faciale existe déjà!", "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var nouveauType = new TypeValeur { Nature = nature, ValeurFaciale = vf };
            _context.TypeValeurs.Add(nouveauType);
            SaveChanges("Type de valeur créée avec succès", "Erreur lors de la création");
        }

        private void UpdateTypeValeur(string nature, int vf)
        {
            if (_context.TypeValeurs.Any(t => t.Nature == nature && t.ValeurFaciale == vf && t.Id != _type.Id))
            {
                MessageBox.Show("Un type de valeur de même nature et valeur faciale existe déjà!", "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _type.Nature = nature;
            _type.ValeurFaciale = vf;
            SaveChanges("Type modifié avec succès", "Erreur lors de la modification");
        }

        private void SaveChanges(string successMessage, string errorMessage)
        {
            try
            {
                _context.SaveChanges();
                MessageBox.Show(successMessage, "SUCCES", MessageBoxButton.OK, MessageBoxImage.Information);
                Init();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{errorMessage}: {ex.Message}", "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtannuler_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_type != null)
            {
                var result = MessageBox.Show("Confirmer la suppression", "SUPPRESSION", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    DeleteTypeValeur();
                }
            }
            else
            {
                Init(); // Annuler l'ajout
                Close(); // Fermer la fenêtre si aucun type n'est sélectionné
            }
        }

        private void DeleteTypeValeur()
        {
            var typeToDelete = _context.TypeValeurs.Find(_type.Id);
            if (typeToDelete != null)
            {
                _context.TypeValeurs.Remove(typeToDelete);
                SaveChanges("Type de valeur supprimée avec succès.", "Erreur lors de la suppression");
            }
            else
            {
                MessageBox.Show("Aucun type trouvé avec ce code.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
