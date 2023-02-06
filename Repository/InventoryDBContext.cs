using Microsoft.EntityFrameworkCore;

namespace InventoryManagementApp.Repository
{
    public class InventoryDBContext : DbContext
    {
        public InventoryDBContext(DbContextOptions<InventoryDBContext> options) : base(options)
        {
        }
        public DbSet<Item> Item { get; set; }

    }
}
