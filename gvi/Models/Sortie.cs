using System;
using System.Collections.Generic;
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

    public class SortieValeur
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Sortie")]
        public int SortieId { get; set; }
        public virtual Sortie Sortie { get; set; }

        [ForeignKey("Valeur")]
        public int ValeurId { get; set; }
        public virtual Valeur Valeur { get; set; }

        public int Quantite { get; set; }

        public int MontantTotal { get; set; }
    }

}
