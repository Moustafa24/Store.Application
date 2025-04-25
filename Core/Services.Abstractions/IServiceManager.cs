using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IServiceManager
    {
         IProductService ProductService { get;  }
         IBasketSrvice BasketSrvice { get;  }
        
        ICacheService CacheService { get; }
        IAuthService   AuthService { get; }



    }
}
