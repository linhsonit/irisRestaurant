using Microsoft.EntityFrameworkCore;
using KitchenService.Model;

namespace KitchenService.Data
{
    public class KitchenDbContext : DbContext
    {
        public KitchenDbContext(DbContextOptions<KitchenDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
    }
}
