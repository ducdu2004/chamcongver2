using System;

namespace chamcong.WpfAdmin.Models
{
    public class ManualProductionCreateDto
    {
        public int EmployeeId { get; set; }
        public int ProductId { get; set; }
        public int GarmentPartId { get; set; }
        public string SizeOrTable { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class ManualProductionDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string ProductName { get; set; }
        public string GarmentPartName { get; set; }
        public string SizeOrTable { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal EarnedAmount { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}
