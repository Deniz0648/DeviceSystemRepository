using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSystemRepository.Services.WindowsServices
{
    internal class GpuService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Platform uyumluluğunu doğrula", Justification = "<bekleyen>")]
        public static string GetGpuModel()
        {
            try
            {
                // GPU bilgilerini almak için WMI sorgusu oluştur
                using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");

                // Sorguyu çalıştır ve ilk GPU nesnesini al
                var gpu = searcher.Get().Cast<ManagementObject>().FirstOrDefault();

                // GPU ismini döndür, eğer bulunamazsa "Bilinmeyen GPU" döndür
                return gpu?["Name"]?.ToString() ?? "Bilinmeyen GPU";
            }
            catch (Exception ex)
            {
                // Hata durumunda "GPU bilgisi alınamadı" döndür ve hata mesajını konsola yazdır
                Console.WriteLine($"GPU bilgisi alınırken hata oluştu: {ex.Message}");
                return "GPU bilgisi alınamadı";
            }
        }
    }
}
