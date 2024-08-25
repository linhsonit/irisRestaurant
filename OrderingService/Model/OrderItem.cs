namespace OrderingService.Model
{
    public class OrderItem
    {
        public Guid OrderItemId { get; set; } = Guid.NewGuid();
        public string DishName { get; set; }
        public int Quantity {  get; set; }
    }
}
