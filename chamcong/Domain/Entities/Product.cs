using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chamcong.Domain.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductCode { get; set; }

        [Required]
        [StringLength(200)]
        public string ProductName { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DefaultUnitPrice { get; set; }

        [StringLength(50)]
        public string Size { get; set; } // e.g. S, M, L, XL

        public int? ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }

        public int? DistributorId { get; set; }
        public Distributor Distributor { get; set; }

        public int Quantity { get; set; }

        // Navigation properties
        public ICollection<Batch> Batches { get; set; } = new List<Batch>();
        public ICollection<ProductComponent> ProductComponents { get; set; } = new List<ProductComponent>();
    }
}
