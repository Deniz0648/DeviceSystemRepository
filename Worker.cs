using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using DeviceSystemRepository.Services.ManagerServices;
using DeviceSystemRepository.Services.ApiServices;

namespace DeviceSystemRepository
{
    public class Worker(IServiceProvider serviceProvider, PulseService pulseService) : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        private readonly PulseService _pulseService = pulseService ?? throw new ArgumentNullException(nameof(pulseService));

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Worker ba�lat�ld�.");

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var systemInformationsManager = scope.ServiceProvider.GetRequiredService<SystemInformationsManager>();
                    await systemInformationsManager.ManageSystemInformationsAsync();
                }
                // PulseService'i kullanarak nab�z verisi g�nder
                await _pulseService.PostStatusAsync();
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken); // Her 5 dakikada bir �al��t�r
            }

            Console.WriteLine("Worker durduruldu.");
        }
    }
}