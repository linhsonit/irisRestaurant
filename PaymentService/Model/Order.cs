namespace PaymentService.Model
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public string CustomerName { get; set; }

        public string Status { get; set; }
    }
}
