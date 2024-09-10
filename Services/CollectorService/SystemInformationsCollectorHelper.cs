using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DeviceSystemRepository.Models;
using DeviceSystemRepository.Services.InterfaceServices;

namespace DeviceSystemRepository.Services.CollectorService
{
    internal class SystemInformationsCollectorHelper
    {
        private static readonly ISystemInfoProvider _systemInfoProvider = SystemInfoProviderFactory.Create();

        // Bilgisayar adını al
        public static string GetHostName() => Dns.GetHostName();

        // Kullanıcı adını al
        public static string GetUserName() => Environment.UserName;

        // Network bilgilerini al
        public static List<NetworkInformationsModel> GetNetworkInformations() => _systemInfoProvider.GetNetworkInformations();

        public static string GetPCModel() => _systemInfoProvider.GetPCModel();
        public static string GetPCSerialNumber() => _systemInfoProvider.GetPCSerialNumber();

        // İşletim sistemi sürümünü al
        public static string GetOSVersion() => _systemInfoProvider.GetOSVersion();

        // IP adresini al
        public static string GetIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            return host.AddressList
                .FirstOrDefault(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.ToString()
                ?? "0.0.0.0";
        }

        // MAC adresini al
        public static string GetMACAddress()
        {
            return NetworkInterface.GetAllNetworkInterfaces()
                .FirstOrDefault(nic => nic.OperationalStatus == OperationalStatus.Up)?
                .GetPhysicalAddress().ToString() ?? "123456789abc";
        }



        // CPU modelini al
        public static string GetCpuModel() => _systemInfoProvider.GetCpuModel();

        // GPU modelini al
        public static string GetGpuModel() => _systemInfoProvider.GetGpuModel();

        // Toplam RAM miktarını al
        public static long GetTotalRam() => _systemInfoProvider.GetTotalRam();

        // Toplam disk kapasitesini al
        //public static long GetTotalDiskCapacity() => _systemInfoProvider.GetTotalDiskCapacity();

        // Toplam disk sayısını al
        public static int GetTotalDisks() => _systemInfoProvider.GetTotalDisks();

        // Disklerin sağlık bilgilerini al
        public static List<DiskInformationsModel> GetDiskInformations() => _systemInfoProvider.GetDiskInformations();

        // Takılı RAM modüllerinin sayısını al
        public static int GetInstalledRamModules() => _systemInfoProvider.GetInstalledRamModules();
    }
}
