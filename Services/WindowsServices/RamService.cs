using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSystemRepository.Services.WindowsServices
{
    internal class RamService
    {
        private static readonly string[] separator = ["\r\n", "\r", "\n"];
        private static readonly string[] separatorArray = ["\r\n", "\r", "\n"];

        // Toplam RAM kapasitesini al
        public static long GetTotalRam()
        {
            try
            {
                // RAM kapasitelerini almak için komutu çalıştır
                var ramCapacities = RunCommand("wmic MEMORYCHIP get Capacity");

                // Çıktıyı satırlara böl ve başlığı atla
                var capacities = ramCapacities.Split(separator, StringSplitOptions.RemoveEmptyEntries)
                                               .Skip(1) // İlk satır başlıktır, atlanmalıdır.
                                               .Select(line => Convert.ToInt64(line.Trim())) // Satırları uzun tamsayıya dönüştür
                                               .ToList();

                // Toplam kapasiteyi hesapla
                long totalCapacity = capacities.Sum();

                return totalCapacity;
            }
            catch (Exception ex)
            {
                // Hata durumunda 0 döndür ve hata mesajını konsola yazdır
                Console.WriteLine($"RAM kapasitesi alınırken hata oluştu: {ex.Message}");
                return 0; // Hata durumunda 0 döndür
            }
        }

        // Takılı RAM modüllerinin sayısını al
        public static int GetInstalledRamModules()
        {
            try
            {
                // RAM modüllerini almak için komutu çalıştır
                var installedRamModulesInfo = RunCommand("wmic MEMORYCHIP get Capacity");

                // Çıktıyı satırlara böl ve başlığı atla
                return installedRamModulesInfo.Split(separatorArray, StringSplitOptions.RemoveEmptyEntries)
                                               .Skip(1) // İlk satır başlıktır, atlanmalıdır.
                                               .Count(); // Satır sayısını döndür
            }
            catch (Exception ex)
            {
                // Hata durumunda 0 döndür ve hata mesajını konsola yazdır
                Console.WriteLine($"RAM modüllerinin sayısı alınırken hata oluştu: {ex.Message}");
                return 0; // Hata durumunda 0 döndür
            }
        }

        // Komutu çalıştır ve çıktıyı al
        private static string RunCommand(string command)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c {command}", // Komut satırında çalıştırılacak komut
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            var output = process.StandardOutput.ReadToEnd(); // Çıktıyı oku
            process.WaitForExit(); // Komutun bitmesini bekle

            return output;
        }
    }
}
