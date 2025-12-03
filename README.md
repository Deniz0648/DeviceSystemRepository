# System Activity Monitor Worker Service

Bu proje, Windows ortamında çalışan bir **.NET Core Worker Service** uygulamasıdır. Uygulama isteğe bağlı olarak **Windows Servisi** olarak da kurulabilir.
Görevi, çalıştığı makinenin çeşitli sistem bilgilerini toplayarak JSON formatında bir API'ye göndermek ve belirli aralıklarla **“heartbeat” (nabız)** üretip makinenin aktiflik durumunu bildirmektir.

## Özellikler

- Windows üzerinde arka planda çalışan Worker Service mimarisi  
- İsteğe göre Windows Service olarak kurulum  
- Sistem bilgilerinin toplanması (CPU, RAM, Disk, vb.)  
- Toplanan verilerin JSON formatında REST API’ye gönderilmesi  
- Uygulama çalıştığı sürece belirli aralıklarla aktiflik nabzı üretme  
- Loglama ve hata yönetimi

## Teknolojiler

- **Platform:** .NET Core Worker Service  
- **Dil:** C#  
- **Hedef Sistem:** Windows  
- **API Haberleşmesi:** HTTP/JSON  

## Kurulum

### 1. Çalıştırma (Debug)

```bash
dotnet run
