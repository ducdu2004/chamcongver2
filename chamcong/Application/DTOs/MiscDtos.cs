namespace chamcong.Application.DTOs
{
    public class BundleDto
    {
        public int Id { get; set; }
        public int BundleNumber { get; set; }
        public string StepName { get; set; }
        public decimal StepPrice { get; set; }
        public int Quantity { get; set; }
        public int Status { get; set; }
    }

    public class ExportVoucherDto
    {
        public int BatchId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string AssignedWorkshopName { get; set; }
        public decimal TotalValue { get; set; }
    }

    public class FingerprintScanDto
    {
        public string FingerprintId { get; set; }
    }
}
