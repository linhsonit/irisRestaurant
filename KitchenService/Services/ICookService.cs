using KitchenService.Model;

namespace KitchenService.Services
{
    public interface ICookService
    {
        public Task<Order> PayOrderAsync(Order order);
    }
}
