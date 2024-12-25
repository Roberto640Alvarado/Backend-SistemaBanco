using Microsoft.EntityFrameworkCore;
using SistemaBancoBack.Models;

namespace SistemaBancoBack.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<TipoTransaccion> TipoTransaccion { get; set; }
        public DbSet<Transaccion> Transaccion { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Configuracion> Configuracion { get;set; }
    }   
}
