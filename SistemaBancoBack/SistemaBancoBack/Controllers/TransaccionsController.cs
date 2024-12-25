using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaBancoBack.Context;
using SistemaBancoBack.Controllers.Services;
using SistemaBancoBack.Models;
using SistemaBancoBack.Models.DTO;

namespace SistemaBancoBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransaccionsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TransaccionService _transaccionService;
        public TransaccionsController(AppDbContext context, TransaccionService transaccionService)
        {
            _context = context;
            _transaccionService = transaccionService;
        }

        //POST: api/Transacciones/compra
        //Crear una transacción de tipo 1 Compra
        [HttpPost("compra")]
        public async Task<IActionResult> CrearCompra([FromBody] CompraDTO compraDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Mensaje = "Los datos enviados no son válidos.",
                    Errores = ModelState
                });
            }

            try
            {
                //Buscar el cliente
                var cliente = await _transaccionService.BuscarClienteAsync(compraDto.CodigoCliente);

                if (cliente == null)
                {
                    return NotFound(new { Mensaje = "Cliente no encontrado." });
                }

                //Validar el saldo disponible
                if (cliente.SaldoDisponible < compraDto.Monto)
                {
                    return BadRequest(new
                    {
                        Mensaje = "Saldo insuficiente para realizar la compra.",
                        SaldoDisponible = cliente.SaldoDisponible,
                        MontoIntentado = compraDto.Monto
                    });
                }

                //Buscar el tipo de transacción
                var tipoTransaccion = await _transaccionService.BuscarTipoTransaccionAsync(compraDto.CodigoTipoTransaccion);

                if (tipoTransaccion == null)
                {
                    return NotFound(new { Mensaje = "El tipo de transacción especificado no existe." });
                }

                //Actualizar el saldo disponible
                cliente.SaldoDisponible -= compraDto.Monto;

                //Crear la transacción de compra
                var transaccion = new Transaccion
                {
                    CodigoCliente = compraDto.CodigoCliente,
                    CodigoTipoTransaccion = compraDto.CodigoTipoTransaccion,
                    TipoTransaccion = tipoTransaccion,
                    Fecha = compraDto.Fecha,
                    Descripcion = compraDto.Descripcion,
                    Monto = compraDto.Monto
                };

                //Adjuntar los cambios
                _context.Entry(cliente).State = EntityState.Modified;
                _context.Transaccion.Add(transaccion);

                //Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Mensaje = "La compra se registró correctamente.",
                    SaldoDisponible = cliente.SaldoDisponible,
                    Transaccion = new
                    {
                        transaccion.CodigoTransaccion,
                        transaccion.Fecha,
                        transaccion.Descripcion,
                        transaccion.Monto
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Mensaje = "Error interno del servidor.",
                    Detalles = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        //POST: api/Transacciones/pago
        //Crear una transacción de tipo 2 Pago
        [HttpPost("pago")]
        public async Task<IActionResult> CrearPago([FromBody] PagoDTO pagoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Mensaje = "Los datos enviados no son válidos.",
                    Errores = ModelState
                });
            }

            try
            {
                // Usar el servicio para buscar al cliente
                var cliente = await _transaccionService.BuscarClienteAsync(pagoDto.CodigoCliente);

                if (cliente == null)
                {
                    return NotFound(new { Mensaje = "Cliente no encontrado." });
                }

                //Verificar que el monto del pago sea mayor a 0
                if (pagoDto.Monto <= 0)
                {
                    return BadRequest(new
                    {
                        Mensaje = "El monto del pago debe ser mayor a 0.",
                        Monto = pagoDto.Monto
                    });
                }

                //Buscar el tipo de transacción
                var tipoTransaccion = await _transaccionService.BuscarTipoTransaccionAsync(pagoDto.CodigoTipoTransaccion);

                if (tipoTransaccion == null)
                {
                    return NotFound(new { Mensaje = "El tipo de transacción especificado no existe." });
                }

                //Crear la transacción del pago
                var transaccion = new Transaccion
                {
                    CodigoCliente = pagoDto.CodigoCliente,
                    CodigoTipoTransaccion = pagoDto.CodigoTipoTransaccion,
                    TipoTransaccion = tipoTransaccion,
                    Fecha = pagoDto.Fecha,
                    Descripcion = pagoDto.Descripcion,
                    Monto = pagoDto.Monto
                };

               //Actualizar el saldo disponible 
                cliente.SaldoDisponible += pagoDto.Monto; 

                //Adjuntar los cambios
                _context.Entry(cliente).State = EntityState.Modified;
                _context.Transaccion.Add(transaccion);

                //Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Mensaje = "El pago se registró correctamente.",
                    SaldoDisponible = cliente.SaldoDisponible,
                    Transaccion = new
                    {
                        transaccion.CodigoTransaccion,
                        transaccion.Fecha,
                        transaccion.Descripcion,
                        transaccion.Monto
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Mensaje = "Error interno del servidor.",
                    Detalles = ex.InnerException?.Message ?? ex.Message
                });
            }
        }


        //GET: api/Transacciones
        //Traer todas las transacciones
        [HttpGet("{codigoCliente}")]
        public async Task<ActionResult<IEnumerable<Transaccion>>> ObtenerTodasLasTransacciones(int codigoCliente)
        {
            //Verificar si el cliente existe
            var cliente = await _transaccionService.BuscarClienteAsync(codigoCliente);

            if (cliente == null)
            {
                return NotFound(new { Mensaje = "Cliente no encontrado." });
            }

            //Obtener todas las transacciones del cliente
            var transacciones = await _context.Transaccion
                .Where(t => t.CodigoCliente == codigoCliente)
                .ToListAsync();

            return Ok(transacciones);
        }

        //GET: api/Transacciones/estadocuenta/{codigoCliente}
        //Ejecutar procedimiento: ObtenerEstadoGeneralCliente
        [HttpGet("estadocuenta/{codigoCliente}")]
        public async Task<ActionResult> ObtenerEstadoGeneralCliente(int codigoCliente)
        {
            //Verificar si el cliente existe
            var cliente = await _transaccionService.BuscarClienteAsync(codigoCliente);

            if (cliente == null)
            {
                return NotFound(new { Mensaje = "Cliente no encontrado." });
            }

            var resultado = await _context.Cliente
                .FromSqlInterpolated($"EXEC ObtenerEstadoGeneralCliente {codigoCliente}")
                .ToListAsync();

            return Ok(resultado);
        }

        //GET: api/Transacciones/transacciones-mes-actual/{codigoCliente}
        //Ejecutar procedimiento: ObtenerTransaccionesMesActual
        [HttpGet("transacciones-mes-actual/{codigoCliente}")]
        public async Task<ActionResult> ObtenerTransaccionesMesActual(int codigoCliente)
        {
            //Verificar si el cliente existe
            var cliente = await _transaccionService.BuscarClienteAsync(codigoCliente);

            if (cliente == null)
            {
                return NotFound(new { Mensaje = "Cliente no encontrado." });
            }

            var transacciones = await _context.Transaccion
                .FromSqlInterpolated($"EXEC ObtenerTransaccionesMesActual {codigoCliente}")
                .ToListAsync();

            return Ok(transacciones);
        }

        //GET: api/Transacciones/totales-compras/{codigoCliente}
        //Ejecutar procedimiento: ObtenerTotalesCompras
        [HttpGet("totales-compras/{codigoCliente}")]
        public async Task<ActionResult> ObtenerTotalesCompras(int codigoCliente)
        {
            //Verificar si el cliente existe
            var cliente = await _transaccionService.BuscarClienteAsync(codigoCliente);

            if (cliente == null)
            {
                return NotFound(new { Mensaje = "Cliente no encontrado." });
            }

            var totales = await _context.Database
                .ExecuteSqlInterpolatedAsync($"EXEC ObtenerTotalesCompras {codigoCliente}");

            return Ok(new { Mensaje = "Operación realizada correctamente.", Totales = totales });
        }



    }
}
