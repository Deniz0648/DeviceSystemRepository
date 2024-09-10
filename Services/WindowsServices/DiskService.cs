using DeviceSystemRepository.Models;
using DeviceSystemRepository.Services.CollectorService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSystemRepository.Services.WindowsServices
{
    internal class DiskService
    {
        // Disklerin sağlık durumunu almak için metod
        public static List<DiskInformationsModel> GetDiskInformations()
        {
            try
            {
                // WMIC komutunu kullanarak disklerin sağlık durumu ve boyutunu al
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "wmic",
                        Arguments = "diskdrive get Index,MediaType,Size,Status,SystemName",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                var output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                // Çıktıyı konsola logla
                //Console.WriteLine("WMIC Komutu Çıktısı:");
                //Console.WriteLine(output);

                // Çıktıları ayrıştır ve listeye dönüştür
                return ParseDiskHealthOutput(output);
            }
            catch (Exception ex)
            {
                // Hata durumunda hata mesajı içeren bir liste döndür
                return
                [
                    new() {
                        HostName = SystemInformationsCollectorHelper.GetHostName(),
                        DiskNumber = -1, // Disk numarası bilinmediğinden -1 ata
                        Status = $"Disk sağlık bilgisi alınamadı: {ex.Message}",
                        DiskCapacity = 0, // Hata durumunda kapasite 0 olarak ayarla
                        MediaType = "Tip Yok"
                    }
                ];
            }
        }

        private static readonly string[] LineSeparators = ["\r\n", "\r", "\n"];
        internal static readonly char[] separator = [' '];

        // Disk sağlık ve boyut çıktısını ayrıştırarak DiskHealthInfo nesneleri listesine dönüştürür
        private static List<DiskInformationsModel> ParseDiskHealthOutput(string output)
        {
            // Çıktıyı satırlara ayır
            var lines = output.Split(LineSeparators, StringSplitOptions.RemoveEmptyEntries)
                              .Skip(1) // İlk satır başlıktır, bu yüzden atlanır
                              .Select(line => line.Trim())
                              .Where(line => !string.IsNullOrEmpty(line))
                              .ToList();

            // Sağlık bilgilerini listeye dönüştür
            var healthInfoList = new List<DiskInformationsModel>();

            foreach (var line in lines)
            {
                // Satırı ayır ve her bir parçayı trimle
                var parts = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 5)
                {
                    // Disk bilgilerini ayır
                    var index = int.Parse(parts[0]);
                    var mediaType = string.Join(' ', parts.Skip(1).Take(parts.Length - 4)); // MediaType alanını al
                    var size = parts[^3];
                    var status = parts[^2];
                    var systemName = parts[^1];

                    healthInfoList.Add(new DiskInformationsModel
                    {
                        HostName = systemName,
                        DiskNumber = index,
                        DiskCapacity = long.Parse(size),
                        Status = status,
                        MediaType = mediaType
                    });
                }
                else
                {
                    // Format hatası durumunda bir uyarı yaz
                    Console.WriteLine($"Beklenmeyen format: {line}");
                }
            }

            return healthInfoList;
        }
    }
}
