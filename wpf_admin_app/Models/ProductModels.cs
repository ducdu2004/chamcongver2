using System.Collections.Generic;
using System.Linq;

namespace chamcong.WpfAdmin.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal DefaultUnitPrice { get; set; }
        public string Size { get; set; }
        public int? ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
        
        public int? DistributorId { get; set; }
        public string DistributorName { get; set; }
        public int Quantity { get; set; }
        
        public List<ProductComponentModel> Components { get; set; } = new List<ProductComponentModel>();

        public string ComponentsSummary 
        {
            get 
            {
                if (Components == null || Components.Count == 0) return "Chưa có cấu hình";
                return string.Join(", ", Components.Select(c => $"{c.GarmentPartName} (x{c.QuantityPerProduct})"));
            }
        }
    }

    public class ProductComponentModel
    {
        public int Id { get; set; }
        public int GarmentPartId { get; set; }
        public string GarmentPartName { get; set; }
        public int QuantityPerProduct { get; set; }
    }
}
