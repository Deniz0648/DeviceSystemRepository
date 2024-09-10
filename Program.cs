using DeviceSystemRepository.Services.ApiServices;
using DeviceSystemRepository.Services.ManagerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace DeviceSystemRepository
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                .UseWindowsService() 
                .ConfigureServices((context, services) =>
                {
                    // Add logging
                    services.AddLogging();

                    // Add HttpClient for ApiClientService with configuration
                    services.AddHttpClient<ClientService>(client =>
                    {
                        client.BaseAddress = new Uri("https://10.198.44.35:8081/");
                    })
                    .ConfigurePrimaryHttpMessageHandler(() =>
                    {
                        return new HttpClientHandler
                        {
                            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true // Sertifika doðrulamasýný atlar (test amaçlý)
                        };
                    });

                    // Add HttpClient for PulseService with configuration
                    services.AddHttpClient<PulseService>(client =>
                    {
                        client.BaseAddress = new Uri("https://10.198.44.35:8081/");
                    })
                    .ConfigurePrimaryHttpMessageHandler(() =>
                    {
                        return new HttpClientHandler
                        {
                            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true // Sertifika doðrulamasýný atlar (test amaçlý)
                        };
                    });

                    services.AddHostedService<Worker>();
                    services.AddScoped<SystemInformationsManager>();
                });

            var host = builder.Build();
            host.Run();
        }
    }
}
