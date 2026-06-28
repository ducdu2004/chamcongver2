using chamcong.Application.Common;
using chamcong.Application.DTOs;
using chamcong.Application.Interfaces;
using chamcong.Domain.Entities;

namespace chamcong.Application.Services
{
    public class ProductionService : IProductionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> GenerateBundlesAsync(int batchId, int numberOfBundles, string stepName, decimal stepPrice, int quantityPerBundle)
        {
            var batch = await _unitOfWork.Batches.GetByIdAsync(batchId);
            if (batch == null) return Result.Failure("Batch not found", 404);

            for (int i = 1; i <= numberOfBundles; i++)
            {
                var bundle = new Bundle
                {
                    BatchId = batchId,
                    BundleNumber = i,
                    StepName = stepName,
                    StepPrice = stepPrice,
                    Quantity = quantityPerBundle,
                    Status = 0 // New
                };
                await _unitOfWork.Bundles.AddAsync(bundle);
            }

            await _unitOfWork.SaveChangesAsync();
            return Result.Ok("Bundles generated successfully");
        }

        public async Task<Result> CompleteBundleAsync(int bundleId, int employeeId)
        {
            var bundle = await _unitOfWork.Bundles.GetByIdAsync(bundleId);
            if (bundle == null) return Result.Failure("Bundle not found", 404);

            // BR04: Check if already completed
            var existingLog = await _unitOfWork.ProductionLogs.FindAsync(p => p.BundleId == bundleId);
            if (existingLog.Any())
            {
                return Result.Failure("Bundle already completed", 409); // Conflict
            }

            bundle.Status = 2; // Completed
            _unitOfWork.Bundles.Update(bundle);

            // BR03: Calculate earned amount based on bundle StepPrice
            var productionLog = new ProductionLog
            {
                EmployeeId = employeeId,
                BundleId = bundleId,
                CompletedAt = DateTime.Now,
                EarnedAmount = bundle.Quantity * bundle.StepPrice
            };

            await _unitOfWork.ProductionLogs.AddAsync(productionLog);
            await _unitOfWork.SaveChangesAsync();

            return Result.Ok("Bundle completed successfully");
        }

        public async Task<Result> AddManualProductionLogAsync(ManualProductionCreateDto dto)
        {
            var productionLog = new ProductionLog
            {
                EmployeeId = dto.EmployeeId,
                ProductId = dto.ProductId,
                GarmentPartId = dto.GarmentPartId,
                SizeOrTable = dto.SizeOrTable,
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice,
                EarnedAmount = dto.Quantity * dto.UnitPrice,
                CompletedAt = DateTime.Now
            };

            await _unitOfWork.ProductionLogs.AddAsync(productionLog);
            await _unitOfWork.SaveChangesAsync();

            return Result.Ok("Đã ghi nhận công thành công");
        }

        public async Task<Result<IEnumerable<ManualProductionDto>>> GetManualProductionLogsAsync(int employeeId, DateTime date)
        {
            var logs = await _unitOfWork.ProductionLogs.FindAsync(p => p.EmployeeId == employeeId && p.CompletedAt.Date == date.Date && !p.BundleId.HasValue);
            
            var result = new List<ManualProductionDto>();
            foreach(var log in logs)
            {
                string productName = "N/A";
                string partName = "N/A";

                if (log.ProductId.HasValue)
                {
                    var product = await _unitOfWork.Products.GetByIdAsync(log.ProductId.Value);
                    if (product != null) productName = product.ProductName;
                }

                if (log.GarmentPartId.HasValue)
                {
                    var part = await _unitOfWork.GarmentParts.GetByIdAsync(log.GarmentPartId.Value);
                    if (part != null) partName = part.Name;
                }

                result.Add(new ManualProductionDto
                {
                    Id = log.Id,
                    EmployeeId = log.EmployeeId,
                    ProductName = productName,
                    GarmentPartName = partName,
                    SizeOrTable = log.SizeOrTable,
                    Quantity = log.Quantity,
                    UnitPrice = log.UnitPrice,
                    EarnedAmount = log.EarnedAmount,
                    CompletedAt = log.CompletedAt
                });
            }

            return Result<IEnumerable<ManualProductionDto>>.Ok(result.OrderByDescending(r => r.CompletedAt));
        }
    }
}
