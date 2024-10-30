using gvi.Data;
using gvi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace gvi
{
    public partial class ajout_sorties : Window
    {
        private readonly DataContext _context;
        private Sortie _sortie;
        private bool champEstRempli = false;
        public ObservableCollection<SortieValeur> ValeursSelectionnees { get; set; }
        public ObservableCollection<Employe> EmployesFiltered { get; set; }
        public ObservableCollection<Demande> DemandesFiltered { get; set; }

        public ajout_sorties(DataContext context, Sortie sortie)
        {
            InitializeComponent();
            _context = context;
            _sortie = sortie;

            ValeursSelectionnees = new ObservableCollection<SortieValeur>();
            EmployesFiltered = new ObservableCollection<Employe>();
            DemandesFiltered = new ObservableCollection<Demande>();

            DataContext = this;
            LoadCommunes();

            if (_sortie != null)
            {
                txtentête.Text = "Modification de Sortie";
                btnannuler.Content = "Supprimer";
                RemplirChamp();
            }
        }

        private void RemplirChamp()
        {
            ComboBoxCommune.SelectedValue = _sortie.CommuneId;
            ComboBoxEmploye.SelectedValue = _sortie.EmployeId;
            ComboBoxDemandes.SelectedValue = _sortie.DemandeId;
            DateSortiePicker.SelectedDate = _sortie.DateSortie;

            foreach (var valeur in _sortie.Valeurs)
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

        private void FilterEmployes(int communeId)
        {
            var employes = _context.Employes
                .Where(e => e.CommuneId == communeId)
                .ToList();

            EmployesFiltered.Clear();
            foreach (var employe in employes)
            {
                EmployesFiltered.Add(employe);
            }

            ComboBoxEmploye.ItemsSource = EmployesFiltered;
            ComboBoxEmploye.DisplayMemberPath = "NomComplet";
            ComboBoxEmploye.SelectedValuePath = "Id";
        }

        private void FilterDemandes(int communeId)
        {
            var demandes = _context.Demandes
                .Where(d => d.CommuneId == communeId && d.EstTraitee && !d.EstSortie)
                .ToList();

            DemandesFiltered.Clear();
            foreach (var demande in demandes)
            {
                DemandesFiltered.Add(demande);
            }

            ComboBoxDemandes.ItemsSource = DemandesFiltered;
            ComboBoxDemandes.DisplayMemberPath = "NomDemande";
            ComboBoxDemandes.SelectedValuePath = "Id";
        }

        private void LoadValeursFromDemande(int demandeId)
        {
            var demande = _context.Demandes
                    .Include(d => d.Valeurs)
                    .ThenInclude(v => v.Valeur)
                    .ThenInclude(v => v.TypeValeur)
                    .FirstOrDefault(d => d.Id == demandeId);

            if (demande != null)
            {
                ValeursSelectionnees.Clear();
                foreach (var demandeValeur in demande.Valeurs)
                {
                    if (demandeValeur.Valeur != null && demandeValeur.Valeur.TypeValeur != null)
                    {
                        ValeursSelectionnees.Add(new SortieValeur
                        {
                            ValeurId = demandeValeur.Valeur.Id,
                            Valeur = demandeValeur.Valeur,
                            Quantite = demandeValeur.Quantite,
                            MontantTotal = demandeValeur.MontantTotal
                        });
                    }
                }
            }
        }

        private void ValeursSelectionneesGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit && e.Column.Header.ToString() == "Quantité")
            {
                var sortieValeur = e.Row.Item as SortieValeur;
                if (sortieValeur != null)
                {
                    var editedTextBox = e.EditingElement as TextBox;
                    if (int.TryParse(editedTextBox.Text, out int newQuantite))
                    {
                        sortieValeur.Quantite = newQuantite;
                        sortieValeur.MontantTotal = sortieValeur.CalculateMontantTotal();
                    }
                }
            }
        }

        private void Modification()
        {
            if (!ValidateInputs()) return;
            
            _sortie.CommuneId = (int)ComboBoxCommune.SelectedValue;
            _sortie.EmployeId = (int)ComboBoxEmploye.SelectedValue;
            _sortie.DemandeId = (int)ComboBoxDemandes.SelectedValue;
            _sortie.DateSortie = DateSortiePicker.SelectedDate ?? DateTime.Now;

            _sortie.Valeurs.Clear();

            foreach (var valeurSelectionnee in ValeursSelectionnees)
            {
                _sortie.Valeurs.Add(new SortieValeur
                {
                    ValeurId = valeurSelectionnee.ValeurId,
                    Quantite = valeurSelectionnee.Quantite,
                    MontantTotal = valeurSelectionnee.MontantTotal
                });
            }
            champEstRempli = true;

            var demande = _context.Demandes.Find(_sortie.DemandeId);
            if (demande != null)
            {
                demande.EstSortie = true;
            }

            _context.Entry(_sortie).State = EntityState.Modified;
            _context.SaveChanges();

            MessageBox.Show("La sortie a été modifiée avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private bool ValidateInputs()
        {
            if (ComboBoxCommune.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner une commune.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (ComboBoxEmploye.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner un employé.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (ComboBoxDemandes.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner une demande traitée.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (ValeursSelectionnees.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner au moins une valeur.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void btnEnregistrer_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs()) return;
            if (champEstRempli == false)
            {
                var valeurs = ValeursSelectionnees
                    .Select(v => (v.ValeurId, v.Quantite))
                    .ToList();
                CreerSortie((int)ComboBoxCommune.SelectedValue, (int)ComboBoxEmploye.SelectedValue, (int)ComboBoxDemandes.SelectedValue, valeurs);
            }
            else Modification();



        }

        public Sortie CreerSortie(int communeId, int employeId, int demandeId, List<(int valeurId, int quantite)> valeurs)
        {
            var sortie = new Sortie
            {
                CommuneId = communeId,
                EmployeId = employeId,
                DemandeId = demandeId,
                DateSortie = DateTime.Now,
                Valeurs = new List<SortieValeur>()
            };

            foreach (var (valeurId, quantite) in valeurs)
            {
                var stockageExistant = _context.Stockages
                    .FirstOrDefault(s => s.CommuneId == communeId && s.ValeurId == valeurId);

                if (stockageExistant != null && stockageExistant.QuantiteDisponible >= quantite)
                {
                    stockageExistant.QuantiteDisponible -= quantite;

                    sortie.Valeurs.Add(new SortieValeur
                    {
                        ValeurId = valeurId,
                        Quantite = quantite
                    });
                }
                else
                {
                    MessageBox.Show($"Quantité insuffisante pour la valeur avec ID: {valeurId}.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
            var demande = _context.Demandes.Find(demandeId);
            if (demande != null)
            {
                demande.EstSortie = true;
            }

            _context.Sorties.Add(sortie);
            _context.SaveChanges();
            MessageBox.Show("La sortie a été créée avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            return sortie;
        }

        private void btnannuler_Click(object sender, RoutedEventArgs e)
        {
            if (_sortie != null)
            {
                foreach (var valeur in _sortie.Valeurs)
                {
                    var stockageExistant = _context.Stockages
                        .FirstOrDefault(s => s.CommuneId == _sortie.CommuneId && s.ValeurId == valeur.ValeurId);

                    if (stockageExistant != null)
                    {
                        stockageExistant.QuantiteDisponible += valeur.Quantite;
                    }
                }
                _sortie.Demande.EstSortie = false;

                _context.Sorties.Remove(_sortie);
                _context.SaveChanges();
                MessageBox.Show("La sortie a été annulée et supprimée.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Aucune sortie n'est en cours de modification.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ComboBoxDemandes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxDemandes.SelectedItem is Demande selectedDemande)
            {
                LoadValeursFromDemande(selectedDemande.Id);
            }
        }

        private void ComboBoxCommune_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ComboBoxCommune.SelectedItem is Commune selectedCommune)
            {
                FilterEmployes(selectedCommune.Id);
                FilterDemandes(selectedCommune.Id);
            }
            
        }

      
    }
}





