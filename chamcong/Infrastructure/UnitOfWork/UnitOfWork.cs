using chamcong.Application.Interfaces;
using chamcong.Domain.Entities;
using chamcong.Infrastructure.Data;
using chamcong.Infrastructure.Repositories;

namespace chamcong.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IGenericRepository<Workshop>? _workshops;
        private IGenericRepository<Employee>? _employees;
        private IGenericRepository<Product>? _products;
        private IGenericRepository<Batch>? _batches;
        private IGenericRepository<Bundle>? _bundles;
        private IGenericRepository<ProductionLog>? _productionLogs;
        private IGenericRepository<AttendanceLog>? _attendanceLogs;
        private IGenericRepository<FingerprintLog>? _fingerprintLogs;
        private IGenericRepository<Account>? _accounts;
        private IGenericRepository<IssueReport>? _issueReports;
        private IGenericRepository<EmploymentHistory>? _employmentHistories;
        private IGenericRepository<GarmentPart>? _garmentParts;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<Workshop> Workshops => _workshops ??= new GenericRepository<Workshop>(_context);
        public IGenericRepository<Employee> Employees => _employees ??= new GenericRepository<Employee>(_context);
        public IGenericRepository<Product> Products => _products ??= new GenericRepository<Product>(_context);
        public IGenericRepository<Batch> Batches => _batches ??= new GenericRepository<Batch>(_context);
        public IGenericRepository<Bundle> Bundles => _bundles ??= new GenericRepository<Bundle>(_context);
        public IGenericRepository<ProductionLog> ProductionLogs => _productionLogs ??= new GenericRepository<ProductionLog>(_context);
        public IGenericRepository<AttendanceLog> AttendanceLogs => _attendanceLogs ??= new GenericRepository<AttendanceLog>(_context);
        public IGenericRepository<FingerprintLog> FingerprintLogs => _fingerprintLogs ??= new GenericRepository<FingerprintLog>(_context);
        public IGenericRepository<Account> Accounts => _accounts ??= new GenericRepository<Account>(_context);
        public IGenericRepository<IssueReport> IssueReports => _issueReports ??= new GenericRepository<IssueReport>(_context);
        public IGenericRepository<EmploymentHistory> EmploymentHistories => _employmentHistories ??= new GenericRepository<EmploymentHistory>(_context);
        public IGenericRepository<GarmentPart> GarmentParts => _garmentParts ??= new GenericRepository<GarmentPart>(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
