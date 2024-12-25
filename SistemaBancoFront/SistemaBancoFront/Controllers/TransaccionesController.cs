using Microsoft.AspNetCore.Mvc;
using SistemaBancoFront.Models;
using SistemaBancoFront.Models.DTO;
using SistemaBancoFront.Services;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;

namespace SistemaBancoFront.Controllers
{
    public class TransaccionesController : Controller
    {
        private readonly TransaccionService _transaccionService;

        public TransaccionesController()
        {
            _transaccionService = new TransaccionService();
        }

        //Acción para mostrar el formulario y manejar la compra
        [HttpGet]
        public IActionResult CrearCompra(int codigoCliente, string nombre, string apellido, string numeroTarjeta)
        {
            //Crear un objeto CompraDTO con los datos del cliente
            var compraDto = new CompraDTO
            {
                CodigoCliente = codigoCliente,
                Descripcion = "",  
                Monto = 0,        
                Fecha = DateTime.Now
            };

            //Pasar los datos del cliente a la vista
            ViewBag.NombreCliente = nombre;
            ViewBag.ApellidoCliente = apellido;
            ViewBag.NumeroTarjeta = numeroTarjeta;

            return View(compraDto);  
        }

        //Acción para manejar la creación de la compra desde el formulario
        [HttpPost]
        public async Task<IActionResult> CrearCompra(CompraDTO compraDto)
        {
            //Llamar al servicio para hacer la compra
            var resultado = await _transaccionService.CrearCompraAsync(compraDto);

            if (resultado)
            {
                TempData["Mensaje"] = "Compra registrada correctamente.";  //Mensaje de éxito
                return RedirectToAction("CrearCompra", new { codigoCliente = compraDto.CodigoCliente, nombre = "", apellido = "", numeroTarjeta = "" });
            }
            else
            {
                TempData["Mensaje"] = "Error al registrar la compra.";  //Mensaje de error
                return RedirectToAction("CrearCompra", new { codigoCliente = compraDto.CodigoCliente, nombre = "", apellido = "", numeroTarjeta = "" });
            }
        }


    }
}
