using chamcong.Application.Common;
using chamcong.Application.Interfaces;
using chamcong.Domain.Entities;

namespace chamcong.Application.Services
{
    public class FingerprintService : IFingerprintService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FingerprintService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> ProcessFingerprintScanAsync(string fingerprintId)
        {
            var now = DateTime.Now;

            // 1. Log raw scan
            var fpLog = new FingerprintLog
            {
                FingerprintId = fingerprintId,
                ScanTime = now,
                IsProcessed = true
            };
            await _unitOfWork.FingerprintLogs.AddAsync(fpLog);

            // 2. Find employee
            var employees = await _unitOfWork.Employees.FindAsync(e => e.FingerprintId == fingerprintId);
            var employee = employees.FirstOrDefault();
            
            if (employee == null)
            {
                fpLog.IsProcessed = false;
                await _unitOfWork.SaveChangesAsync();
                return Result.Failure("Employee with fingerprint not found", 404);
            }

            // 3. Process Check-in / Check-out (BR07)
            var today = now.Date;
            var attendances = await _unitOfWork.AttendanceLogs.FindAsync(a => a.EmployeeId == employee.Id && a.WorkDate.Date == today);
            var log = attendances.FirstOrDefault();

            if (log == null)
            {
                // Check in
                log = new AttendanceLog
                {
                    EmployeeId = employee.Id,
                    WorkDate = today,
                    CheckInTime = now
                };
                await _unitOfWork.AttendanceLogs.AddAsync(log);
            }
            else
            {
                // Check out
                if (log.CheckOutTime == null)
                {
                    log.CheckOutTime = now;
                    var overtimeStart = today.AddHours(17).AddMinutes(30);
                    if (now > overtimeStart)
                    {
                        log.OvertimeHours = (decimal)(now - overtimeStart).TotalHours;
                    }
                    _unitOfWork.AttendanceLogs.Update(log);
                }
            }

            await _unitOfWork.SaveChangesAsync();
            return Result.Ok("Fingerprint processed successfully");
        }
    }
}
