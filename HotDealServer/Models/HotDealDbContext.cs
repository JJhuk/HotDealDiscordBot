using Microsoft.EntityFrameworkCore;

namespace HotDealServer.Models
{
    public class HotDealDbContext : DbContext
    {
        public HotDealDbContext(DbContextOptions<HotDealDbContext> options) : base(options)
        {
            
        }

        public DbSet<HotDealProduct> Products { get; set; }
    }
}