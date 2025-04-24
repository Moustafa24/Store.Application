using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Services.Abstractions;

namespace Services
{
    public class ChacheService(ICacheRepository cacheRepository) : ICacheService
    {
        public async Task<string?> GetCacheValueAsync(string Key)
        {
            var value = await cacheRepository.GetAsync(Key);
            return value == null ? null : value; 

        }

        public async Task SetCacheValueAsync(string key, string value, TimeSpan duration)
        {
            await cacheRepository.SetAsync(key, value,duration);
        }
    }
}
