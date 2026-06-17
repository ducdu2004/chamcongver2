namespace chamcong.Application.DTOs
{
    public class BatchDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
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
}
