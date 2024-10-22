using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gvi.Models
{
    public class TypeValeur
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(250)]
        public string Nature { get; set; }
        [Required]
        public int ValeurFaciale { get; set; }
        public ICollection<Valeur> Valeurs { get; set; }
        public string NatureValeurFaciale => $"{Nature} - {ValeurFaciale}";
    }
}
