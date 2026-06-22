using chamcong.Application.Common;
using chamcong.Application.DTOs;

namespace chamcong.Application.Interfaces
{
    public interface IIssueReportService
    {
        Task<Result<int>> CreateReportAsync(int employeeId, CreateIssueReportDto dto);
        Task<Result<List<IssueReportDto>>> GetPendingReportsAsync();
        Task<Result<bool>> ResolveReportAsync(int id);
    }
}
