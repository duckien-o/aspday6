using Microsoft.EntityFrameworkCore;
using day6_17.Models;

namespace day6_17.data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
    }
}