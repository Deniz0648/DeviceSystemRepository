using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSystemRepository.Services.ApiServices
{
    public class PulseService(HttpClient client)
    { 
            private readonly HttpClient _client = client;

        public async Task<bool> PostStatusAsync()
            {
                // HostName'ı al
                string hostName = Dns.GetHostName();

                // Status verisini al
                bool status = true;

                // API'nin adresi
                var apiUrl = "api/Status";

                // Gönderilecek veri
                var data = new
                {
                    HostName = hostName,
                    Status = status
                };

                // JSON formatına dönüştür
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    // POST isteğini gönder
                    var response = await _client.PostAsync(apiUrl, content);
                    Console.WriteLine("Nabız Verildi");
                    // Yanıt başarılıysa true döndür
                    return response.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                    // Hata durumunda logla veya hata yönetimi yap
                    Console.WriteLine($"Hata: {ex.Message}");
                    return false;
                }
            }
        }
    }

