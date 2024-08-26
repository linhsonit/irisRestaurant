using System.ComponentModel.DataAnnotations;

namespace KitchenService.Model
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public string CustomerName { get; set; }
        public string Meals { get; set; }
        public string Status { get; set; }
    }
}
