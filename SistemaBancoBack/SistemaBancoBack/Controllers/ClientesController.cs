using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaBancoBack.Context;
using SistemaBancoBack.Models;

namespace SistemaBancoBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        //GET: api/Clientes
        //Traer todos los clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetClientes()
        {
            var clientes = await _context.Clientes
                .Select(c => new
                {   c.CodigoCliente,
                    c.NumeroTarjeta,
                    c.Nombre,
                    c.Apellido
                })
                .ToListAsync();

            return Ok(clientes);
        }

        //GET: api/Clientes/buscar
        //Buscar cliente por nombre o número de tarjeta
        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<object>>> BuscarCliente([FromQuery] string? nombre, [FromQuery] string? numeroTarjeta)
        {
            if (string.IsNullOrEmpty(nombre) && string.IsNullOrEmpty(numeroTarjeta))
            {
                return BadRequest("Debe proporcionar un nombre o un número de tarjeta para buscar.");
            }

            var clientes = await _context.Clientes
                .Where(c => (!string.IsNullOrEmpty(nombre) && EF.Functions.Like(c.Nombre, $"%{nombre}%")) ||
                            (!string.IsNullOrEmpty(numeroTarjeta) && c.NumeroTarjeta == numeroTarjeta))
                .Select(c => new
                {   c.CodigoCliente,
                    c.NumeroTarjeta,
                    c.Nombre,
                    c.Apellido
                })
                .ToListAsync();

            return Ok(clientes);
        }

        //POST: api/Clientes
        //Crear un nuevo cliente
        [HttpPost]
        public async Task<ActionResult<Cliente>> CrearCliente(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetClientes), new { id = cliente.CodigoCliente }, cliente);
        }

        //GET: api/Clientes/estadocuenta/{codigoCliente}
        //Ejecutar procedimiento: ObtenerEstadoGeneralCliente
        [HttpGet("estadocuenta/{codigoCliente}")]
        public async Task<ActionResult> ObtenerEstadoGeneralCliente(int codigoCliente)
        {
            var resultado = await _context.Clientes
                .FromSqlInterpolated($"EXEC ObtenerEstadoGeneralCliente {codigoCliente}")
                .ToListAsync();

            return Ok(resultado);
        }

        //GET: api/Clientes/transacciones-mes-actual/{codigoCliente}
        //Ejecutar procedimiento: ObtenerTransaccionesMesActual
        [HttpGet("transacciones-mes-actual/{codigoCliente}")]
        public async Task<ActionResult> ObtenerTransaccionesMesActual(int codigoCliente)
        {
            var transacciones = await _context.Transacciones
                .FromSqlInterpolated($"EXEC ObtenerTransaccionesMesActual {codigoCliente}")
                .ToListAsync();

            return Ok(transacciones);
        }

        //GET: api/Clientes/totales-compras/{codigoCliente}
        //Ejecutar procedimiento: ObtenerTotalesCompras
        [HttpGet("totales-compras/{codigoCliente}")]
        public async Task<ActionResult> ObtenerTotalesCompras(int codigoCliente)
        {
            var totales = await _context.Database
                .ExecuteSqlInterpolatedAsync($"EXEC ObtenerTotalesCompras {codigoCliente}");

            return Ok(totales);
        }

    }
}
