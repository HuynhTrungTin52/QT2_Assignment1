using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Demo.Models
{
    [Table("Tickets")]
    public class Ticket
    {
        [Key]
        public int TicketID { get; set; }

        [ForeignKey("Destination")]
        public int DestinationID { get; set; }
        public virtual Destination Destination { get; set; } 

        public bool IsStudentTicket { get; set; }
        public decimal FinalPrice { get; set; }

        [StringLength(255)]
        public string BarcodeData { get; set; }

        public DateTime IssueDate { get; set; } = DateTime.Now;

        public void CalculateFinalPrice(decimal effectiveBaseFare)
        {
            if (IsStudentTicket)
            {
                FinalPrice = effectiveBaseFare * 0.5m; 
            }
            else
            {
                FinalPrice = effectiveBaseFare;
            }
        }

        public void GenerateBarcode()
        {
            BarcodeData = $"TKT-{DestinationID}-{DateTime.Now.Ticks}";
        }
    }
}