using EcommerceApp.dao;
using EcommerceApp.Entity;
using EcommerceApp.exceptions;

namespace Test
{
    public class Tests
    {
        private const string connectionString = "Server=TUSH-R;Database=ECOM;Trusted_Connection=True";

        [Test]
        public void TestCreateProductSuccess()
        {
            OrderProcessorRepositoryImpl repo = new OrderProcessorRepositoryImpl();
            Product product = new Product();
            product.ProductId = 22;
            product.Name = "TestProduct1";
            product.Price = 11.99m;
            product.Description = "This is a test product1.";
            product.StockQuantity = 1;

            var creationStatus = repo.CreateProduct(product);
            OrderProcessorRepositoryImpl orderProcessorRepositoryImpl = new OrderProcessorRepositoryImpl();

            Assert.AreEqual(true, creationStatus);
        }


        [Test]
        public void TestAddToCartSuccess()
        {
            OrderProcessorRepositoryImpl repo = new OrderProcessorRepositoryImpl();
            Product product = new Product();
            Customer customer = new Customer();
            int cartId = 21;
            customer.CustomerId = 16;
            product.ProductId = 12;
            int quantity = 2;

            Cart cart = new Cart();

            OrderProcessorRepositoryImpl orderProcessorRepositoryImpl = new OrderProcessorRepositoryImpl();
            bool cartStatus = orderProcessorRepositoryImpl.AddToCart(cartId, customer, product, quantity);

            Assert.AreEqual(true, cartStatus);
        }


        [Test]
        public void TestPlaceOrderSuccess()
        {
            OrderProcessorRepositoryImpl repo = new OrderProcessorRepositoryImpl();
            Product product = new Product();
            Customer customers = new Customer();
            List<KeyValuePair<Product, int>> prodPair = new List<KeyValuePair<Product, int>>();

            int orderId = 21;
            customers.CustomerId = 16;
            product.ProductId = 12;
            int quantity = 1;
            string shippingAddress = "ABC Test";
            prodPair.Add(new KeyValuePair<Product, int>(product, quantity));

            var orderStatus = repo.PlaceOrder(orderId, customers, prodPair, shippingAddress);

            Assert.AreEqual(true, orderStatus);
        }


        [Test]
        public void TestCustomerNotFoundException()
        {
            OrderProcessorRepositoryImpl repo = new OrderProcessorRepositoryImpl();

            Assert.Throws<CustomerNotFoundException>(() => repo.DeleteCustomer(12));
        }

        [Test]
        public void TestProductNotFoundException()
        {
            OrderProcessorRepositoryImpl repo = new OrderProcessorRepositoryImpl();

            Assert.Throws<ProductNotFoundException>(() => repo.DeleteProduct(21));
        }
    }
}