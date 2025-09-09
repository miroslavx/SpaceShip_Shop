
using Microsoft.EntityFrameworkCore;
using shoptar.Core.Domain;

namespace shoptar.Data
{
    public class ShoptarContext : DbContext
    {
        public ShoptarContext(DbContextOptions<ShoptarContext>options)
        :base(options) { }

        public DbSet <Spaceship> Spaceships { get; set; }
    }
}
