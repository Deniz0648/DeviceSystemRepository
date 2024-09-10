using DeviceSystemRepository.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DeviceSystemRepository.Services.ApiServices
{
    public class ClientService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

        internal async Task PostAsync(SystemInformationsModel systemInformations)
        {
            var jsonContent = JsonSerializer.Serialize(systemInformations, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("api/Devices", httpContent);

                if (!response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Hata: {response.StatusCode}, Yanıt: {responseContent}");
                    throw new HttpRequestException($"HTTP isteği başarısız oldu: {response.StatusCode}, Yanıt: {responseContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"İstek Hatası: {ex.Message}");
                throw;
            }
        }
    }
}