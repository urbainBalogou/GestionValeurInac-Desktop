
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gvi.Models
{
    public class Commune
    {

        [Key]
        public int Id { get; set; }

        [Required]

        [MaxLength(10)]
        public string CodeCommune { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nom { get; set; }

        [Required]
        [EmailAddress]
        public string Mail { get; set; }

        [Required]
        public string Adresse { get; set; }

        // Relations
        public ICollection<Employe> Employes { get; set; }

        public override string ToString()
        {
            return Nom;
        }
    }
}


