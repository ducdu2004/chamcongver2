using System.Collections.Generic;

namespace chamcong.Application.DTOs
{
    public class ProductCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class GarmentPartDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductCategoryId { get; set; }
        public decimal DefaultUnitPrice { get; set; }
    }

    public class ProductDto
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

        public List<ProductComponentDto> Components { get; set; } = new List<ProductComponentDto>();
    }

    public class ProductComponentDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int GarmentPartId { get; set; }
        public string GarmentPartName { get; set; }
        public int QuantityPerProduct { get; set; }
    }

    // Input DTOs
    public class ProductCreateDto
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal DefaultUnitPrice { get; set; }
        public string Size { get; set; }
        public int? ProductCategoryId { get; set; }
        
        public int? DistributorId { get; set; }
        public int Quantity { get; set; }

        public List<ProductComponentCreateDto> Components { get; set; } = new List<ProductComponentCreateDto>();
    }

    public class ProductComponentCreateDto
    {
        public int GarmentPartId { get; set; }
        public int QuantityPerProduct { get; set; }
    }
}
