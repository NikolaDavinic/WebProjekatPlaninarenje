using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace Models
{
    public class PlaninarskoDrustvo
    {
        [Key]
        public int IDPlaninarskogDrustva { get; set; }

        [MaxLength(50)]
        [Required]
        public String ImePlaninarskogDrustva { get; set; }

        [MaxLength(40)]
        [Required]
        public String Grad { get; set; }

        [Required]
        [MaxLength(40)]
        public String Drzava { get; set; }

        [Required]
        [Range(1,10000)]
        public int BrojClana { get; set; }

        [Required]
        [Range(1,5000)]
        public int GodisnjaClanarina { get; set; }
        
        public List<Planinar> Planinari { get; set; }
        public List<Organizuje> Dogadjaji { get; set; }
        
    }
}