using System;

namespace chamcong.WpfAdmin.Models
{
    public class EmployeeSummaryModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? LatestStartDate { get; set; }
        public int SeniorityYears { get; set; }
    }

    public class DailyWorkerModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public decimal HourlyWage { get; set; }
        public decimal OvertimeHourlyWage { get; set; }
        public decimal TotalOvertimeHours { get; set; }
    }

    public class AttendanceSheetDayModel
    {
        public int Day { get; set; }
        public DateTime Date { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public decimal OvertimeHours { get; set; }
        
        // For UI Binding
        public string DayText => Day.ToString();
        public string CheckInText => CheckInTime?.ToString("HH:mm") ?? "-";
        public string CheckOutText => CheckOutTime?.ToString("HH:mm") ?? "-";
        public string OvertimeText => OvertimeHours > 0 ? $"{OvertimeHours}h" : "-";
        public bool IsWeekend => Date.DayOfWeek == DayOfWeek.Saturday || Date.DayOfWeek == DayOfWeek.Sunday;
    }
}
