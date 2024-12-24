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
    public class TransaccionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransaccionsController(AppDbContext context)
        {
            _context = context;
        }

        //POST: api/Transacciones/compra
        //Crear una transacción de tipo 1 Compra
        [HttpPost("compra")]
        public async Task<IActionResult> CrearCompra([FromBody] CompraDTO compraDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Mensaje = "Los datos enviados no son válidos.", Errores = ModelState });

            try
            {
                //Buscar el cliente
                var cliente = await _context.Clientes.FindAsync(compraDto.CodigoCliente);
                if (cliente == null)
                    return NotFound(new { Mensaje = "Cliente no encontrado." });

                //Validar límite de crédito
                if (cliente.SaldoDisponible + compraDto.Monto > cliente.LimiteCredito)
                {
                    return BadRequest(new
                    {
                        Mensaje = "La compra excede el límite de crédito disponible.",
                        LimiteCredito = cliente.LimiteCredito,
                        SaldoDisponible = cliente.SaldoDisponible,
                        MontoIntentado = compraDto.Monto
                    });
                }

                //Crear transacción
                var transaccion = new Transaccion
                {
                    CodigoCliente = compraDto.CodigoCliente,
                    CodigoTipoTransaccion = compraDto.CodigoTipoTransaccion,
                    Fecha = compraDto.Fecha,
                    Descripcion = compraDto.Descripcion,
                    Monto = compraDto.Monto
                };

                _context.Transacciones.Add(transaccion);

                //Actualizar saldo disponible del cliente
                cliente.SaldoDisponible += compraDto.Monto;

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Mensaje = "La transacción de compra se creó correctamente.",
                    Transaccion = new
                    {
                        transaccion.CodigoTransaccion,
                        transaccion.CodigoCliente,
                        transaccion.CodigoTipoTransaccion,
                        transaccion.Fecha,
                        transaccion.Descripcion,
                        transaccion.Monto
                    }
                });
            }
            catch (Exception ex)
            {
                //Manejar errores inesperados
                return StatusCode(500, new { Mensaje = "Error interno del servidor.", Detalles = ex.InnerException?.Message ?? ex.Message });
            }
        }

        //POST: api/Transacciones/pago
        //Crear una transacción de tipo 2 Pago
        [HttpPost("pago")]
        public async Task<IActionResult> CrearPago([FromBody] PagoDTO pagoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Los datos enviados no son válidos.");

            try
            {
                var cliente = await _context.Clientes.FindAsync(pagoDto.CodigoCliente);
                if (cliente == null)
                    return NotFound("Cliente no encontrado.");

                var transaccion = new Transaccion
                {
                    CodigoCliente = pagoDto.CodigoCliente,
                    CodigoTipoTransaccion = pagoDto.CodigoTipoTransaccion,
                    Fecha = pagoDto.Fecha,
                    Descripcion = pagoDto.Descripcion,
                    Monto = pagoDto.Monto
                };

                _context.Transacciones.Add(transaccion);

                //Actualizar saldo disponible
                cliente.SaldoDisponible -= pagoDto.Monto;

                await _context.SaveChangesAsync();

                return Ok("El pago se realizó correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        //GET: api/Transacciones
        //Traer todas las transacciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaccion>>> ObtenerTodasLasTransacciones()
        {
            var transacciones = await _context.Transacciones.ToListAsync();
            return Ok(transacciones);
        }


    }
}
