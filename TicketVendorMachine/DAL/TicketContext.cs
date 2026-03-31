using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class TicketContext : DbContext
    {
        public TicketContext() : base("name=TicketConnectionString")
        {
        }
        public DbSet<User> Users { get; set; }

    }
}
