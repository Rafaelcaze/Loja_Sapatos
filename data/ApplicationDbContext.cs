

using Microsoft.EntityFrameworkCore;
using PeDeOutro.Models;

namespace PeDeOutro.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Defina suas DbSet aqui. Por exemplo:
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Produto> Produto { get; set; }
    }
}
