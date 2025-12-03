# System Activity Monitor Worker Service

Bu proje, Windows ortamında çalışan bir **.NET Core Worker Service** uygulamasıdır. Uygulama isteğe bağlı olarak **Windows Servisi** olarak da kurulabilir.
Görevi, çalıştığı makinenin çeşitli sistem bilgilerini toplayarak JSON formatında bir API'ye göndermek ve belirli aralıklarla **“heartbeat” (nabız)** üretip makinenin aktiflik durumunu bildirmektir.

## Özellikler

- Windows üzerinde arka planda çalışan Worker Service mimarisi  

- Sistem bilgilerinin toplanması (CPU, RAM, Disk, vb.)  
- Toplanan verilerin JSON formatında REST API’ye gönderilmesi  
- Uygulama çalıştığı sürece belirli aralıklarla aktiflik nabzı üretme  
- Loglama ve hata yönetimi

- - Windows üzerinde arka planda çalışan Worker Service mimarisi  
- Setup Project ile kurulum ve registry üzerinden otomatik çalışma
- İsteğe göre Windows Service olarak kurulum  
- Konsolsuz (sessiz) çalışma modu  
- Sistem bilgilerini JSON formatında toplama  
- İlk çalışmada API ile iletişim kurma  
- Sonraki çalışmalarda veri değişikliği yoksa API çağrısını atlama  
- Heartbeat mekanizması ile aktiflik bildirimi  
- SSL doğrulama bypass (test ortamı için)  
- Yerel JSON cache dosyası kullanımı  
- HttpClient tabanlı haberleşme  
- `SystemInformationsManager` ile ayrıştırılmış iş mantığı  

## Veri İşleme Mantığı

Uygulamanın veri işleme akışı aşağıdaki gibidir:

1. Sistem bilgileri toplanır.  
2. Toplanan veri `%AppData%/SystemActivityMonitor/data.json` dosyasına kaydedilir.  
3. Eğer uygulama ilk kez çalışıyorsa:
   - Veriler API'ye gönderilir.
4. Eğer uygulama tekrar çalışıyorsa:
   - Yeni toplanan veriler önceki kaydedilmiş JSON dosyası ile karşılaştırılır.
   - **Değişiklik yoksa API çağrısı yapılmaz.**
   - **Fark varsa API'ye güncel veri gönderilir ve JSON dosyası güncellenir.**

Bu mekanizma gereksiz API yükünü azaltır ve performansı artırır.

## Teknolojiler

- **Platform:** .NET Core Worker Service  
- **Dil:** C#  
- **Hedef Sistem:** Windows  
- **API Haberleşmesi:** HTTP/JSON  

## Kurulum

### 1. Çalıştırma (Debug)

```bash
dotnet run
