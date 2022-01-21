using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace Models
{
    [Table("Planinar")]
    public class Planinar
    {
        [Key]   
        public int IDPlaninara { get; set; }
        
        [MaxLength(30)]   
        [Required]       
        public String Ime { get; set; }

        [MaxLength(30)]
        [Required]
        public String Prezime { get; set; }
        
        [Required]
        public int JMBG { get; set; }

        [Range(0,10)]
        public int Spremnost { get; set; }

        [MaxLength(40)]
        [Required]
        public String Grad { get; set; }

        [MaxLength(40)]
        [Required]
        public String Drzava { get; set; }
        
        public PlaninarskoDrustvo IDPlaninarskogDrustva { get; set; }
        [JsonIgnore]
        public List<IdeNaDogadjaj> Dogadjaji { get; set; }
    }
}