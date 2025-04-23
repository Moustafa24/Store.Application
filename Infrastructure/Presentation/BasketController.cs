using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpGet] // GET: /api/baskets?id=sadas  
        public async Task<IActionResult> GetBasketById(string id)
        {
            var result = await serviceManager.BasketSrvice.GetBasketAsync(id);
            return Ok(result);
        }

        [HttpPost] // POST: /api/baskets  
        public async Task<IActionResult> UpdateBasket(BasketDto basketDto)
        {
            var result = await serviceManager.BasketSrvice.UpdateBasketAsync(basketDto);
            return Ok(result);
        }

        [HttpDelete] // DELETE: /api/baskets?id=id  
        public async Task<IActionResult> DeleteBasket(string id)
        {
            await serviceManager.BasketSrvice.DeleteBasketAsync(id);
            return NoContent(); // 204  
        }

    }
}
