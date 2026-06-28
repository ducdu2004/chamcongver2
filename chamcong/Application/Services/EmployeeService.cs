using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using chamcong.Application.Common;
using chamcong.Application.DTOs;
using chamcong.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace chamcong.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<EmployeeSummaryDto>>> GetEmployeesSummaryAsync()
        {
            // Fetch employees with their histories
            var employees = await _unitOfWork.Employees.GetAllAsync();
            var histories = await _unitOfWork.EmploymentHistories.GetAllAsync();
            
            var result = new List<EmployeeSummaryDto>();
            var today = DateTime.Today;

            foreach (var emp in employees)
            {
                var activeHistory = histories
                    .Where(h => h.EmployeeId == emp.Id && h.EndDate == null)
                    .OrderByDescending(h => h.StartDate)
                    .FirstOrDefault();

                int seniority = 0;
                DateTime? latestStartDate = null;

                if (activeHistory != null)
                {
                    latestStartDate = activeHistory.StartDate;
                    seniority = today.Year - activeHistory.StartDate.Year;
                    if (activeHistory.StartDate.Date > today.AddYears(-seniority)) seniority--;
                    if (seniority < 0) seniority = 0;
                }

                result.Add(new EmployeeSummaryDto
                {
                    Id = emp.Id,
                    FullName = emp.FullName,
                    Phone = emp.Phone ?? "",
                    Email = emp.Email ?? "",
                    LatestStartDate = latestStartDate,
                    SeniorityYears = seniority
                });
            }

            return Result<IEnumerable<EmployeeSummaryDto>>.Ok(result);
        }

        public async Task<Result<IEnumerable<DailyWorkerDto>>> GetDailyWorkersAsync()
        {
            // PayType == 1 is daily worker
            var employees = await _unitOfWork.Employees.FindAsync(e => e.PayType == 1);
            var attendances = await _unitOfWork.AttendanceLogs.GetAllAsync();

            var result = new List<DailyWorkerDto>();

            foreach (var emp in employees)
            {
                var empAttendances = attendances.Where(a => a.EmployeeId == emp.Id);
                var totalOvertime = empAttendances.Sum(a => a.OvertimeHours);

                result.Add(new DailyWorkerDto
                {
                    Id = emp.Id,
                    FullName = emp.FullName,
                    HourlyWage = emp.HourlyWage,
                    OvertimeHourlyWage = emp.OvertimeHourlyWage,
                    TotalOvertimeHours = totalOvertime
                });
            }

            return Result<IEnumerable<DailyWorkerDto>>.Ok(result);
        }

        public async Task<Result<IEnumerable<AttendanceSheetDayDto>>> GetAttendanceSheetAsync(int employeeId, int month, int year)
        {
            var emp = await _unitOfWork.Employees.GetByIdAsync(employeeId);
            if (emp == null) return Result<IEnumerable<AttendanceSheetDayDto>>.Failure("Employee not found", 404);

            var attendances = await _unitOfWork.AttendanceLogs.FindAsync(
                a => a.EmployeeId == employeeId && a.WorkDate.Month == month && a.WorkDate.Year == year);

            var daysInMonth = DateTime.DaysInMonth(year, month);
            var sheet = new List<AttendanceSheetDayDto>();

            for (int i = 1; i <= daysInMonth; i++)
            {
                var date = new DateTime(year, month, i);
                var log = attendances.FirstOrDefault(a => a.WorkDate.Date == date.Date);

                sheet.Add(new AttendanceSheetDayDto
                {
                    Day = i,
                    Date = date,
                    CheckInTime = log?.CheckInTime,
                    CheckOutTime = log?.CheckOutTime,
                    OvertimeHours = log?.OvertimeHours ?? 0
                });
            }

            return Result<IEnumerable<AttendanceSheetDayDto>>.Ok(sheet);
        }
    }
}
