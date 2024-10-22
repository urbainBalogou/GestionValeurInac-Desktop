using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gvi.Models
{
    // Models/Fonction.cs
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Fonction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string CodeFonction { get; set; }

        [Required]
        [MaxLength(100)]
        public string Libelle { get; set; }

        // Propriété de navigation pour les employés
        public ICollection<Employe> Employes { get; set; }

        public Fonction()
        {
            Employes = new HashSet<Employe>();
        }

        public override string ToString()
        {
            return Libelle;
        }
    }
}