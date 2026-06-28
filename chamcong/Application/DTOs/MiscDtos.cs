namespace chamcong.Application.DTOs
{
    public class DistributorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }

    public class DistributorCreateDto
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }

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
