using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaBancoBack.Context;
using SistemaBancoBack.Models;
using SistemaBancoBack.Models.DTO;

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
            var clientes = await _context.Cliente
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
        public async Task<IActionResult> BuscarCliente([FromQuery] string? nombre, [FromQuery] string? numeroTarjeta)
        {
            if (string.IsNullOrWhiteSpace(nombre) && string.IsNullOrWhiteSpace(numeroTarjeta))
            {
                return BadRequest("Debe proporcionar un nombre o un número de tarjeta para buscar.");
            }

            try
            {
                var clientes = await _context.Cliente
                    .Where(c =>
                        (!string.IsNullOrWhiteSpace(nombre) && EF.Functions.Like(c.Nombre, $"%{nombre}%")) ||
                        (!string.IsNullOrWhiteSpace(numeroTarjeta) && c.NumeroTarjeta == numeroTarjeta))
                    .Select(c => new
                    {
                        c.CodigoCliente,
                        c.NumeroTarjeta,
                        c.Nombre,
                        c.Apellido
                    })
                    .ToListAsync();

                if (!clientes.Any())
                {
                    return NotFound("No se encontraron clientes que coincidan con los criterios de búsqueda.");
                }

                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }


        //POST: api/Clientes
        //Crear un nuevo cliente
        [HttpPost]
        public async Task<IActionResult> CrearCliente([FromBody] CrearClienteDTO clienteDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                //Verificar si el número de tarjeta ya existe
                if (await _context.Cliente.AnyAsync(c => c.NumeroTarjeta == clienteDto.NumeroTarjeta))
                {
                    return BadRequest("El número de tarjeta ya está asociado a otro cliente.");
                }

                var nuevoCliente = new Cliente
                {
                    Nombre = clienteDto.Nombre,
                    Apellido = clienteDto.Apellido,
                    NumeroTarjeta = clienteDto.NumeroTarjeta,
                    LimiteCredito = clienteDto.LimiteCredito,
                    SaldoDisponible = clienteDto.SaldoDisponible
                };

                _context.Cliente.Add(nuevoCliente);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(BuscarCliente), new { id = nuevoCliente.CodigoCliente }, new
                {
                    Mensaje = "Cliente creado exitosamente.",
                    Cliente = new
                    {
                        nuevoCliente.CodigoCliente,
                        nuevoCliente.Nombre,
                        nuevoCliente.Apellido,
                        nuevoCliente.NumeroTarjeta,
                        nuevoCliente.LimiteCredito,
                        nuevoCliente.SaldoDisponible
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

    }
}
