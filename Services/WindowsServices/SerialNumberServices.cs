using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSystemRepository.Services.WindowsServices
{
    internal class SerialNumberServices
    {
        public static string GetPCSerialNumber()
        {
            try
            {
                // Komutu çalıştırmak için Process sınıfını kullan
                ProcessStartInfo processStartInfo = new()
                {
                    FileName = "wmic",
                    Arguments = "bios get serialnumber",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using Process? process = Process.Start(processStartInfo); // Nullable process
                if (process != null) // Null kontrolü
                {
                    // Komutun çıktısını oku
                    using var reader = process.StandardOutput;
                    if (reader != null) // Null kontrolü
                    {
                        string output = reader.ReadToEnd();
                        // Çıktı satırlarını ayır
                        var lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                        // İlk satır başlıktı, ikinci satır ise seri numarası
                        if (lines.Length > 1)
                        {
                            return lines[1].Trim(); // Seri numarası
                        }
                        else
                        {
                            return "Seri numarası alınamadı.";
                        }
                    }
                    else
                    {
                        return "Çıktı okuyucu null.";
                    }
                }
                else
                {
                    return "Process başlatılamadı.";
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda mesaj döndür
                return $"Hata: {ex.Message}";
            }
        }
    }
}
