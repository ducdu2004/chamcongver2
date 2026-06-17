using chamcong.Application.Common;
using chamcong.Application.Interfaces;
using chamcong.Domain.Entities;

namespace chamcong.Application.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AttendanceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> CheckOutAsync(int employeeId)
        {
            var today = DateTime.Today;
            var attendanceLogs = await _unitOfWork.AttendanceLogs.FindAsync(a => a.EmployeeId == employeeId && a.WorkDate.Date == today);
            var log = attendanceLogs.FirstOrDefault();

            if (log == null)
            {
                return Result.Failure("No check-in found for today", 400);
            }

            if (log.CheckOutTime != null)
            {
                return Result.Failure("Already checked out today", 400);
            }

            var now = DateTime.Now;
            log.CheckOutTime = now;

            // BR05: Overtime calc
            var overtimeStart = today.AddHours(17).AddMinutes(30);
            if (now > overtimeStart)
            {
                log.OvertimeHours = (decimal)(now - overtimeStart).TotalHours;
            }

            _unitOfWork.AttendanceLogs.Update(log);
            await _unitOfWork.SaveChangesAsync();

            return Result.Ok("Checked out successfully");
        }
    }
}
