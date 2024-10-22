using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gvi.Models
{
    public class Valeur
    {
        [Key]
       public int Id { get; set; } 
        public int typeValeurId { get; set; }
        [ForeignKey ("typeValeurId")]
        public TypeValeur TypeValeur { get; set; }
        [Required]
        public int nombre_de_valeur_par_feuillet_ou_carnet { get; set; }
        [Required]
        public int nombre_de_feuillets { get; set; }
        [Required]
        public int montant { get; set; }
        
        public void CalculMontant(int valeur, int nombreParFeuillet,int nombreFeuillet)
        {
            montant = valeur * nombreParFeuillet * nombreFeuillet;
        }
        public string Nom => $"{TypeValeur.Nature}-{TypeValeur.ValeurFaciale}({nombre_de_valeur_par_feuillet_ou_carnet}/{nombre_de_feuillets})";
    }

}
