using Showrunner.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Showrunner.Data.DatabaseConnection
{
    public class ShowrunnerDbContext : DbContext
    {
        public ShowrunnerDbContext(DbConnection connection) : base(connection, true)
        {
            Database.SetInitializer<ShowrunnerDbContext>(null);
        }

        public virtual DbSet<Show> Shows { get; set; }
        public virtual DbSet<Episode> Episodes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Setup(modelBuilder.Entity<Episode>());
            Setup(modelBuilder.Entity<Show>());
        }

        private void Setup(EntityTypeConfiguration<Show> entityConfig)
        {
            entityConfig.ToTable("Show").HasKey(x => x.Oid);
        }

        private void Setup(EntityTypeConfiguration<Episode> entityConfig)
        {
            entityConfig.ToTable("episode").HasKey(x => x.Oid);

            entityConfig
                .HasRequired(s => s.Show)
                .WithMany()
                .HasForeignKey(s => s.ShowId);

            entityConfig.Property(s => s.ShowId).HasColumnName("Show");
        }
    }
}
