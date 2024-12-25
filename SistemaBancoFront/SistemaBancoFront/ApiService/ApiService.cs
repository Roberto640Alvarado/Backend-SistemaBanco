using SistemaBancoFront.Models;
using System.Text.Json;

namespace SistemaBancoFront.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5137/")
            };
        }
            //Método para buscar clientes
            public async Task<List<Cliente>> BuscarClientesAsync(string? nombre, string? numeroTarjeta)
            {
                var query = new Dictionary<string, string>();
                if (!string.IsNullOrWhiteSpace(nombre))
                    query.Add("nombre", nombre);
                if (!string.IsNullOrWhiteSpace(numeroTarjeta))
                    query.Add("numeroTarjeta", numeroTarjeta);

                var queryString = string.Join("&", query.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
                var response = await _httpClient.GetAsync($"api/Clientes/buscar?{queryString}");

                if (response.IsSuccessStatusCode)
                {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Cliente>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

                return new List<Cliente>();
            }

        //Método para obtener todos los clientes
        public async Task<List<Cliente>> ObtenerTodosLosClientesAsync()
        {
            var response = await _httpClient.GetAsync("api/Clientes");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Cliente>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            return new List<Cliente>();
        }
    }
}
