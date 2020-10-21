using System;
using KuberaManager.Models.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KuberaManager.Models.Database
{
    public partial class kuberaDbContext : DbContext
    {
        public kuberaDbContext()
        {
        }

        public kuberaDbContext(DbContextOptions<kuberaDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Config> Configs { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Computer> Computers { get; set; }
        public virtual DbSet<Scenario> Scenarios { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=kuberamanagerdev;User Id=postgres;Password=postgres");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.Login).IsRequired();

                entity.Property(e => e.Password).IsRequired();
            });

            modelBuilder.Entity<Computer>(entity =>
            {
                entity.Property(e => e.Hostname).IsRequired();
            });

            modelBuilder.Entity<Scenario>(entity =>
            {
                entity.HasIndex(e => e.Id);

                entity.Property(e => e.Name).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
