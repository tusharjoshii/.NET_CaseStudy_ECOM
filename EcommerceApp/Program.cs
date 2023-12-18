using System;
using EcommerceApp.dao;
using EcommerceApp.Entity;
using EcommerceApp.exceptions;

namespace EcommerceApp
{
    class EcomApp
    {
        static void Main(string[] args)
        {
            try
            {
                IOrderProcessorRepository repo = new OrderProcessorRepositoryImpl();
                int choice = 0;
                Customer customer = new Customer();
                Product product = new Product();
                int quantityToAdd = 0;
                do
                {
                    Console.WriteLine("1. Create a new customer");
                    Console.WriteLine("2. Create a new product");
                    Console.WriteLine("3. Add product to cart");
                    Console.WriteLine("4. Remove product from cart");
                    Console.WriteLine("5. Get all products from cart");
                    Console.WriteLine("6. Place an order");
                    Console.WriteLine("7. Get all orders by customer");
                    Console.WriteLine("8. Exit");
                    Console.Write("Enter your choice: ");
                    choice = int.Parse(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            Console.Write("Enter customer ID: ");
                            int customerId = int.Parse(Console.ReadLine());

                            Console.Write("Enter customer name: ");
                            string customerName = Console.ReadLine();

                            Console.Write("Enter customer email: ");
                            string customerEmail = Console.ReadLine();

                            Console.Write("Enter customer password: ");
                            string customerPassword = Console.ReadLine();

                            customer = new Customer { CustomerId = customerId, Name = customerName, Email = customerEmail, Password = customerPassword };
                            bool isCustomerCreated = repo.CreateCustomer(customer);
                            Console.WriteLine(isCustomerCreated ? "Customer created successfully." : "Customer creation failed.");
                            break;
                        case 2:
                            Console.Write("Enter product ID: ");
                            int productId = int.Parse(Console.ReadLine());

                            Console.Write("Enter product name: ");
                            string productName = Console.ReadLine();

                            Console.Write("Enter product price: ");
                            decimal productPrice = decimal.Parse(Console.ReadLine());

                            Console.Write("Enter product description: ");
                            string productDescription = Console.ReadLine();

                            Console.Write("Enter product stock quantity: ");
                            int productStockQuantity = int.Parse(Console.ReadLine());

                            product = new Product { ProductId = productId, Name = productName, Price = productPrice, Description = productDescription, StockQuantity = productStockQuantity };
                            bool isProductCreated = repo.CreateProduct(product);
                            Console.WriteLine(isProductCreated ? "Product created successfully." : "Product creation failed.");
                            break;
                        case 3:
                            Console.Write("Enter cart ID: ");
                            int cartId = int.Parse(Console.ReadLine());
                            Console.Write("Enter customer ID: ");
                            customerId = int.Parse(Console.ReadLine());
                            customer.CustomerId = customerId;
                            Console.Write("Enter product ID: ");
                            productId = int.Parse(Console.ReadLine());
                            product.ProductId = productId;
                            Console.Write("Enter quantity to add to cart: ");
                            quantityToAdd = int.Parse(Console.ReadLine());
                            
                            Cart cart = new Cart { CartId = cartId, CustomerId = customerId, ProductId = productId, Quantity = quantityToAdd };
                            bool isProductAddedToCart = repo.AddToCart(cartId, customer, product, quantityToAdd);
                            Console.WriteLine(isProductAddedToCart ? "Product added to cart successfully." : "Failed to add product to cart.");
                            break;
                        case 4:
                            Console.Write("Do you want to remove the product from the cart? (yes/no): ");
                            string removeProduct = Console.ReadLine();

                            if (removeProduct.ToLower() == "yes")
                            {
                                Console.Write("Enter customer ID: ");
                                customerId = int.Parse(Console.ReadLine());
                                customer.CustomerId = customerId;
                                Console.Write("Enter product ID: ");
                                productId = int.Parse(Console.ReadLine());
                                product.ProductId = productId;

                                bool isProductRemovedFromCart = repo.RemoveFromCart(customer, product);
                                Console.WriteLine(isProductRemovedFromCart ? "Product removed from cart successfully." : "Failed to remove product from cart.");
                            }
                            break;
                        case 5:
                            Console.Write("Enter customer ID: ");
                            customerId = int.Parse(Console.ReadLine());
                            customer.CustomerId = customerId;

                            List<Product> productsInCart = repo.GetAllFromCart(customer);
                            Console.WriteLine($"There are {productsInCart.Count} products in the cart.");
                            break;
                        case 6:
                            Console.Write("Enter order ID: ");
                            int orderId = int.Parse(Console.ReadLine());
                            Console.Write("Enter customer ID: ");
                            customerId = int.Parse(Console.ReadLine());
                            customer.CustomerId = customerId;
                            Console.Write("Enter quantity to order: ");
                            quantityToAdd = int.Parse(Console.ReadLine());
                            Console.Write("Enter shipping address: ");
                            string shippingAddress = Console.ReadLine();

                            List<KeyValuePair<Product, int>> productsToOrder = repo.GetProducts(customer, quantityToAdd);
                            bool isOrderPlaced = repo.PlaceOrder(orderId, customer, productsToOrder, shippingAddress);
                            Console.WriteLine(isOrderPlaced ? "Order placed successfully." : "Failed to place order.");
                            break;
                        case 7:
                            Console.Write("Enter customer ID: ");
                            customerId = int.Parse(Console.ReadLine());
                            customer.CustomerId = customerId;

                            List<KeyValuePair<Product, int>> customerOrders = repo.GetOrdersByCustomer(customer.CustomerId);
                            Console.WriteLine($"The customer has placed {customerOrders.Count} orders.");
                            break;
                        case 8:
                            Console.WriteLine("Exiting...");
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please enter a number between 1 and 8.");
                            break;
                    }
                } while (choice != 8);
            }
            catch (CustomerNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (ProductNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (OrderNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}


