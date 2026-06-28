namespace chamcong.WpfAdmin.Models
{
    public class ReferenceModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    
    public class ProductReferenceModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
    }

    public class GarmentPartModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductCategoryId { get; set; }
        public decimal DefaultUnitPrice { get; set; }
    }
}
