    using EcommerceApp.Entity;
using EcommerceApp.exceptions;
using EcommerceApp.util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.dao
{
    public class OrderProcessorRepositoryImpl : IOrderProcessorRepository
    {
        public string connectionString;
       
        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }
        SqlCommand cmd = null;

        public OrderProcessorRepositoryImpl()
        {
            connectionString = DBConnection.GetConnectionString();
            cmd = new SqlCommand();
        }


        public bool CreateProduct(Product product)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                cmd.Parameters.Clear();
                cmd.CommandText = "INSERT INTO products (product_id, name, price, description, stockQuantity) VALUES (@ProductId, @Name, @Price, @Description, @StockQuantity)";
                cmd.Parameters.AddWithValue("@ProductId", product.ProductId);
                cmd.Parameters.AddWithValue("@Name", product.Name);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Description", product.Description);
                cmd.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();
                int addProductStatus = cmd.ExecuteNonQuery();
                return addProductStatus > 0;
            }
        }

        public bool CreateCustomer(Customer customer)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                cmd.Parameters.Clear();
                cmd.CommandText = "INSERT INTO customers (customer_id, name, email, password) VALUES (@CustomerId, @Name, @Email, @Password)";
                cmd.Parameters.AddWithValue("@CustomerId", customer.CustomerId);
                cmd.Parameters.AddWithValue("@Name", customer.Name);
                cmd.Parameters.AddWithValue("@Email", customer.Email);
                cmd.Parameters.AddWithValue("@Password", customer.Password);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();
                int addCustomerStatus = cmd.ExecuteNonQuery();
                return addCustomerStatus > 0;
            }
        }

        public bool DeleteProduct(int productId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                cmd.Parameters.Clear();
                cmd.CommandText = "DELETE FROM products WHERE product_id = @ProductId";
                cmd.Parameters.AddWithValue("@ProductId", productId);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();
                int deleteProductStatus = cmd.ExecuteNonQuery();
                if (deleteProductStatus == 0)
                {
                    throw new ProductNotFoundException($"Product with ID {productId} not found.");
                }
                return deleteProductStatus > 0;
            }
        }

        public bool DeleteCustomer(int customerId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                cmd.Parameters.Clear();
                cmd.CommandText = "DELETE FROM customers WHERE customer_id = @CustomerId";
                cmd.Parameters.AddWithValue("@CustomerId", customerId);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();
                int deleteCustomerStatus = cmd.ExecuteNonQuery();
                if (deleteCustomerStatus == 0)
                {
                    throw new CustomerNotFoundException($"Customer with ID {customerId} not found.");
                }
                return deleteCustomerStatus > 0;
            }
        }

        public bool AddToCart(int cart_id, Customer customer, Product product, int quantity)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                cmd.Parameters.Clear();
                cmd.CommandText = "INSERT INTO cart (cart_id, customer_id, product_id, quantity) VALUES (@CartId, @CustomerId, @ProductId, @Quantity)";
                cmd.Parameters.AddWithValue("@CartId", cart_id);
                cmd.Parameters.AddWithValue("@CustomerId", customer.CustomerId);
                cmd.Parameters.AddWithValue("@ProductId", product.ProductId);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();
                int addToCartStatus = cmd.ExecuteNonQuery();
                return addToCartStatus > 0;
            }
        }

        public bool RemoveFromCart(Customer customer, Product product)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                cmd.Parameters.Clear();
                cmd.CommandText = "DELETE FROM cart WHERE customer_id = @CustomerId AND product_id = @ProductId";
                cmd.Parameters.AddWithValue("@CustomerId", customer.CustomerId);
                cmd.Parameters.AddWithValue("@ProductId", product.ProductId);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();
                int removeFromCartStatus = cmd.ExecuteNonQuery();
                if (removeFromCartStatus == 0)
                {
                    throw new ProductNotFoundException($"Product with ID {product.ProductId} not found in the cart.");
                }
                return removeFromCartStatus > 0;
            }
        }

        public List<Product> GetAllFromCart(Customer customer)
        {
            List<Product> productsInCart = new List<Product>();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                cmd.Parameters.Clear();
                cmd.CommandText = "SELECT * FROM products WHERE product_id IN (SELECT product_id FROM cart WHERE customer_id = @CustomerId)";
                cmd.Parameters.AddWithValue("@CustomerId", customer.CustomerId);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Product product = new Product
                    {
                        ProductId = (int)reader["product_id"],
                        Name = (string)reader["name"],
                        Price = (decimal)reader["price"],
                        Description = (string)reader["description"],
                        StockQuantity = (int)reader["stockQuantity"]
                    };
                    productsInCart.Add(product);
                }
                if (productsInCart.Count == 0)
                {
                    throw new ProductNotFoundException($"No products found in the cart for customer with ID {customer.CustomerId}.");
                }
            }
            return productsInCart;
        }

        public List<KeyValuePair<Product, int>> GetProducts(Customer customer, int quantityToAdd)
        {
            List<KeyValuePair<Product, int>> productsAndQuantities = new List<KeyValuePair<Product, int>>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                cmd.Parameters.Clear();
                cmd.CommandText = "SELECT * FROM products where product_id IN (Select product_id from cart where customer_id = @CustomerId)";
                cmd.Parameters.AddWithValue("@CustomerId", customer.CustomerId);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Product product = new Product
                    {
                        ProductId = (int)reader["product_id"],
                        Name = (string)reader["name"],
                        Price = (decimal)reader["price"],
                        Description = (string)reader["description"],
                        StockQuantity = (int)reader["stockQuantity"]
                    };

                    int quantity = quantityToAdd;
                    productsAndQuantities.Add(new KeyValuePair<Product, int>(product, quantity));
                }
            }

            return productsAndQuantities;
        }


        public bool PlaceOrder(int order_id, Customer customer, List<KeyValuePair<Product, int>> products, string shippingAddress)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlTransaction transaction = sqlConnection.BeginTransaction();

                try
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "INSERT INTO orders (order_id, customer_id, order_date, total_price, shipping_address) VALUES (@OrderId, @CustomerId, @OrderDate, @TotalPrice, @ShippingAddress)";
                    cmd.Parameters.AddWithValue("@OrderId", order_id);
                    cmd.Parameters.AddWithValue("@CustomerId", customer.CustomerId);
                    cmd.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@ShippingAddress", shippingAddress);

                    decimal totalPrice = 0;
                    foreach (var item in products)
                    {
                        totalPrice += item.Key.Price * item.Value;
                    }
                    cmd.Parameters.AddWithValue("@TotalPrice", totalPrice);
                    cmd.Connection = sqlConnection;
                    cmd.Transaction = transaction;

                    cmd.ExecuteNonQuery();

                    foreach (var item in products)
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = "INSERT INTO order_items (order_item_id, order_id, product_id, quantity_supplied) VALUES (@OrderItemId, @OrderId, @ProductId, @Quantity)";
                        cmd.Parameters.AddWithValue("@OrderItemId", order_id);
                        cmd.Parameters.AddWithValue("@OrderId", order_id);
                        cmd.Parameters.AddWithValue("@ProductId", item.Key.ProductId);
                        cmd.Parameters.AddWithValue("@Quantity", item.Value);
                        cmd.Connection = sqlConnection;
                        cmd.ExecuteNonQuery();
                    }

                transaction.Commit();
            }
                catch (Exception)
                {
                transaction.Rollback();
                throw new OrderNotFoundException($"Failed to place order for customer with ID {customer.CustomerId}.");
            }
        }

            return true;
        }


        public List<KeyValuePair<Product, int>> GetOrdersByCustomer(int customerId)
        {
            List<KeyValuePair<Product, int>> customerOrders = new List<KeyValuePair<Product, int>>();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                cmd.Parameters.Clear();
                cmd.CommandText = "SELECT p.*, oi.quantity_supplied FROM products p JOIN order_items oi ON p.product_id = oi.product_id JOIN orders o ON oi.order_id = o.order_id WHERE o.customer_id = @CustomerId;";
                cmd.Parameters.AddWithValue("@CustomerId", customerId);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Product product = new Product
                    {
                        ProductId = (int)reader["product_id"],
                        Name = (string)reader["name"],
                        Price = (decimal)reader["price"],
                        Description = (string)reader["description"],
                        StockQuantity = (int)reader["stockQuantity"]
                    };
                    int quantity = (int)reader["quantity_supplied"];
                    customerOrders.Add(new KeyValuePair<Product, int>(product, quantity));
                }
                if (customerOrders.Count == 0)
                {
                    throw new OrderNotFoundException($"No orders found for customer with ID {customerId}.");
                }
            }
            return customerOrders;
        }
    }

        //private List<Customer> customers = new List<Customer>();
        //private List<Product> products = new List<Product>();
        //private List<Cart> cart = new List<Cart>();

        //public bool CreateProduct(Product product)
        //{
        //    if (products.Any(p => p.ProductId == product.ProductId))
        //    {
        //        return false;
        //    }

        //    products.Add(product);
        //    return true;
        //}

        //public bool CreateCustomer(Customer customer)
        //{
        //    if (customers.Any(c => c.CustomerId == customer.CustomerId))
        //    {
        //        return false;
        //    }

        //    customers.Add(customer);
        //    return true;
        //}

        //public bool DeleteProduct(int productId)
        //{
        //    var product = products.FirstOrDefault(p => p.ProductId == productId);
        //    if (product == null)
        //    {
        //        throw new ProductNotFoundException($"Product with ID {productId} not found.");
        //    }

        //    products.Remove(product);
        //    return true;
        //}

        //public bool DeleteCustomer(int customerId)
        //{
        //    var customer = customers.FirstOrDefault(c => c.CustomerId == customerId);
        //    if (customer == null)
        //    {
        //        throw new CustomerNotFoundException($"Customer with ID {customerId} not found.");
        //    }

        //    customers.Remove(customer);
        //    return true;
        //}

        //public bool AddToCart(Customer customer, Product product, int quantity)
        //{
        //    var cartItem = cart.FirstOrDefault(c => c.CustomerId == customer.CustomerId && c.ProductId == product.ProductId);
        //    if (cartItem != null)
        //    {
        //        return false;
        //    }

        //    cart.Add(new Cart { CustomerId = customer.CustomerId, ProductId = product.ProductId, Quantity = quantity });
        //    return true;
        //}

        //public bool RemoveFromCart(Customer customer, Product product)
        //{
        //    var cartItem = cart.FirstOrDefault(c => c.CustomerId == customer.CustomerId && c.ProductId == product.ProductId);
        //    if (cartItem == null)
        //    {
        //        throw new ProductNotFoundException($"Product with ID {product.ProductId} not found in the cart.");
        //    }

        //    cart.Remove(cartItem);
        //    return true;
        //}

        //public List<Product> GetAllFromCart(Customer customer)
        //{
        //    var productIds = cart.Where(c => c.CustomerId == customer.CustomerId).Select(c => c.ProductId);
        //    var productsInCart = products.Where(p => productIds.Contains(p.ProductId)).ToList();
        //    return productsInCart;
        //}

        //public bool PlaceOrder(Customer customer, List<KeyValuePair<Product, int>> products, string shippingAddress)
        //{
        //    //logic here
        //    return true;
        //}

        //public List<KeyValuePair<Product, int>> GetOrdersByCustomer(int customerId)
        //{
        //    //logic 
        //    return new List<KeyValuePair<Product, int>>();
        //}
  
 }
