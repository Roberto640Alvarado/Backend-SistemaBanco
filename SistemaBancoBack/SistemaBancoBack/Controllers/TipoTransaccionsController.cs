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
    public class TipoTransaccionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TipoTransaccionsController(AppDbContext context)
        {
            _context = context;
        }

        //POST: api/TipoTransacciones
        //Crear un tipo de transacción
        [HttpPost]
        public async Task<ActionResult<TipoTransaccion>> CrearTipoTransaccion([FromBody] TipoTransaccion tipoTransaccion)
        {
            if (tipoTransaccion == null || string.IsNullOrWhiteSpace(tipoTransaccion.Descripcion))
            {
                return BadRequest("La descripción del tipo de transacción es obligatoria.");
            }

            _context.TipoTransacciones.Add(tipoTransaccion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CrearTipoTransaccion), new { id = tipoTransaccion.CodigoTipoTransaccion }, tipoTransaccion);
        }
    }
}
