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
        public async Task<ActionResult<Transaccion>> CrearTransaccionCompra([FromBody] Transaccion transaccion)
        {
            //Validar tipo de transacción
            transaccion.CodigoTipoTransaccion = 1; //Tipo 1 = Compra
            transaccion.Fecha = DateTime.Now;

            _context.Transacciones.Add(transaccion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CrearTransaccionCompra), new { id = transaccion.CodigoTransaccion }, transaccion);
        }

        //POST: api/Transacciones/pago
        //Crear una transacción de tipo 2 Pago
        [HttpPost("pago")]
        public async Task<ActionResult<Transaccion>> CrearTransaccionPago([FromBody] Transaccion transaccion)
        {
            //Validar tipo de transacción
            transaccion.CodigoTipoTransaccion = 2; //Tipo 2 = Pago
            transaccion.Fecha = DateTime.Now;

            _context.Transacciones.Add(transaccion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CrearTransaccionPago), new { id = transaccion.CodigoTransaccion }, transaccion);
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
