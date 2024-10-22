using gvi.Data;
using gvi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Logique d'interaction pour ajout_entree.xaml
    /// </summary>
    public partial class ajout_entree : Window
    {
        private readonly DataContext _context;
        private Entree _entree;
        private bool champEstRempli = false;
        public ObservableCollection<EntreeValeur> ValeursSelectionnees { get; set; }

        public ajout_entree(DataContext context, Entree entree)
        {
            InitializeComponent();
            _context = context;

            ValeursSelectionnees = new ObservableCollection<EntreeValeur>();
            DataContext = this;
            LoadCommunes();
            LoadValeursDisponibles();
            _entree = entree;
            if (_context != null && _entree != null)
            {
                btnannuler.Content = "Supprimer";
                RemplirChamp();
            }
        }
        private void RemplirChamp()
        {
            ComboBoxCommune.SelectedValue = _entree.CommuneId;
            datepic.SelectedDate = _entree.DateEntree;

            // Utilisez _entree.Valeurs directement car c'est une collection
            foreach (EntreeValeur valeur in _entree.Valeurs)
            {
                ValeursSelectionnees.Add(valeur);
            }

            champEstRempli = true;
        }


        private void LoadCommunes()
        {
            var communes = _context.Communes.ToList();
            ComboBoxCommune.ItemsSource = communes;
            ComboBoxCommune.DisplayMemberPath = "Nom";
            ComboBoxCommune.SelectedValuePath = "Id";
        }

        private void LoadValeursDisponibles()
        {
            var valeurs = _context.Valeurs.Include(v => v.TypeValeur).ToList();
            ValeursList.ItemsSource = valeurs;
            ValeursList.DisplayMemberPath = "Nom";
            ValeursList.SelectedValuePath = "Id";
        }

        private void ValeursList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ValeursList.SelectedItem is Valeur valeurSelectionnee)
            {
                var existingEntree = ValeursSelectionnees.FirstOrDefault(v => v.Valeur.Id == valeurSelectionnee.Id);
                if (existingEntree == null)
                {
                    var nouvelleEntree = new EntreeValeur
                    {
                        Valeur = valeurSelectionnee,
                        Quantite = 1,
                        MontantTotal = valeurSelectionnee.montant * 1
                    };
                    ValeursSelectionnees.Add(nouvelleEntree);
                }
                else
                {
                    MessageBox.Show("Cette valeur est déjà dans la liste des valeurs sélectionnées.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void ValeursSelectionneesGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
        }

        private void ValeursSelectionneesGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit && e.Column.Header.ToString() == "Quantité")
            {
                var entreeValeur = e.Row.Item as EntreeValeur;
                if (entreeValeur != null)
                {
                    var editedTextBox = e.EditingElement as TextBox;
                    if (int.TryParse(editedTextBox.Text, out int newQuantite))
                    {
                        // Mettre à jour la quantité et le montant total
                        entreeValeur.Quantite = newQuantite;
                        entreeValeur.MontantTotal = entreeValeur.CalculateMontantTotal();

                        // Forcer la mise à jour de la colonne MontantTotal
                        ValeursSelectionneesGrid.UpdateLayout();
                    }
                }
            }
        }


      
        private void Modification()
        {
            if (ComboBoxCommune.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner une commune.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (ValeursSelectionnees.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner au moins une valeur.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            // Mise à jour des champs de l'entrée existante
            _entree.CommuneId = (int)ComboBoxCommune.SelectedValue;
            _entree.DateEntree = datepic.SelectedDate ?? DateTime.Now;

            // Suppression des anciennes valeurs associées
            _entree.Valeurs.Clear();

            // Ajout des nouvelles valeurs
            foreach (var valeurSelectionnee in ValeursSelectionnees)
            {
                _entree.Valeurs.Add(new EntreeValeur
                {
                    ValeurInactiveId = valeurSelectionnee.Valeur.Id,
                    Quantite = valeurSelectionnee.Quantite,
                    MontantTotal = valeurSelectionnee.MontantTotal
                });
            }

            // Enregistrer les changements
            _context.Entry(_entree).State = EntityState.Modified;
            _context.SaveChanges();

            MessageBox.Show("L'entrée a été modifiée avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void btnEnregistrer_Click(object sender, RoutedEventArgs e)
        {
            if (champEstRempli)
            {
                // Si on modifie une entrée existante
                Modification();
            }
            else
            {
                if (ComboBoxCommune.SelectedItem == null)
                {
                    MessageBox.Show("Veuillez sélectionner une commune.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (ValeursSelectionnees.Count == 0)
                {
                    MessageBox.Show("Veuillez sélectionner au moins une valeur.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var nouvelleEntree = new Entree
                {
                    CommuneId = (int)ComboBoxCommune.SelectedValue,
                    DateEntree = DateTime.Now,
                    Valeurs = new List<EntreeValeur>()
                };

                foreach (var valeurSelectionnee in ValeursSelectionnees)
                {
                    nouvelleEntree.Valeurs.Add(new EntreeValeur
                    {
                        ValeurInactiveId = valeurSelectionnee.Valeur.Id,
                        Quantite = valeurSelectionnee.Quantite,
                        MontantTotal = valeurSelectionnee.MontantTotal
                    });

                    // Vérifier si un stockage existe pour cette commune et cette valeur inactive
                    var stockageExistant = _context.Stockages
                        .FirstOrDefault(s => s.CommuneId == (int)ComboBoxCommune.SelectedValue && s.ValeurId == valeurSelectionnee.Valeur.Id);

                    if (stockageExistant == null)
                    {
                        // Si aucun stockage existant, créer un nouveau stockage
                        Stockage nouveauStockage = new Stockage
                        {
                            ValeurId = valeurSelectionnee.Valeur.Id,
                            CommuneId = (int)ComboBoxCommune.SelectedValue,
                            QuantiteDisponible = valeurSelectionnee.Quantite
                        };
                        _context.Stockages.Add(nouveauStockage);
                    }
                    else
                    {
                        // Si un stockage existe déjà, on met à jour la quantité disponible
                        stockageExistant.QuantiteDisponible += valeurSelectionnee.Quantite;
                        _context.Entry(stockageExistant).State = EntityState.Modified;
                    }
                }

                // Sauvegarder la nouvelle entrée
                _context.Entrees.Add(nouvelleEntree);
                _context.SaveChanges();

                MessageBox.Show("L'entrée a été enregistrée avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }

        private void ValeursSelectionneesGrid_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {

            if (ValeursSelectionneesGrid.SelectedItem is EntreeValeur entreeValeur)
            {
                ValeursSelectionnees.Remove(entreeValeur);
            }
        }

        private void btnannuler_Click_1(object sender, RoutedEventArgs e)
        {
            if (champEstRempli == true)
            {
                MessageBoxResult result = MessageBox.Show("Voulez-vous vraiment supprimer cette entrée ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    // Parcourir les valeurs associées à cette entrée pour mettre à jour les quantités dans le stockage
                    foreach (var valeur in _entree.Valeurs)
                    {
                        // Récupérer le stockage correspondant pour la commune et la valeur inactive
                        var stockageExistant = _context.Stockages
                            .FirstOrDefault(s => s.CommuneId == _entree.CommuneId && s.ValeurId == valeur.ValeurInactiveId);

                        if (stockageExistant != null)
                        {
                            // Réduire la quantité disponible dans le stockage
                            stockageExistant.QuantiteDisponible -= valeur.Quantite;

                            // Si la quantité devient négative, la remettre à 0 (ou gérer selon vos besoins)
                            if (stockageExistant.QuantiteDisponible < 0)
                            {
                                stockageExistant.QuantiteDisponible = 0;
                            }

                            _context.Entry(stockageExistant).State = EntityState.Modified;
                        }
                    }

                    // Supprimer l'entrée
                    _context.Entrees.Remove(_entree);

                    // Sauvegarder les changements
                    _context.SaveChanges();

                    MessageBox.Show("L'entrée a été supprimée avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Aucune entrée à supprimer.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            this.Close();
        }
    }

}