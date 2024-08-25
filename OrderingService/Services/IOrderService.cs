using OrderingService.Model;

namespace OrderingService.Services
{
    public interface IOrderService
    {
        public Task<OrderItem> AddOrderItemAsync(OrderItem orderItem);
    }
}
