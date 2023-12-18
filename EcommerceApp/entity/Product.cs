namespace EcommerceApp.Entity
{
    public class Product
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int StockQuantity { get; set; }

        // Default constructor
        public Product() { }

        // Parameterized constructor
        public Product(int productId, string name, decimal price, string description, int stockQuantity)
        {
            ProductId = productId;
            Name = name;
            Price = price;
            Description = description;
            StockQuantity = stockQuantity;
        }
    }
}