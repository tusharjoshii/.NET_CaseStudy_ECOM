using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.entity
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity{ get; set; }

        // Default constructor
        public OrderItem() { }

        // Parameterized constructor
        public OrderItem(int orderItemId, int orderId, int productId, int quantity)
        {
            OrderItemId = orderItemId;
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
        }
    }
}
