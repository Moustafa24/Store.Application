using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.order;

namespace Services.Specifications
{
    public class OrderSpecifications : BaseSpecification<Order ,Guid>
    {
        public OrderSpecifications(Guid id) :base(o=>o.Id == id )
        {
            AddInclude(O => O.DeliveryMethod);
            AddInclude(O => O.OrderItems);

            
        }

        public OrderSpecifications(string userEmail) : base(o => o.UserEmail == userEmail)
        {
            AddInclude(O => O.DeliveryMethod);
            AddInclude(O => O.OrderItems);
            AddInclude(O => O.OrderDate);


        }

    }
}
