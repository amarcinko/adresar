using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Adresar.Models
{
    public class AdresarContext : DbContext
    {
        public AdresarContext() : base("AdresarContext")
        {
        }

        public DbSet<Kontakt> Kontakti { get; set; }
        public DbSet<Unos> Brojevi { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}