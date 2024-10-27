using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gvi.Models
{
    public class Sortie
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Commune")]
        public int CommuneId { get; set; }
        public virtual Commune Commune { get; set; }

        [ForeignKey("Employe")]
        public int EmployeId { get; set; }
        public virtual Employe Employe { get; set; }

        [ForeignKey("Demande")]
        public int DemandeId { get; set; }
        public virtual Demande Demande { get; set; }
        public virtual ICollection<SortieValeur> Valeurs { get; set; }

        public DateTime DateSortie { get; set; } = DateTime.Now;

        public void MarkDemandeAsSortie()
        {
            if (Demande != null)
            {
                Demande.EstSortie = true;
            }
        }

        public override string ToString()
        {
            return $"Sortie #{Id} - {Commune.Nom}";
        }
    }

    public class SortieValeur : INotifyPropertyChanged
    {
        private int _quantite;
        private int _montantTotal;
        private Valeur _valeur;

        [Key]
        public int Id { get; set; }

        [ForeignKey("Sortie")]
        public int SortieId { get; set; }
        public virtual Sortie Sortie { get; set; }

        [ForeignKey("Valeur")]
        public int ValeurId { get; set; }

        public virtual Valeur Valeur
        {
            get => _valeur;
            set
            {
                if (_valeur != value)
                {
                    _valeur = value;
                    OnPropertyChanged(nameof(Valeur));
                    MontantTotal = CalculateMontantTotal(); // Recalcule le montant si la valeur change
                }
            }
        }

        public int Quantite
        {
            get => _quantite;
            set
            {
                if (_quantite != value)
                {
                    _quantite = value;
                    OnPropertyChanged(nameof(Quantite));
                    MontantTotal = CalculateMontantTotal(); // Recalcule le montant si la quantité change
                }
            }
        }

        public int MontantTotal
        {
            get => _montantTotal;
            set
            {
                if (_montantTotal != value)
                {
                    _montantTotal = value;
                    OnPropertyChanged(nameof(MontantTotal));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Calcul du montant total en fonction des propriétés Valeur et Quantité
        public int CalculateMontantTotal()
        {
            return Valeur != null && Valeur.TypeValeur != null
                ? Valeur.nombre_de_valeur_par_feuillet_ou_carnet * Quantite * Valeur.nombre_de_feuillets * Valeur.TypeValeur.ValeurFaciale
                : 0;
        }

        // Validation des données (quantité et montant total)
        public void Validate()
        {
            if (Quantite <= 0)
                throw new ValidationException("La quantité doit être supérieure à 0.");
            if (MontantTotal < 0)
                throw new ValidationException("Le montant total doit être supérieur à 0.");
        }

        public override string ToString()
        {
            return $"{Valeur?.TypeValeur} - {Quantite}";
        }
    }
}