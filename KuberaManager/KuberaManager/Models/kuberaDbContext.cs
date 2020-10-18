using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KuberaManager.Models
{
    public partial class kuberaDbContext : DbContext
    {
        // Entities
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Computer> Computers { get; set; }
        public DbSet<Scenario> Scenarios { get; set; }
        public DbSet<Session> Sessions { get; set; }


        public kuberaDbContext()
        {
        }

        public kuberaDbContext(DbContextOptions<kuberaDbContext> options)
            : base(options)
        {
        }

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
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
