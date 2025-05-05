using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.OrderModels;

namespace peresistence
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController(IServiceManager serviceManager) :ControllerBase
    {

        [HttpPost] // POST: /api/Orders
        public async Task<IActionResult> CreateOrder(OrderRequestDto request)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await serviceManager.OrderService.CreateOrderAsync(request,email);
            return Ok(result);
        }

        [HttpGet] // GET: /api/Orders
        public async Task<IActionResult> GetOrders()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await serviceManager.OrderService.GetOrderByUserEmailAsync(email);
            return Ok(result);
        }


        [HttpGet( "{id}")] // GET: /api/Orders/dasdas
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var result = await serviceManager.OrderService.GetOrderByIdAsync(id);
            return Ok(result);
        }

        [HttpGet( "DeliveryMethods")] // GET: /api/Orders/DeliveryMethods
        public async Task<IActionResult> GetAllDeliveryMethods()
        {
            var result = await serviceManager.OrderService.GetAllDeliveryMethod();
            return Ok(result);
        }



    }
}
