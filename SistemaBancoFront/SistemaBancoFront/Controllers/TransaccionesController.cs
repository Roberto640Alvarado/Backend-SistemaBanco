using Microsoft.AspNetCore.Mvc;
using SistemaBancoFront.Models;
using SistemaBancoFront.Models.DTO;
using System.Net.Http.Headers;
using System.Transactions;
using SistemaBancoFront.Models.ViewModels;

namespace SistemaBancoFront.Controllers
{
    public class TransaccionesController : Controller
    {
        private readonly HttpClient httpClient;

        public TransaccionesController(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri("http://localhost:5137/");

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<IActionResult> DetailsTransaccion(int codigoCliente)
        {
            //Consultar los datos del cliente
            var respuestaCliente = await httpClient.GetAsync($"api/Clientes/{codigoCliente}");
            if (!respuestaCliente.IsSuccessStatusCode)
            {
                return View("Error");
            }
            var cliente = await respuestaCliente.Content.ReadAsAsync<Cliente>();

            //Consultar las transacciones del cliente
            var respuestaTransacciones = await httpClient.GetAsync($"api/Transaccions/{codigoCliente}");
            if (!respuestaTransacciones.IsSuccessStatusCode)
            {
                return View("Error");
            }
            var transacciones = await respuestaTransacciones.Content.ReadAsAsync<List<Transaccion>>();

            //Construir el modelo de vista
            var clienteViewModel = new ClienteTransaccionesViewModel
            {
                CodigoCliente = cliente.CodigoCliente,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                NumeroTarjeta = cliente.NumeroTarjeta,
                Transacciones = transacciones
            };

            return View(clienteViewModel);
        }


        public async Task<IActionResult> CreateCompra(int codigoCliente)
        {
            var respuesta = await httpClient.GetAsync($"api/Clientes/{codigoCliente}");
            if (respuesta.IsSuccessStatusCode)
            {
                var cliente = await respuesta.Content.ReadAsAsync<Cliente>();

                var viewModel = new CompraViewModel
                {
                    Cliente = cliente,
                    Compra = new CompraDTO
                    {
                        CodigoCliente = cliente.CodigoCliente
                    }
                };

                return View(viewModel);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompra(CompraViewModel viewModel)
        {
            var respuesta = await httpClient.PostAsJsonAsync("api/Transaccions/compra", viewModel.Compra);
            if (respuesta.IsSuccessStatusCode)
            {
                return Redirect("/");
            }
            else
            {
                return View("Error");
            }
        }

        public async Task<IActionResult> CreatePago(int codigoCliente)
        {
            var respuesta = await httpClient.GetAsync($"api/Clientes/{codigoCliente}");
            if (respuesta.IsSuccessStatusCode)
            {
                var cliente = await respuesta.Content.ReadAsAsync<Cliente>();

                var viewModel = new PagoViewModel
                {
                    Cliente = cliente,
                    Pago = new PagoDTO
                    {
                        CodigoCliente = cliente.CodigoCliente
                    }
                };

                return View(viewModel);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePago(PagoViewModel viewModel)
        {
            var respuesta = await httpClient.PostAsJsonAsync("api/Transaccions/pago", viewModel.Pago);
            if (respuesta.IsSuccessStatusCode)
            {
                return Redirect("/");
            }
            else
            {
                return View("Error");
            }
        }





    }
}
