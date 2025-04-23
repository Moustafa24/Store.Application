using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore.Storage.Json;
using StackExchange.Redis;

namespace peresistence.Repositeries
{
    public class BasketRepository(IConnectionMultiplexer connection) : IBasketRepository
    {

        private readonly IDatabase _database = connection.GetDatabase() ;

        public async Task<CustomerBasket?> GetBasketAsync(string id)
        {
           var redisvalue= await _database.StringGetAsync(id);
            if(redisvalue.IsNullOrEmpty) return null;
             var basket= JsonSerializer.Deserialize<CustomerBasket>(redisvalue);
            if (basket is null) return null;

            return basket;
        }


        public async Task<CustomerBasket?> UpdateBasketAsyncc(CustomerBasket basket, TimeSpan? timeToLive = null)
        {
            var redusvalue = JsonSerializer.Serialize(basket);
            var flag= _database.StringSet(basket.Id , redusvalue, TimeSpan.FromDays(30));

            return flag ? await GetBasketAsync(basket.Id) : null;
        }

        public async Task<bool> DeleteBasketAsyncc(string id)
        {
          return await _database.KeyDeleteAsync(id);
        }

      
    }
}
