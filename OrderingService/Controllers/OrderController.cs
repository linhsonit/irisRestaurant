using Microsoft.AspNetCore.Mvc;
using OrderingService.Model;
using OrderingService.Services;

namespace OrderingService.Controllers
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

        [HttpPost("add-order-item")]
        public async Task<OrderItem> AddOrderItemAsync(OrderItem orderItem)
        {
            return await _orderService.AddOrderItemAsync(orderItem);
        }
    }
}
