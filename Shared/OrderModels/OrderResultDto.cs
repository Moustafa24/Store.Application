using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.OrderModels
{
    public class OrderResultDto
    {

        public Guid Id { get; set; }


        public string UserEmail { get; set; }

        //Shipping Address

        public AddressDto ShippingAddress { get; set; }

        //OrdrItem
        public ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>(); // Navigational Property


        public string DeliveryMethod { get; set; } 

  

        // Payment Status 

        public string paymentStatus { get; set; } 

        // Sub Total 
        public decimal SubTotal { get; set; }

        //OrderDate

        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

        //Payment
        public string PaymentIntentId { get; set; } =string.Empty;

        public decimal Total { get; set; }

    }
}
