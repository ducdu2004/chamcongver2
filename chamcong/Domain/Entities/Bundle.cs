using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chamcong.Domain.Entities
{
    public class Bundle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int BatchId { get; set; }

        public int BundleNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string StepName { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal StepPrice { get; set; }

        public int Quantity { get; set; }

        public int Status { get; set; }

        // Navigation properties
        [ForeignKey("BatchId")]
        public Batch Batch { get; set; }

        public ProductionLog? ProductionLog { get; set; }
    }
}
