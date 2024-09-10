using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSystemRepository.Services.WindowsServices
{
    internal class CpuService
    {

        // CPU modelini almak için metod
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Platform uyumluluğunu doğrula", Justification = "<bekleyen>")]
        public static string GetCpuModel()
        {

            try
            {
                // Yönetim nesnelerini aramak için bir arayıcı oluştur
                using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");

                // Arama sonuçlarını al ve ilk CPU nesnesini seç
                var cpu = searcher.Get().Cast<ManagementObject>().FirstOrDefault();

                // CPU'nun adını al, eğer mevcut değilse "Bilinmeyen CPU" döndür
                return cpu?["Name"]?.ToString() ?? "Bilinmeyen CPU";

            }
            catch
            {
                // Hata durumunda "CPU bilgisi alınamadı" döndür
                return "CPU bilgisi alınamadı";
            }
        }
    }
}
