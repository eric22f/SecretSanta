using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace SecretSanta.Models
{
    public partial class SantaContext : DbContext
    {
        public SantaContext()
        {
        }

        public SantaContext(DbContextOptions<SantaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<SantasTeam> SantasTeams { get; set; }
        public virtual DbSet<SecretSanta> SecretSantas { get; set; }
        public virtual DbSet<SelectedSanta> SelectedSantas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("SantaDB"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SantasTeam>(entity =>
            {
                entity.HasKey(e => e.TeamId);

                entity.Property(e => e.TeamId).ValueGeneratedNever();

                entity.Property(e => e.TeamName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<SecretSanta>(entity =>
            {
                entity.HasKey(e => e.SantaId);

                entity.Property(e => e.AddressAndNotes).HasMaxLength(2000);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.Property(e => e.UserCode).HasMaxLength(50);
            });

            modelBuilder.Entity<SelectedSanta>(entity =>
            {
                entity.HasKey(e => e.SantaId);

                entity.HasIndex(e => e.SelectedSantaId, "IX_SelectedSantasSelections")
                    .IsUnique();

                entity.Property(e => e.SantaId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
