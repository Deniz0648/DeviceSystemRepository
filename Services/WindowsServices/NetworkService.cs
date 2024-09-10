using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DeviceSystemRepository.Models;

namespace DeviceSystemRepository.Services.WindowsServices
{
    internal class NetworkService
    {
        public static List<NetworkInformationsModel> GetNetworkInformations()
        {
            string hostName = Dns.GetHostName(); // Bilgisayarın adını al

            return NetworkInterface.GetAllNetworkInterfaces()
                .Where(nic => nic.OperationalStatus == OperationalStatus.Up) // Yalnızca aktif ağ adaptörlerini al
                .Select(nic => new NetworkInformationsModel
                {
                    HostName = hostName,
                    MacAdress = BitConverter.ToString(nic.GetPhysicalAddress().GetAddressBytes()).Replace("-", ":"), // MAC Adresini al
                    IPAdress = nic.GetIPProperties().UnicastAddresses
                                .FirstOrDefault(ip => ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?
                                .Address.ToString() ?? "N/A", // İlk IPv4 adresini al veya "N/A" döndür
                    ConnectionType = GetConnectionType(nic) // Bağlantı türünü al
                })
                .Where(info => info.IPAdress != "N/A") // IP adresi olmayanları filtrele
                .ToList();
        }

        private static string GetConnectionType(NetworkInterface nic)
        {
            return nic.NetworkInterfaceType switch
            {
                NetworkInterfaceType.Wireless80211 => "WiFi",
                NetworkInterfaceType.Ethernet => "LAN",
                NetworkInterfaceType.Loopback => "Local",
                _ => "Bilinmiyor",
            };
        }
    }
}
