using DeviceSystemRepository.Models;
using DeviceSystemRepository.Services.ProviderServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSystemRepository.Services.InterfaceServices
{
    internal interface ISystemInfoProvider
    {
        public string GetPCSerialNumber();
        public string GetPCModel();
        public string GetOSVersion();
        string GetCpuModel(); // CPU modelini al
        string GetGpuModel(); // GPU modelini al
        long GetTotalRam(); // Toplam RAM miktarını al
        int GetTotalDisks(); // Toplam disk sayısını al
        List<NetworkInformationsModel> GetNetworkInformations(); // Disklerin sağlık bilgilerini al
        List<DiskInformationsModel> GetDiskInformations(); // Disklerin sağlık bilgilerini al
        int GetInstalledRamModules(); // Takılı RAM modüllerinin sayısını al
    }

    // Sistem bilgi sağlayıcılarının üreticisi
    internal static class SystemInfoProviderFactory
    {
        // Uygun sistem bilgi sağlayıcısını oluşturur
        public static ISystemInfoProvider Create()
        {
            if (OperatingSystem.IsWindows())
            {
                // Windows işletim sistemi için bilgi sağlayıcıyı döndür
                return new WindowsSystemInformationProvider();
            }
            else
            {
                // Desteklenmeyen bir platform için hata fırlat
                throw new PlatformNotSupportedException("Mevcut platform desteklenmiyor.");
            }
        }
    }
}
