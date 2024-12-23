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
    public class ConfiguracionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ConfiguracionsController(AppDbContext context)
        {
            _context = context;
        }

        //POST: api/Configuracion/configuracion
        //Crear una nueva configuración
        [HttpPost("configuracion")]
        public async Task<ActionResult<Configuracion>> CrearConfiguracion([FromBody] Configuracion configuracion)
        {
            _context.Configuraciones.Add(configuracion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CrearConfiguracion), new { id = configuracion.CodigoConfiguracion }, configuracion);
        }

        //PUT: api/Configuracion/configuracion/{id}
        //Modificar una configuración existente
        [HttpPut("configuracion/{id}")]
        public async Task<ActionResult> ModificarConfiguracion(int id, [FromBody] Configuracion configuracion)
        {
            if (id != configuracion.CodigoConfiguracion)
            {
                return BadRequest("El codigo de la configuración no coincide.");
            }

            var configuracionExistente = await _context.Configuraciones.FindAsync(id);
            if (configuracionExistente == null)
            {
                return NotFound("Configuración no encontrada.");
            }

            //Actualizar los valores de la configuración
            configuracionExistente.PorcentajeInteres = configuracion.PorcentajeInteres;
            configuracionExistente.PorcentajeSaldoMinimo = configuracion.PorcentajeSaldoMinimo;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
