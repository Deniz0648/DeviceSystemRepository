using DeviceSystemRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;

namespace DeviceSystemRepository.Services.WindowsServices
{
    internal class MonitorService
    {
        public static List<MonitorInformationsModel> GetMonitorInformations()
        {
            string hostName = Dns.GetHostName(); // Bilgisayarın adını al
            var searcher = new ManagementObjectSearcher(@"root\wmi", "SELECT * FROM WmiMonitorID");
            try
            {
                return searcher.Get()
                    .Cast<ManagementObject>()
                    .Select(mo => new MonitorInformationsModel
                    {
                        HostName = hostName,
                        ManufacturerName = mo["ManufacturerName"] is ushort[] manufacturerName
                            ? GetStringFromWmiArray(manufacturerName)
                            : "N/A",
                        SerialNumberID = mo["SerialNumberID"] is ushort[] serialNumberId
                            ? GetStringFromWmiArray(serialNumberId)
                            : "N/A",
                        UserFriendlyName = mo["UserFriendlyName"] is ushort[] userFriendlyName
                            ? GetStringFromWmiArray(userFriendlyName)
                            : "N/A"
                    })
                    .Where(info => !string.IsNullOrEmpty(info.SerialNumberID))
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving monitor information: {ex.Message}");
                return []; // Hata durumunda boş liste döndür
            }
        }

        private static string GetStringFromWmiArray(ushort[]? data)
        {
            if (data is null || data.Length == 0) return "N/A";
            return string.Join("", data.TakeWhile(c => c != 0).Select(c => Convert.ToChar(c)));
        }
    }
}


