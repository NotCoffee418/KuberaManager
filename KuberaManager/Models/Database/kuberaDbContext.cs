using System;
using KuberaManager.Models.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using KuberaManager.Models.PageModels;

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
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<Levels> Levels { get; set; }
        public virtual DbSet<AccountCompletionData> AccountCompletionData { get; set; }
        public virtual DbSet<EventLog> EventLogs { get; set; }
        public virtual DbSet<EventLogText> EventLogTexts { get; set; }


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
                    optionsBuilder.UseMySql(
                        configuration.GetConnectionString("DefaultConnection"),
                        new MySqlServerVersion(new Version(5, 7, 32))
                        );
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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
