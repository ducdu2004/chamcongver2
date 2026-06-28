using System;
using System.Collections.Generic;

namespace chamcong.Application.DTOs
{
    public class EmployeeSummaryDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? LatestStartDate { get; set; }
        public int SeniorityYears { get; set; }
    }

    public class DailyWorkerDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public decimal HourlyWage { get; set; }
        public decimal OvertimeHourlyWage { get; set; }
        public decimal TotalOvertimeHours { get; set; }
    }

    public class AttendanceSheetDayDto
    {
        public int Day { get; set; }
        public DateTime Date { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public decimal OvertimeHours { get; set; }
    }
}
