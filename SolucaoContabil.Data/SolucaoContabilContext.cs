using SolucaoContabil.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolucaoContabil.Data
{
    public class SolucaoContabilContext : DbContext
    {
        public SolucaoContabilContext()
            : base("DefaultConnection")
        {

        }

        public DbSet<SpedZ> SpedZ { get; set; }
        public DbSet<SpedA> SpedA { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<ClienteTipo> ClienteTipo { get; set; }
        public DbSet<Lancamento> Lancamentos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Properties<string>()
                .Configure(p => p.HasColumnType("varchar"));
            modelBuilder.Properties<string>()
                .Configure(p => p.HasMaxLength(150));

            //modelBuilder.Entity<SpedA>()
            //    .Property(p => p.Data).HasColumnType("datetime2").HasPrecision(0);

            base.OnModelCreating(modelBuilder);

        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

    }
}
