using System;
using System.Collections.Generic;
using System.Linq;

namespace DeviceSystemRepository.Models
{
    internal class SystemInformationsModel
    {
        public string HostName { get; set; } = "";
        public string? UserName { get; set; } = "";
        public List<NetworkInformationsModel>? Networks { get; set; } = [];
        public string? PCModel { get; set; } = "";
        public string? PCSerialNumber { get; set; } = "";
        public List<MonitorInformationsModel>? Monitors { get; set; } = [];
        public string? IPAddress { get; set; } = "";
        public string? MACAddress { get; set; } = "";
        public string? OSVersion { get; set; } = "";
        public string? CpuModel { get; set; } = "";
        public string? GpuModel { get; set; } = "";
        public int InstalledRamModules { get; set; } = 0;
        public long TotalRam { get; set; } = 0;
        public int TotalDisks { get; set; } = 0;
        public List<DiskInformationsModel>? Disks { get; set; } = [];
        public bool Status { get; set; } = true;
    }
}
