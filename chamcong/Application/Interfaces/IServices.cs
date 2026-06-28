using chamcong.Application.Common;
using chamcong.Application.DTOs;

namespace chamcong.Application.Interfaces
{
    public interface IBatchService
    {
        Task<Result<IEnumerable<BatchDto>>> GetBatchesAsync(int? workshopId, bool isAdmin);
        Task<Result<BatchDto>> CreateBatchAsync(BatchCreateDto dto);
        Task<Result<BatchDto>> CreateSubBatchAsync(SubBatchCreateDto dto);
        Task<Result<ExportVoucherDto>> ExportVoucherAsync(int batchId);
        Task<Result<IEnumerable<BatchEmployeeDto>>> GetEmployeesByBatchAsync(int batchId);
    }

    public interface IProductionService
    {
        Task<Result> GenerateBundlesAsync(int batchId, int numberOfBundles, string stepName, decimal stepPrice, int quantityPerBundle);
        Task<Result> CompleteBundleAsync(int bundleId, int employeeId);
        Task<Result> AddManualProductionLogAsync(ManualProductionCreateDto dto);
        Task<Result<IEnumerable<ManualProductionDto>>> GetManualProductionLogsAsync(int employeeId, DateTime date);
    }

    public interface IAttendanceService
    {
        Task<Result> CheckOutAsync(int employeeId);
    }

    public interface IFingerprintService
    {
        Task<Result> ProcessFingerprintScanAsync(string fingerprintId);
    }

    public interface IEmployeeService
    {
        Task<Result<IEnumerable<EmployeeSummaryDto>>> GetEmployeesSummaryAsync();
        Task<Result<IEnumerable<DailyWorkerDto>>> GetDailyWorkersAsync();
        Task<Result<IEnumerable<AttendanceSheetDayDto>>> GetAttendanceSheetAsync(int employeeId, int month, int year);
    }
}
