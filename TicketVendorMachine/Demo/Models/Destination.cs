using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Demo.Models
{
    [Table("Destinations")]
    public class Destination
    {
        [Key]
        public int DestinationID { get; set; }

        [Required]
        [StringLength(100)]
        public string StationName { get; set; }

        public int Zone { get; set; }
        public decimal BaseFare { get; set; }

        public decimal GetEffectiveBaseFare()
        {
            return BaseFare;
        }
    }
}