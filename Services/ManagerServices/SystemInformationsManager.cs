using DeviceSystemRepository.Models;
using DeviceSystemRepository.Services.ApiServices;
using DeviceSystemRepository.Services.CollectorService;
using System.Text.Json;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

namespace DeviceSystemRepository.Services.ManagerServices
{
    public class SystemInformationsManager
    {
        private readonly ClientService _clientService;
        private SystemInformationsModel _cachedSystemInformations;

        public SystemInformationsManager(ClientService clientService)
        {
            _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
            LoadCachedSystemInformations();
        }

        private static string GetCacheFilePath()
        {
            string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(userProfile, "DeviceSystemRepository", "cachedSystemInformations.json");
        }

        private void LoadCachedSystemInformations()
        {
            string cacheFilePath = GetCacheFilePath();
            if (File.Exists(cacheFilePath))
            {
                var json = File.ReadAllText(cacheFilePath);
                _cachedSystemInformations = JsonSerializer.Deserialize<SystemInformationsModel>(json);
            }
        }

        private void SaveCachedSystemInformations()
        {
            string cacheFilePath = GetCacheFilePath();
            var json = JsonSerializer.Serialize(_cachedSystemInformations);
            Directory.CreateDirectory(Path.GetDirectoryName(cacheFilePath)); // Ensure the directory exists
            File.WriteAllText(cacheFilePath, json);
        }

        public async Task ManageSystemInformationsAsync()
        {
            try
            {
                Console.WriteLine("Sistem bilgileri toplanıyor...");

                // Sistem bilgilerini topla
                var currentSystemInformations = SystemInformationsCollector.GetSystemInformations();

                // Bellekteki eski veriyi kontrol et
                Console.WriteLine("Eski Sistem Bilgileri:");
                if (_cachedSystemInformations != null)
                {
                    LogSystemInformation("Eski Sistem Bilgileri:", _cachedSystemInformations);
                }
                else
                {
                    Console.WriteLine("(Veri yok)");
                }

                if (_cachedSystemInformations == null || !AreSystemInformationsEqual(_cachedSystemInformations, currentSystemInformations))
                {
                    Console.WriteLine("Sistem bilgileri güncellenmiş veya ilk kez toplanıyor.");

                    // Bellekteki veriyi güncelle
                    _cachedSystemInformations = currentSystemInformations;

                    // Veriyi POST et
                    await _clientService.PostAsync(_cachedSystemInformations);

                    // Veriyi sakla
                    SaveCachedSystemInformations();

                    Console.WriteLine("Sistem bilgileri başarıyla gönderildi.");
                }
                else
                {
                    Console.WriteLine("Sistem bilgileri değişmedi. Hiçbir işlem yapılmadı.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bir hata oluştu: {ex.Message}");
            }
        }

        private static bool AreSystemInformationsEqual(SystemInformationsModel oldData, SystemInformationsModel newData)
        {
            if (oldData == null || newData == null)
                return false;

            return oldData.HostName == newData.HostName &&
                   oldData.UserName == newData.UserName &&
                   oldData.PCModel == newData.PCModel &&
                   oldData.PCSerialNumber == newData.PCSerialNumber &&
                   oldData.IPAddress == newData.IPAddress &&
                   oldData.MACAddress == newData.MACAddress &&
                   oldData.OSVersion == newData.OSVersion &&
                   oldData.CpuModel == newData.CpuModel &&
                   oldData.GpuModel == newData.GpuModel &&
                   oldData.InstalledRamModules == newData.InstalledRamModules &&
                   oldData.TotalRam == newData.TotalRam &&
                   oldData.TotalDisks == newData.TotalDisks &&
                   oldData.Status == newData.Status &&
                   AreNetworkInformationsEqual(oldData.Networks, newData.Networks) &&
                   AreDiskInformationsEqual(oldData.Disks, newData.Disks);
        }

        private static bool AreNetworkInformationsEqual(List<NetworkInformationsModel> oldList, List<NetworkInformationsModel> newList)
        {
            if (oldList.Count != newList.Count)
                return false;

            for (int i = 0; i < oldList.Count; i++)
            {
                if (!oldList[i].Equals(newList[i]))
                    return false;
            }

            return true;
        }

        private static bool AreDiskInformationsEqual(List<DiskInformationsModel> oldList, List<DiskInformationsModel> newList)
        {
            if (oldList.Count != newList.Count)
                return false;

            for (int i = 0; i < oldList.Count; i++)
            {
                if (!oldList[i].Equals(newList[i]))
                    return false;
            }

            return true;
        }

        private static void LogSystemInformation(string label, SystemInformationsModel systemInformations)
        {
            if (systemInformations == null)
            {
                Console.WriteLine($"{label} (Veri yok)");
                return;
            }

            var json = JsonSerializer.Serialize(systemInformations, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            Console.WriteLine($"{label}\n{json}");
        }
    }
}
