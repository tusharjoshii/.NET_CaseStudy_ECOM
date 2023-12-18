using EcommerceApp.Entity;
using EcommerceApp.exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.dao
{
    internal interface IOrderProcessorRepository
    {
        bool CreateProduct(Product product);
        bool CreateCustomer(Customer customer);
        bool DeleteProduct(int productId);
        bool DeleteCustomer(int customerId);
        bool AddToCart(int cart_id, Customer customer, Product product, int quantity);
        bool RemoveFromCart(Customer customer, Product product);
        List<Product> GetAllFromCart(Customer customer);
        public List<KeyValuePair<Product, int>> GetProducts(Customer customer, int quantityToAdd);
        bool PlaceOrder(int order_id, Customer customer, List<KeyValuePair<Product, int>> products, string shippingAddress);
        List<KeyValuePair<Product, int>> GetOrdersByCustomer(int customerId);
    }
}
