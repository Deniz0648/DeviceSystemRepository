using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSystemRepository.Services.WindowsServices
{
    internal class OsService
    {
        private static readonly char[] separator = ['\r', '\n'];

        public static string GetOSInfo()
        {
            try
            {
                // WMIC komutunu çalıştırmak için Process sınıfını kullan
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "wmic",
                        Arguments = "os get Caption /value",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                // Çıktıyı işleyerek sadece değeri döndür
                var lines = output.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    if (line.StartsWith("Caption=", StringComparison.OrdinalIgnoreCase))
                    {
                        // "Caption=" kısmını temizle ve değeri döndür
                        return line["Caption=".Length..].Trim();
                    }
                }

                // Eğer "Caption=" bulunmazsa hata mesajı döndür
                return "Bilgi alınamadı.";
            }
            catch (Exception ex)
            {
                // Hata durumunda hata mesajını döndür
                return $"Hata: {ex.Message}";
            }
        }
    }
}
