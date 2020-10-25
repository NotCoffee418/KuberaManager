using System;
using KuberaManager.Models.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

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

        public static bool IsUnitTesting { get; set; }

        public virtual DbSet<Config> Configs { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Computer> Computers { get; set; }
        public virtual DbSet<Scenario> Scenarios { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                if (IsUnitTesting)
                {
                    optionsBuilder.UseInMemoryDatabase("KuberaMgrUnitTestDb");
                }
                else
                {
                    IConfigurationRoot configuration = new ConfigurationBuilder()
                        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                        .AddJsonFile("appsettings.json")
                        .Build();
                    optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                }
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
