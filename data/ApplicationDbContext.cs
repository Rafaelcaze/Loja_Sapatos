

using Microsoft.EntityFrameworkCore;
using PeDeOuro.Models;

namespace PeDeOuro.Data
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
        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<ItensPedido> ItensPedido { get; set; }
        public DbSet<PeDeOuro.Models.Promocao> Promocao { get; set; } = default!;
    }
}
