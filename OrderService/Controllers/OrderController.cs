using Microsoft.AspNetCore.Mvc;
using OrderService.Model;
using OrderService.Services;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("add-order")]
        public async Task<Order> AddOrderItemAsync(Order order)
        {
            return await _orderService.AddOrderItemAsync(order);
        }
    }
}
