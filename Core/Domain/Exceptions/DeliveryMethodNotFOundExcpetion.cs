using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class DeliveryMethodNotFOundExcpetion(int id ) : NotFoundException($" DeliveryMethod with Id {id} is Not Found !")
    {

    }

}
