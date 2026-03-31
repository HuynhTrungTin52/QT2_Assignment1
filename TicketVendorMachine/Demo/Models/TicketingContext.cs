using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Demo.Models
{
    public class TicketingContext : DbContext
    {
        public TicketingContext() : base("name=TicketingDB") { }

        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}