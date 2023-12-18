using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.Entity
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        // Default constructor
        public Customer() { }

        // Parameterized constructor
        public Customer(int customerId, string name, string email, string password)
        {
            CustomerId = customerId;
            Name = name;
            Email = email;
            Password = password;
        }
    }
}
