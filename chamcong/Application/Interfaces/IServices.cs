using chamcong.Application.Common;
using chamcong.Application.DTOs;

namespace chamcong.Application.Interfaces
{
    public interface IBatchService
    {
        Task<Result<IEnumerable<BatchDto>>> GetBatchesAsync(int? workshopId, bool isAdmin);
        Task<Result<BatchDto>> CreateSubBatchAsync(SubBatchCreateDto dto);
        Task<Result<ExportVoucherDto>> ExportVoucherAsync(int batchId);
    }

    public interface IProductionService
    {
        Task<Result> GenerateBundlesAsync(int batchId, int numberOfBundles, string stepName, decimal stepPrice, int quantityPerBundle);
        Task<Result> CompleteBundleAsync(int bundleId, int employeeId);
    }

    public interface IAttendanceService
    {
        Task<Result> CheckOutAsync(int employeeId);
    }

    public interface IFingerprintService
    {
        Task<Result> ProcessFingerprintScanAsync(string fingerprintId);
    }
}
