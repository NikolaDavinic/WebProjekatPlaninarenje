using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace Models
{
    public class Dogadjaj
    {
        [Key]
        public int IDDogadjaja { get; set; }

        [Required]
        [MaxLength(50)]
        public String ImeDogadjaja { get; set; }

        [Required]
        [MaxLength(30)]
        public String ImeVrhaDogadjaja { get; set; }

        [Required]
        public DateTime DatumOdrzavanja { get; set; }

        [Required]
        [JsonIgnore]
        public Planina Planina { get; set; }

        [Required]
        [Range(0,10)]
        public int TezinaUspona { get; set; }

        [Required]
        public int BrojUcesnika { get; set; }

        [JsonIgnore]
        public  List<IdeNaDogadjaj> Planinari { get; set; }
        
        public List<Organizuje> PlaninarskaDrustva { get; set; }
    }
}