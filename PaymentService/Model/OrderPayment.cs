using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentService.Model
{
    public class OrderPayment
    {
        public Guid OrderPaymentId { get; set; } = Guid.NewGuid();
        public Guid OrderId { get; set; }
        public string CustomerName { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }
        public bool IsPaid { get; set; }
    }
}
