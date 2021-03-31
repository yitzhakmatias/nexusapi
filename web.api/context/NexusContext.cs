using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace web.api.context
{
 
    public class NexusContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Bill> Bills { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(@"Data Source=C:nexus.db");
        public NexusContext(DbContextOptions<NexusContext> options) : base(options)
        {
        }
    }

    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Bill> Bills { get; } = new List<Bill>();
    }

    public class Bill
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public DateTime Period { get; set; }
        public string State { get; set; }
        public decimal Amount { get; set; }

        public int ClientId { get; set; }
 
       
        public Client Client { get; set; }
    }
}
