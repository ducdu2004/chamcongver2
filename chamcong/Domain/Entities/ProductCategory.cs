using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chamcong.Domain.Entities
{
    public class ProductCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } // e.g. Áo, Quần, Váy

        public ICollection<GarmentPart> GarmentParts { get; set; } = new List<GarmentPart>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
