using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace Models
{
    public class IdeNaDogadjaj
    {
        [Key]
        public int ID { get; set; }
        
        public Planinar Planinari { get; set; }
        
        public Dogadjaj Dogadjaji { get; set; }
    }
}
