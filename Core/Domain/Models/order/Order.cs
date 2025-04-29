using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.order
{
    public class Order : BaseEntity<Guid>
    {
        public Order()
        {
            
        }
        public Order(string userEmail, Address shippingAddress, ICollection<OrderItem> orderItems, DeliveryMethod deliveryMethod, decimal subTotal, string paymentIntentId)
        {
            Id = Guid.NewGuid();
            UserEmail = userEmail;
            ShippingAddress = shippingAddress;
            OrderItems = orderItems;
            DeliveryMethod = deliveryMethod;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }

        // Id 

        //User Email
        public string UserEmail {  get; set; }

        //Shipping Address

        public Address ShippingAddress { get; set; }

        //OrdrItem
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); // Navigational Property


        public DeliveryMethod DeliveryMethod { get; set; } // Navigational Property

        public int?  DeleveryMethodId { get; set; } //FK

        // Payment Status 

        public OrderPaymentStatus paymentStatus { get; set; } = OrderPaymentStatus.Pending;

        // Sub Total 
        public decimal SubTotal  { get; set; }

        //OrderDate

        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

        //Payment
        public string PaymentIntentId { get; set; }



    }
}
