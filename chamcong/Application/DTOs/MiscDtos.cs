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

    public class CreateIssueReportDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class IssueReportDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
