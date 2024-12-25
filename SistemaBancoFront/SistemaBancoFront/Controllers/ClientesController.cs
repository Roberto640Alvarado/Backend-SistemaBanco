using Microsoft.AspNetCore.Mvc;
using SistemaBancoFront.Services;
using System.Linq;

namespace SistemaBancoFront.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ApiService _apiService;

        public ClientesController()
        {
            _apiService = new ApiService();
        }

        //Mostrar la vista inicial con todos los clientes
        public async Task<IActionResult> Index()
        {
            var clientes = await _apiService.ObtenerTodosLosClientesAsync();
            return View(clientes);
        }

        [HttpPost]
        public async Task<IActionResult> Buscar(string? busqueda)
        {
            List<SistemaBancoFront.Models.Cliente> clientes;

            if (string.IsNullOrEmpty(busqueda))
            {
                //Si el campo está vacío, trae todos los clientes
                clientes = await _apiService.ObtenerTodosLosClientesAsync();
            }
            else
            {
                //Si hay un valor en busqueda, lo pasamos a la API para buscar
                clientes = await _apiService.BuscarClientesAsync(busqueda, busqueda);
            }

            return PartialView("_ClientesTablePartial", clientes);
        }



    }
}
