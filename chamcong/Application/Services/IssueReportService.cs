using chamcong.Application.Common;
using chamcong.Application.DTOs;
using chamcong.Application.Interfaces;
using chamcong.Domain.Entities;

namespace chamcong.Application.Services
{
    public class IssueReportService : IIssueReportService
    {
        private readonly IUnitOfWork _unitOfWork;

        public IssueReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> CreateReportAsync(int employeeId, CreateIssueReportDto dto)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(employeeId);
            if (employee == null) return Result<int>.Failure("Không tìm thấy nhân viên.", 404);

            var report = new IssueReport
            {
                EmployeeId = employeeId,
                Title = dto.Title,
                Description = dto.Description,
                Status = 0, // Pending
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.IssueReports.AddAsync(report);
            await _unitOfWork.SaveChangesAsync();

            return Result<int>.Ok(report.Id);
        }

        public async Task<Result<List<IssueReportDto>>> GetPendingReportsAsync()
        {
            var reports = await _unitOfWork.IssueReports.FindAsync(r => r.Status == 0);
            
            var resultList = new List<IssueReportDto>();
            foreach(var r in reports)
            {
                var emp = await _unitOfWork.Employees.GetByIdAsync(r.EmployeeId);
                resultList.Add(new IssueReportDto
                {
                    Id = r.Id,
                    EmployeeId = r.EmployeeId,
                    EmployeeName = emp?.FullName ?? "Unknown",
                    Title = r.Title,
                    Description = r.Description,
                    Status = r.Status,
                    CreatedAt = r.CreatedAt
                });
            }

            return Result<List<IssueReportDto>>.Ok(resultList);
        }

        public async Task<Result<bool>> ResolveReportAsync(int id)
        {
            var report = await _unitOfWork.IssueReports.GetByIdAsync(id);
            if (report == null) return Result<bool>.Failure("Không tìm thấy báo cáo.", 404);

            report.Status = 1; // Resolved
            _unitOfWork.IssueReports.Update(report);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }
    }
}
