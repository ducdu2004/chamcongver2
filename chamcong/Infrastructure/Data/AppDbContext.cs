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
        public DbSet<EmploymentHistory> EmploymentHistories { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<GarmentPart> GarmentParts { get; set; }
        public DbSet<ProductComponent> ProductComponents { get; set; }
        public DbSet<Distributor> Distributors { get; set; }

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

            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductCategory)
                .WithMany(pc => pc.Products)
                .HasForeignKey(p => p.ProductCategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Distributor)
                .WithMany(d => d.Products)
                .HasForeignKey(p => p.DistributorId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<GarmentPart>()
                .HasOne(gp => gp.ProductCategory)
                .WithMany(pc => pc.GarmentParts)
                .HasForeignKey(gp => gp.ProductCategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductComponent>()
                .HasOne(pc => pc.Product)
                .WithMany(p => p.ProductComponents)
                .HasForeignKey(pc => pc.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductComponent>()
                .HasOne(pc => pc.GarmentPart)
                .WithMany(gp => gp.ProductComponents)
                .HasForeignKey(pc => pc.GarmentPartId)
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
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductionLog>()
                .HasOne(p => p.Product)
                .WithMany()
                .HasForeignKey(p => p.ProductId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductionLog>()
                .HasOne(p => p.GarmentPart)
                .WithMany()
                .HasForeignKey(p => p.GarmentPartId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AttendanceLog>()
                .HasOne(a => a.Employee)
                .WithMany(e => e.AttendanceLogs)
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EmploymentHistory>()
                .HasOne(eh => eh.Employee)
                .WithMany(e => e.EmploymentHistories)
                .HasForeignKey(eh => eh.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

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
