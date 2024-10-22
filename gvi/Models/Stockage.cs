using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gvi.Models
{
    public class Stockage
    {
        [Key]
        public int Id { get; set; }

        public int CommuneId { get; set; }

        [ForeignKey("CommuneId")]
        [Required]
        public Commune Commune { get; set; }

        public int ValeurId { get; set; }

        [ForeignKey("ValeurId")]
        [Required]
        public Valeur Valeur { get; set; }

        public int QuantiteDisponible { get; set; }
    }
}
