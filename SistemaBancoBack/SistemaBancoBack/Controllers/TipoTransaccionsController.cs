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
    public class TipoTransaccionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TipoTransaccionsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CrearTipoTransaccion([FromBody] TipoTransaccionDTO tipoTransaccionDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Los datos enviados no son válidos.");

            try
            {
                var tipoTransaccion = new TipoTransaccion
                {
                    Descripcion = tipoTransaccionDto.Descripcion
                };

                _context.TipoTransacciones.Add(tipoTransaccion);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(ObtenerTodosLosTiposTransaccion), new { id = tipoTransaccion.CodigoTipoTransaccion }, new
                {
                    Mensaje = "El tipo de transacción se creó correctamente.",
                    TipoTransaccion = tipoTransaccion
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        //GET: api/TipoTransacciones
        //Mostrar todos los tipos de transacción
        [HttpGet]
        public async Task<IActionResult> ObtenerTodosLosTiposTransaccion()
        {
            try
            {
                var tiposTransaccion = await _context.TipoTransacciones.ToListAsync();

                if (!tiposTransaccion.Any())
                    return NotFound("No se encontraron tipos de transacción.");

                return Ok(tiposTransaccion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}
