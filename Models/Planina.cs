using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace Models
{
    public class Planina
    {
        [Key]
        public int IDPlanine { get; set; }

        [Required]
        [MaxLength(50)]
        public String ImePlanine { get; set; }

        [Required]
        [MaxLength(50)]
        public String Drzava { get; set; }

        [Required]
        public int MaksimalnaVisina { get; set; }
        
        [Required]
        [MaxLength(50)]
        public String ImeNajvisegVrha { get; set; }
        
        [Required]
        [Range(0,10)]
        public int TezinaPlanine { get; set; }
        [JsonIgnore]
        public List<Dogadjaj> Dogadjaj { get; set; }
    }
}