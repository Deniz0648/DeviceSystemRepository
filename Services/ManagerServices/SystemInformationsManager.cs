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
            _cachedSystemInformations = new SystemInformationsModel
            {
                HostName = "",
                UserName = "",
                CpuModel = "",
                GpuModel = "",
                InstalledRamModules= 0,
                IPAddress = "",
                MACAddress = "",
                OSVersion = "",
                PCModel = "",
                PCSerialNumber = "",
                Status = true,
                TotalDisks = 0,
                TotalRam = 0,
                Disks = [],
                Monitors = [],
                Networks = []

            };
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
                try
                {
                    var json = File.ReadAllText(cacheFilePath);
                    _cachedSystemInformations = JsonSerializer.Deserialize<SystemInformationsModel>(json)!;
                }
                catch
                {
                    Console.WriteLine("Önbellek dosyası bozuk. Varsayılan bilgi atanıyor.");
                    _cachedSystemInformations = new SystemInformationsModel();
                }
            }
            else
            {
                _cachedSystemInformations = new SystemInformationsModel();
            }
        }


        private void SaveCachedSystemInformations()
        {
            string cacheFilePath = GetCacheFilePath();
            var json = JsonSerializer.Serialize(_cachedSystemInformations);
            Directory.CreateDirectory(Path.GetDirectoryName(cacheFilePath)!); // Ensure the directory exists
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
                //Console.WriteLine("Eski Sistem Bilgileri:");
                if (_cachedSystemInformations != null)
                {
                    LogSystemInformation("Eski Sistem Bilgileri:", _cachedSystemInformations, GetOptions());
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
                    await _clientService.PostAsync(_cachedSystemInformations, ClientService.GetOptions());

                    //// Gönderilen veriyi konsola yazdır
                    //LogSystemInformation("Gönderilen Sistem Bilgileri:", _cachedSystemInformations);

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

        private static bool AreSystemInformationsEqual(SystemInformationsModel? oldData, SystemInformationsModel? newData)
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
                   AreDiskInformationsEqual(oldData.Disks, newData.Disks) &&
                   AreMonitorInformationsEqual(oldData.Monitors, newData.Monitors);
        }

        private static bool AreNetworkInformationsEqual(List<NetworkInformationsModel> oldList, List<NetworkInformationsModel> newList)
        {
            // Eğer her iki liste de null ise, eşit kabul edebiliriz
            if (oldList == null && newList == null)
            {
                return true;
            }

            // Eğer biri null, diğeri null değilse, eşit değildir
            if (oldList == null || newList == null)
            {
                return false;
            }

            // Listelerin uzunlukları farklıysa eşit değildir
            if (oldList.Count != newList.Count)
            {
                return false;
            }

            // Elemanları karşılaştır
            for (int i = 0; i < oldList.Count; i++)
            {
                // Elemanlardan biri null ise, eşit değildir
                if (oldList[i] == null || newList[i] == null)
                {
                    return false;
                }

                // NetworkInformationsModel eşitlik kontrolü
                if (!oldList[i].Equals(newList[i]))
                {
                    return false;
                }
            }

            // Tüm kontroller geçti, listeler eşittir
            return true;
        }




        private static bool AreDiskInformationsEqual(List<DiskInformationsModel> oldList, List<DiskInformationsModel> newList)
        {
            // Null kontrolü
            if (oldList == null && newList == null)
            {
                return true;
            }

            if (oldList == null || newList == null)
            {
                return false;
            }

            if (oldList.Count != newList.Count)
            {
                return false;
            }

            for (int i = 0; i < oldList.Count; i++)
            {
                // DiskInformationsModel eşitlik kontrolü
                if (!((IEquatable<DiskInformationsModel>)oldList[i]).Equals(newList[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool AreMonitorInformationsEqual(List<MonitorInformationsModel> oldList, List<MonitorInformationsModel> newList)
        {
            // Null kontrolü
            if (oldList == null && newList == null)
            {
                return true;
            }

            if (oldList == null || newList == null || oldList.Count != newList.Count)
            {
                return false;
            }

            for (int i = 0; i < oldList.Count; i++)
            {
                // Null kontrolü ve eşitlik kontrolü
                if (oldList[i] == null || newList[i] == null)
                {
                    return false; // Null olan elemanlar için eşit değildir
                }

                // MonitorInformationsModel eşitlik kontrolü
                if (!oldList[i].Equals(newList[i]))
                {
                    return false; // Eşit değilse false döner
                }
            }

            return true; // Hiçbir eşitsizlik yoksa true döner
        }


        private static JsonSerializerOptions GetOptions()
        {
            return new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        private static void LogSystemInformation(string label, SystemInformationsModel systemInformations, JsonSerializerOptions options)
        {
            if (systemInformations == null)
            {
                Console.WriteLine($"{label} (Veri yok)");
                return;
            }

            var json = JsonSerializer.Serialize(systemInformations, options);

            Console.WriteLine($"{label}\n{json}");
        }
    }
}
