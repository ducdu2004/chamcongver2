using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chamcong.Domain.Entities
{
    public class ProductComponent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int GarmentPartId { get; set; }
        public GarmentPart GarmentPart { get; set; }

        public int QuantityPerProduct { get; set; } // e.g. 1 Áo có 2 Tay áo
    }
}
