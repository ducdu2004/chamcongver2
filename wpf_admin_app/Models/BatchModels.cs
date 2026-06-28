using System;

namespace chamcong.WpfAdmin.Models
{
    public class BatchModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int Status { get; set; }
        public string StatusText => Status == 0 ? "Mới (New)" : Status == 1 ? "Đang làm" : "Hoàn thành";
        public int? ParentBatchId { get; set; }
        public decimal UnitPrice { get; set; }
        public string AssignedWorkshopName { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string ReceiverName { get; set; }
    }

    public class BatchEmployeeModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int TotalBundles { get; set; }
        public decimal TotalEarned { get; set; }
    }
}
