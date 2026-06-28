using AutoMapper;
using chamcong.Application.Common;
using chamcong.Application.DTOs;
using chamcong.Application.Interfaces;
using chamcong.Domain.Entities;
using System.Linq;

namespace chamcong.Application.Services
{
    public class BatchService : IBatchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BatchService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<BatchDto>>> GetBatchesAsync(int? workshopId, bool isAdmin)
        {
            var batches = await _unitOfWork.Batches.GetAllAsync();
            
            if (!isAdmin && workshopId.HasValue)
            {
                batches = batches.Where(b => b.AssignedWorkshopId == workshopId.Value);
            }

            var result = isAdmin 
                ? _mapper.Map<IEnumerable<AdminBatchDto>>(batches) 
                : _mapper.Map<IEnumerable<BatchDto>>(batches);

            // Manual mapping product name for now since we didn't eager load
            // In a real scenario we should eager load Product using Include in generic repo
            foreach(var dto in result)
            {
                var batchEntity = batches.First(b => b.Id == dto.Id);
                var product = await _unitOfWork.Products.GetByIdAsync(batchEntity.ProductId);
                if (product != null)
                {
                    dto.ProductName = product.ProductName;
                }
                if (batchEntity.AssignedWorkshopId.HasValue)
                {
                    var workshop = await _unitOfWork.Workshops.GetByIdAsync(batchEntity.AssignedWorkshopId.Value);
                    if (workshop != null)
                    {
                        dto.AssignedWorkshopName = workshop.Name;
                    }
                }
                else
                {
                    dto.AssignedWorkshopName = "Xưởng nhà (Internal)";
                }
                
                dto.ReceiveDate = batchEntity.ReceiveDate;
                dto.ReceiverName = batchEntity.ReceiverName;
            }

            return Result<IEnumerable<BatchDto>>.Ok(result.Cast<BatchDto>());
        }

        public async Task<Result<BatchDto>> CreateBatchAsync(BatchCreateDto dto)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(dto.ProductId);
            if (product == null) return Result<BatchDto>.Failure("Sản phẩm không tồn tại", 404);

            var batch = new Batch
            {
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UnitPrice = product.DefaultUnitPrice,
                AssignedWorkshopId = dto.AssignedWorkshopId,
                Status = 0, // New
                ReceiveDate = DateTime.Now,
                ReceiverName = dto.ReceiverName
            };

            await _unitOfWork.Batches.AddAsync(batch);
            await _unitOfWork.SaveChangesAsync();

            var resultDto = _mapper.Map<BatchDto>(batch);
            resultDto.ProductName = product.ProductName;
            return Result<BatchDto>.Created(resultDto);
        }

        public async Task<Result<BatchDto>> CreateSubBatchAsync(SubBatchCreateDto dto)
        {
            var parentBatch = await _unitOfWork.Batches.GetByIdAsync(dto.ParentBatchId);
            if (parentBatch == null) return Result<BatchDto>.Failure("Parent batch not found", 404);

            // BR01: Quantity logic. parentBatch.Quantity is the REMAINING quantity
            if (dto.Quantity > parentBatch.Quantity)
            {
                return Result<BatchDto>.Failure("Sub-batch quantity exceeds remaining parent batch capacity", 400);
            }

            // BR02: Margin logic
            if (parentBatch.UnitPrice > 0 && dto.UnitPrice > parentBatch.UnitPrice)
            {
                return Result<BatchDto>.Failure("Sub-batch unit price cannot exceed parent unit price", 400);
            }

            var subBatch = new Batch
            {
                ProductId = parentBatch.ProductId,
                ParentBatchId = parentBatch.Id,
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice,
                AssignedWorkshopId = dto.AssignedWorkshopId,
                Status = 0, // New
                ReceiveDate = DateTime.Now,
                ReceiverName = "Phân bổ thầu phụ"
            };

            // Deduct from parent batch
            parentBatch.Quantity -= dto.Quantity;
            _unitOfWork.Batches.Update(parentBatch);

            await _unitOfWork.Batches.AddAsync(subBatch);
            await _unitOfWork.SaveChangesAsync();

            var resultDto = _mapper.Map<BatchDto>(subBatch);
            return Result<BatchDto>.Created(resultDto);
        }

        public async Task<Result<ExportVoucherDto>> ExportVoucherAsync(int batchId)
        {
            var batch = await _unitOfWork.Batches.GetByIdAsync(batchId);
            if (batch == null) return Result<ExportVoucherDto>.Failure("Batch not found", 404);

            // BR06
            if (batch.Status != 2) // Completed
            {
                return Result<ExportVoucherDto>.Failure("Batch is not completed. Cannot export voucher.", 400);
            }

            var product = await _unitOfWork.Products.GetByIdAsync(batch.ProductId);
            var workshop = batch.AssignedWorkshopId.HasValue 
                ? await _unitOfWork.Workshops.GetByIdAsync(batch.AssignedWorkshopId.Value) 
                : null;

            var voucher = new ExportVoucherDto
            {
                BatchId = batch.Id,
                ProductName = product?.ProductName ?? "Unknown",
                Quantity = batch.Quantity,
                AssignedWorkshopName = workshop?.Name ?? "Internal",
                TotalValue = batch.Quantity * batch.UnitPrice
            };

            return Result<ExportVoucherDto>.Ok(voucher);
        }

        public async Task<Result<IEnumerable<BatchEmployeeDto>>> GetEmployeesByBatchAsync(int batchId)
        {
            var batch = await _unitOfWork.Batches.GetByIdAsync(batchId);
            if (batch == null) return Result<IEnumerable<BatchEmployeeDto>>.Failure("Batch not found", 404);

            // Get all bundles for this batch
            var bundles = await _unitOfWork.Bundles.FindAsync(b => b.BatchId == batchId);
            var bundleIds = bundles.Select(b => b.Id).ToList();

            if (!bundleIds.Any())
            {
                return Result<IEnumerable<BatchEmployeeDto>>.Ok(new List<BatchEmployeeDto>());
            }

            var productionLogs = await _unitOfWork.ProductionLogs.FindAsync(pl => pl.BundleId.HasValue && bundleIds.Contains(pl.BundleId.Value));
            
            // Need employees data
            var employees = await _unitOfWork.Employees.GetAllAsync();

            var result = productionLogs
                .GroupBy(pl => pl.EmployeeId)
                .Select(g => new BatchEmployeeDto
                {
                    EmployeeId = g.Key,
                    EmployeeName = employees.FirstOrDefault(e => e.Id == g.Key)?.FullName ?? "Unknown",
                    TotalBundles = g.Count(),
                    TotalEarned = g.Sum(pl => pl.EarnedAmount)
                })
                .ToList();

            return Result<IEnumerable<BatchEmployeeDto>>.Ok(result);
        }
    }
}
