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
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Network> Networks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Setup(modelBuilder.Entity<Episode>());
            Setup(modelBuilder.Entity<Show>());
            Setup(modelBuilder.Entity<Network>());
            Setup(modelBuilder.Entity<Genre>());
        }

        private void Setup(EntityTypeConfiguration<Network> entityConfig)
        {
            entityConfig.ToTable("Network").HasKey(x => x.Oid);
        }

        private void Setup(EntityTypeConfiguration<Genre> entityConfig)
        {
            entityConfig.ToTable("Genre").HasKey(x => x.Oid);
        }

        private void Setup(EntityTypeConfiguration<Show> entityConfig)
        {
            entityConfig.ToTable("Show").HasKey(x => x.Oid);

            entityConfig
                .HasMany<Genre>(s => s.Genres)
                .WithMany(s => s.Shows)
                .Map(s =>
                {
                    s.MapLeftKey("Show");
                    s.MapRightKey("Genre");
                    s.ToTable("ShowGenre");
                });

            entityConfig
                .HasOptional(s => s.Network)
                .WithMany(s => s.Shows)
                .HasForeignKey(s => s.NetworkId);

            entityConfig.Property(s => s.NetworkId).HasColumnName("Network");
        }

        private void Setup(EntityTypeConfiguration<Episode> entityConfig)
        {
            entityConfig.ToTable("episode").HasKey(x => x.Oid);

            entityConfig
                .HasRequired(s => s.Show)
                .WithMany(s => s.Episodes)
                .HasForeignKey(s => s.ShowId);

            entityConfig.Property(s => s.ShowId).HasColumnName("Show");
        }
    }
}
