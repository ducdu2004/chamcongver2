using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chamcong.Domain.Entities
{
    public class GarmentPart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } // e.g. Thân trước, Thân sau, Cổ áo

        [Column(TypeName = "decimal(18,2)")]
        public decimal DefaultUnitPrice { get; set; } = 0;

        public int ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }

        public ICollection<ProductComponent> ProductComponents { get; set; } = new List<ProductComponent>();
    }
}
