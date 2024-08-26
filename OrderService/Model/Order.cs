namespace OrderService.Model
{
    public class Order
    {
        public Guid OrderId { get; set; } = Guid.NewGuid();
        public string CustomerName { get; set; }
        public string Meals { get; set; }
        public string Status { get; set; }
    }
}
