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
    public partial class ajout_demande : Window
    {
        private readonly DataContext _context;
        private Demande _demande;
        private bool champEstRempli = false;
        public ObservableCollection<DemandeValeur> ValeursSelectionnees { get; set; }

        public ajout_demande(DataContext context, Demande demande)
        {
            InitializeComponent();
            _context = context;

            ValeursSelectionnees = new ObservableCollection<DemandeValeur>();
            DataContext = this;
            LoadCommunes();
            LoadValeursDisponibles();
            _demande = demande;

            if (_context != null && _demande != null)
            {
                //btnannuler.Content = "Supprimer";
                RemplirChamp();
            }
        }

        private void RemplirChamp()
        {
            ComboBoxCommune.SelectedValue = _demande.CommuneId;
            datepic.SelectedDate = _demande.DateDemande;

            // Utilisez _demande.Valeurs directement car c'est une collection
            foreach (DemandeValeur valeur in _demande.Valeurs)
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
                var existingDemande = ValeursSelectionnees.FirstOrDefault(v => v.Valeur.Id == valeurSelectionnee.Id);
                if (existingDemande == null)
                {
                    var nouvelleDemande = new DemandeValeur
                    {
                        Valeur = valeurSelectionnee,
                        Quantite = 1,
                        MontantTotal = valeurSelectionnee.montant * 1
                    };
                    ValeursSelectionnees.Add(nouvelleDemande);
                }
                else
                {
                    MessageBox.Show("Cette valeur est déjà dans la liste des valeurs sélectionnées.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void ValeursSelectionneesGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Implémentez ici la logique pour supprimer une valeur sélectionnée
            if (ValeursSelectionneesGrid.SelectedItem is DemandeValeur demandeValeur)
            {
                ValeursSelectionnees.Remove(demandeValeur);
            }
        }

        private void ValeursSelectionneesGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit && e.Column.Header.ToString() == "Quantité")
            {
                var demandeValeur = e.Row.Item as DemandeValeur;
                if (demandeValeur != null)
                {
                    var editedTextBox = e.EditingElement as TextBox;
                    if (int.TryParse(editedTextBox.Text, out int newQuantite))
                    {
                        // Mettre à jour la quantité et le montant total
                        demandeValeur.Quantite = newQuantite;
                        demandeValeur.CalculateMontantTotal();

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

            // Mise à jour des champs de la demande existante
            _demande.CommuneId = (int)ComboBoxCommune.SelectedValue;
            _demande.DateDemande = datepic.SelectedDate ?? DateTime.Now;

            // Suppression des anciennes valeurs associées
            _demande.Valeurs.Clear();

            // Ajout des nouvelles valeurs
            foreach (var valeurSelectionnee in ValeursSelectionnees)
            {
                _demande.Valeurs.Add(new DemandeValeur
                {
                    ValeurId = valeurSelectionnee.Valeur.Id,
                    Quantite = valeurSelectionnee.Quantite,
                    MontantTotal = valeurSelectionnee.MontantTotal
                });
            }

            // Enregistrer les changements
            _context.Entry(_demande).State = EntityState.Modified;
            _context.SaveChanges();

            MessageBox.Show("La demande a été modifiée avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void btnEnregistrer_Click(object sender, RoutedEventArgs e)
        {
            if (champEstRempli)
            {
                // Si on modifie une demande existante
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

                var nouvelleDemande = new Demande
                {
                    CommuneId = (int)ComboBoxCommune.SelectedValue,
                    DateDemande = DateTime.Now,
                    Valeurs = new List<DemandeValeur>()
                };

                foreach (var valeurSelectionnee in ValeursSelectionnees)
                {
                    nouvelleDemande.Valeurs.Add(new DemandeValeur
                    {
                        ValeurId = valeurSelectionnee.Valeur.Id,
                        Quantite = valeurSelectionnee.Quantite,
                        MontantTotal = valeurSelectionnee.MontantTotal
                    });
                }

                // Sauvegarder la nouvelle demande
                _context.Demandes.Add(nouvelleDemande);
                _context.SaveChanges();

                MessageBox.Show("La demande a été enregistrée avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }

        private void btnannuler_Click(object sender, RoutedEventArgs e)
        {
            if (champEstRempli)
            {
                MessageBoxResult result = MessageBox.Show("Voulez-vous vraiment supprimer cette demande ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    // Supprimer la demande
                    _context.Demandes.Remove(_demande);
                    _context.SaveChanges();

                    MessageBox.Show("La demande a été supprimée avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Aucune demande à supprimer.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
