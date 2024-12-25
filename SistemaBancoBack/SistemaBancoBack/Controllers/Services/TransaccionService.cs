using Microsoft.EntityFrameworkCore;
using SistemaBancoBack.Context;
using SistemaBancoBack.Models;

namespace SistemaBancoBack.Controllers.Services
{
    public class TransaccionService
    {
        private readonly AppDbContext _context;

        // Inyectar ApplicationDbContext
        public TransaccionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Cliente> BuscarClienteAsync(int codigoCliente)
        {
            return await _context.Cliente
                .Where(c => c.CodigoCliente == codigoCliente)
                .FirstOrDefaultAsync();
        }

        public async Task<TipoTransaccion> BuscarTipoTransaccionAsync(int codigoTipoTransaccion)
        {
            return await _context.TipoTransaccion
                .FirstOrDefaultAsync(t => t.CodigoTipoTransaccion == codigoTipoTransaccion);
        }
    }
}
