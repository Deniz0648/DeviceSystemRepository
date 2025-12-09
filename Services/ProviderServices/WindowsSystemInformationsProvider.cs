using DeviceSystemRepository.Models;
using DeviceSystemRepository.Services.InterfaceServices;
using DeviceSystemRepository.Services.WindowsServices;
using System.Management;


namespace DeviceSystemRepository.Services.ProviderServices
{
    internal class WindowsSystemInformationProvider : ISystemInfoProvider
    {
        public string GetPCSerialNumber() => SerialNumberServices.GetPCSerialNumber();
        public string GetPCModel() => PCModelService.GetPCModel();
        public List<MonitorInformationsModel> GetMonitorInformations() => MonitorService.GetMonitorInformations();
        public string GetOSVersion() => OsService.GetOSInfo();

        // CPU modelini al
        public string GetCpuModel() => CpuService.GetCpuModel();

        // GPU modelini al
        public string GetGpuModel() => GpuService.GetGpuModel();

        // Toplam RAM miktarını al
        public long GetTotalRam() => RamService.GetTotalRam();

        // Takılı RAM modüllerinin sayısını al
        public int GetInstalledRamModules() => RamService.GetInstalledRamModules();

        // Toplam disk sayısını al
        public int GetTotalDisks()
        {
            int physicalDisksCount = 0;

            try
            {
                // WMI sorgusu ile fiziksel diskleri al
                var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

                foreach (ManagementObject disk in searcher.Get().Cast<ManagementObject>())
                {
                    // Her fiziksel disk için sayacı artır
                    physicalDisksCount++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
            }

            return physicalDisksCount;
        }

        // Disklerin sağlık bilgilerini al
        public List<NetworkInformationsModel> GetNetworkInformations() => NetworkService.GetNetworkInformations();

        // Disklerin sağlık bilgilerini al
        public List<DiskInformationsModel> GetDiskInformations() => DiskService.GetDiskInformations();
    }
}
