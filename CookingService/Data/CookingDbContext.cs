using CookingService.Model;
using Microsoft.EntityFrameworkCore;

namespace CookingService.Data
{
    public class CookingDbContext : DbContext
    {
        public CookingDbContext(DbContextOptions<CookingDbContext> options)
            : base(options)
        {
        }

        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
