using SistemaBancoFront.Models;
using SistemaBancoFront.Models.DTO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SistemaBancoFront.Services
{
    public class TransaccionService
    {
        private readonly HttpClient _httpClient;

        public TransaccionService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5137/")
            };
        }

        //Método para crear una compra
        public async Task<bool> CrearCompraAsync(CompraDTO compraDto)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(compraDto), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Transaccions/compra", jsonContent);

            return response.IsSuccessStatusCode;
        }

    }
}
