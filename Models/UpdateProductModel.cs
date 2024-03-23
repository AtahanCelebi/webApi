namespace ProductsAPI.Models 
{
    public class UpdateProductModel {
        public string ProductName { get; set; } = null!;

        public decimal Price { get; set; }
    }
}