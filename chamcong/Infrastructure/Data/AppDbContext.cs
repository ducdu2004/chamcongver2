using chamcong.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace chamcong.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Workshop> Workshops { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Batch> Batches { get; set; }
        public DbSet<Bundle> Bundles { get; set; }
        public DbSet<ProductionLog> ProductionLogs { get; set; }
        public DbSet<AttendanceLog> AttendanceLogs { get; set; }
        public DbSet<FingerprintLog> FingerprintLogs { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<IssueReport> IssueReports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Self-Referencing for Batch
            modelBuilder.Entity<Batch>()
                .HasOne(b => b.ParentBatch)
                .WithMany(b => b.SubBatches)
                .HasForeignKey(b => b.ParentBatchId)
                .OnDelete(DeleteBehavior.Restrict);

            // Restrict cascading delete for safety in production
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Workshop)
                .WithMany(w => w.Employees)
                .HasForeignKey(e => e.WorkshopId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Batch>()
                .HasOne(b => b.Product)
                .WithMany(p => p.Batches)
                .HasForeignKey(b => b.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Batch>()
                .HasOne(b => b.AssignedWorkshop)
                .WithMany()
                .HasForeignKey(b => b.AssignedWorkshopId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bundle>()
                .HasOne(b => b.Batch)
                .WithMany(ba => ba.Bundles)
                .HasForeignKey(b => b.BatchId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductionLog>()
                .HasOne(p => p.Employee)
                .WithMany(e => e.ProductionLogs)
                .HasForeignKey(p => p.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductionLog>()
                .HasOne(p => p.Bundle)
                .WithOne(b => b.ProductionLog)
                .HasForeignKey<ProductionLog>(p => p.BundleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AttendanceLog>()
                .HasOne(a => a.Employee)
                .WithMany(e => e.AttendanceLogs)
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // FingerprintId should be Unique
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.FingerprintId)
                .IsUnique()
                .HasFilter("[FingerprintId] IS NOT NULL");

            // Optional: Username should be unique
            modelBuilder.Entity<Account>()
                .HasIndex(a => a.Username)
                .IsUnique();
        }
    }
}
