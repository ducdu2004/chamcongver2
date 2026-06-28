using chamcong.Domain.Entities;

namespace chamcong.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Workshop> Workshops { get; }
        IGenericRepository<Employee> Employees { get; }
        IGenericRepository<Product> Products { get; }
        IGenericRepository<Batch> Batches { get; }
        IGenericRepository<Bundle> Bundles { get; }
        IGenericRepository<ProductionLog> ProductionLogs { get; }
        IGenericRepository<AttendanceLog> AttendanceLogs { get; }
        IGenericRepository<FingerprintLog> FingerprintLogs { get; }
        IGenericRepository<Account> Accounts { get; }
        IGenericRepository<IssueReport> IssueReports { get; }
        IGenericRepository<EmploymentHistory> EmploymentHistories { get; }
        IGenericRepository<GarmentPart> GarmentParts { get; }
        
        Task<int> SaveChangesAsync();
    }
}
