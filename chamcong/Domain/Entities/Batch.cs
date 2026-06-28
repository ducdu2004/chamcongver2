using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chamcong.Domain.Entities
{
    public class Batch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int? ParentBatchId { get; set; }

        public int? AssignedWorkshopId { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        // 0: New, 1: InProgress, 2: Completed
        public int Status { get; set; }

        public DateTime ReceiveDate { get; set; } = DateTime.Now;

        [MaxLength(200)]
        public string? ReceiverName { get; set; }

        // Navigation properties
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [ForeignKey("ParentBatchId")]
        public Batch? ParentBatch { get; set; }

        public ICollection<Batch> SubBatches { get; set; } = new List<Batch>();

        [ForeignKey("AssignedWorkshopId")]
        public Workshop? AssignedWorkshop { get; set; }

        public ICollection<Bundle> Bundles { get; set; } = new List<Bundle>();
    }
}
