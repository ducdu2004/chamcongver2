using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chamcong.Domain.Entities
{
    public class ProductionLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public int? BundleId { get; set; }

        public int? ProductId { get; set; }

        public int? GarmentPartId { get; set; }

        [StringLength(50)]
        public string? SizeOrTable { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        public DateTime CompletedAt { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal EarnedAmount { get; set; }

        // Navigation properties
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        [ForeignKey("BundleId")]
        public Bundle? Bundle { get; set; }

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        [ForeignKey("GarmentPartId")]
        public GarmentPart? GarmentPart { get; set; }
    }
}
