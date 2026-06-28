namespace chamcong.Application.DTOs
{
    public class BatchDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int Status { get; set; }
        public string AssignedWorkshopName { get; set; }
        public System.DateTime ReceiveDate { get; set; }
        public string ReceiverName { get; set; }
    }

    public class AdminBatchDto : BatchDto
    {
        public int? ParentBatchId { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class SubBatchCreateDto
    {
        public int ParentBatchId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int AssignedWorkshopId { get; set; }
    }

    public class BatchCreateDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int? AssignedWorkshopId { get; set; }
        public string ReceiverName { get; set; }
    }

    public class BatchEmployeeDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int TotalBundles { get; set; }
        public decimal TotalEarned { get; set; }
    }
}
