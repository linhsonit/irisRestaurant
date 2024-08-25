using Microsoft.EntityFrameworkCore;
using OrderingService.Model;

namespace OrderingService.Data
{
    public class OrderingDbContext : DbContext
    {
        public OrderingDbContext(DbContextOptions<OrderingDbContext> options)
            :base(options)
        {
        }

        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
