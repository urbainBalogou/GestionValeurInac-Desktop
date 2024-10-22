using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gvi.Models
{
        public class Demande
        {
            [Key]
            public int Id { get; set; }

            [ForeignKey("Commune")]
            public int CommuneId { get; set; }
            public virtual Commune Commune { get; set; }

            public DateTime DateDemande { get; set; } = DateTime.Now;

            public bool EstTraitee { get; set; } = false;

            public bool EnCours { get; set; } = true;
            public bool EstSortie { get; set; } = false;

            public DateTime? DateRetrait { get; set; }

            public virtual ICollection<DemandeValeur> Valeurs { get; set; }

            public override string ToString()
            {
                return $"Demande #{Id} par {Commune.Nom}";
            }
        }

        public class DemandeValeur
        {
            [Key]
            public int Id { get; set; }

            [ForeignKey("Demande")]
            public int DemandeId { get; set; }
            public virtual Demande Demande { get; set; }

            [ForeignKey("Valeur")]
            public int ValeurId { get; set; }
            public virtual Valeur Valeur { get; set; }

            public int Quantite { get; set; }

            public int MontantTotal { get; set; }

            public void CalculateMontantTotal()
            {
                MontantTotal = Valeur.nombre_de_valeur_par_feuillet_ou_carnet * Quantite * Valeur.nombre_de_feuillets * Valeur.TypeValeur.ValeurFaciale;
            }

            public override string ToString()
            {
                return $"{Valeur.TypeValeur} - {Quantite}";
            }
        }

    }
