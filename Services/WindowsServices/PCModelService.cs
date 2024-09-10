using System;
using System.Diagnostics;

namespace DeviceSystemRepository.Services.WindowsServices
{
    internal class PCModelService
    {
        public static string GetPCModel()
        {
            try
            {
                // Komutu çalıştırmak için Process sınıfını kullan
                ProcessStartInfo processStartInfo = new()
                {
                    FileName = "wmic",
                    Arguments = "csproduct get Version",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using Process? process = Process.Start(processStartInfo); // ? kullanılarak null olma durumu kontrol edilir
                if (process != null) // Null kontrolü
                {
                    // Komutun çıktısını oku
                    using var reader = process.StandardOutput;
                    if (reader != null) // Null kontrolü
                    {
                        string output = reader.ReadToEnd();
                        // Çıktı satırlarını ayır
                        var lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                        // İlk satır başlıktı, ikinci satır ise model bilgisi
                        if (lines.Length > 1)
                        {
                            return lines[1].Trim(); // Model bilgisi
                        }
                        else
                        {
                            return "Model bilgisi alınamadı.";
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
