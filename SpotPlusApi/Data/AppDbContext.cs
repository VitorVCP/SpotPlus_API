using Microsoft.EntityFrameworkCore;
using SpotPlusApi.Models;

namespace SpotPlusApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Estacionamento> Estacionamentos { get; set; }
        public DbSet<Estadia> Estadias { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }
    }
}