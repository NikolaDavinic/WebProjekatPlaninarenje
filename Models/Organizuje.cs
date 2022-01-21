using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace Models
{
    public class Organizuje
    {
        [Key]
        public int ID { get; set; }

        public PlaninarskoDrustvo PlaninarskaDrustva { get; set; }

        public Dogadjaj Dogadjaji { get; set; }
    }
}