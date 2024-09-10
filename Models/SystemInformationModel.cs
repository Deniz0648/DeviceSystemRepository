using System;
using System.Collections.Generic;
using System.Linq;

namespace DeviceSystemRepository.Models
{
    internal class SystemInformationsModel
    {
        public required string HostName { get; set; }
        public required string UserName { get; set; }
        public required List<NetworkInformationsModel> Networks { get; set; }
        public required string PCModel { get; set; }
        public required string PCSerialNumber { get; set; }
        public required string IPAddress { get; set; }
        public required string MACAddress { get; set; }
        public required string OSVersion { get; set; }
        public required string CpuModel { get; set; }
        public required string GpuModel { get; set; }
        public int InstalledRamModules { get; set; }
        public long TotalRam { get; set; }
        public int TotalDisks { get; set; }
        public required List<DiskInformationsModel> Disks { get; set; }
        public bool Status { get; set; } = true;
    }
}
