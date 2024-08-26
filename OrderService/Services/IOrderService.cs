using OrderService.Model;

namespace OrderService.Services
{
    public interface IOrderService
    {
        public Task<Order> AddOrderItemAsync(Order order);
    }
}
