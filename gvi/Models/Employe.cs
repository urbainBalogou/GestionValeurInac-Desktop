using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gvi.Models
{
    // Models/Employe.cs
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class Employe
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CommuneId { get; set; }

        [ForeignKey("CommuneId")]
        public Commune Commune { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nom { get; set; }

        [Required]
        [MaxLength(100)]
        public string Prenom { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "Le numéro de téléphone doit être numérique et contenir 8 chiffres.")]
        public string Telephone { get; set; }

        [Required]
        public int FonctionId { get; set; }

        [ForeignKey("FonctionId")]
        public Fonction Fonction { get; set; }

        //public override string ToString()
        //{
        //    return $"{Nom} {Prenom} - Fonction: {Fonction.Libelle}";
        //}
        public string NomComplet
        {
            get
            {
                return $"{Nom ?? "Nom manquant"} {Prenom ?? "Prénom manquant"}";
            }
        }

    }
}