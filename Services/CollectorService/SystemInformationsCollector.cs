using DeviceSystemRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSystemRepository.Services.CollectorService
{
    internal class SystemInformationsCollector
    {
        public static SystemInformationsModel GetSystemInformations()
        {
            return new SystemInformationsModel
            {
                HostName = SystemInformationsCollectorHelper.GetHostName(), // Bilgisayar adını al
                UserName = SystemInformationsCollectorHelper.GetUserName(), // Kullanıcı adını al
                Networks = SystemInformationsCollectorHelper.GetNetworkInformations(),
                PCModel = SystemInformationsCollectorHelper.GetPCModel(),
                PCSerialNumber = SystemInformationsCollectorHelper.GetPCSerialNumber(),
                OSVersion = SystemInformationsCollectorHelper.GetOSVersion(), // İşletim sistemi sürümünü al
                IPAddress = SystemInformationsCollectorHelper.GetIPAddress(), // IP adresini al
                MACAddress = SystemInformationsCollectorHelper.GetMACAddress(), // MAC adresini al
                CpuModel = SystemInformationsCollectorHelper.GetCpuModel(), // CPU modelini al
                GpuModel = SystemInformationsCollectorHelper.GetGpuModel(), // GPU modelini al
                TotalRam = SystemInformationsCollectorHelper.GetTotalRam(), // Toplam RAM miktarını al
                Disks = SystemInformationsCollectorHelper.GetDiskInformations(), // Disk sağlık bilgilerini al
                TotalDisks = SystemInformationsCollectorHelper.GetTotalDisks(), // Toplam disk sayısını al
                InstalledRamModules = SystemInformationsCollectorHelper.GetInstalledRamModules() // Kurulu RAM modüllerinin sayısını al
            };
        }
    }
}
