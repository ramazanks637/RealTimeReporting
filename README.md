# 📊 RealTimeReporting - Mikroservis Case Projesi

.NET 8 tabanlı mikroservis mimarisiyle geliştirilmiş gerçek zamanlı raporlama sistemi. PostgreSQL ve Redis kullanılarak veriler işlenmekte, Hangfire ile zamanlanmış görevler yönetilmekte, Swagger üzerinden API testleri yapılabilmektedir.

## 🚀 Teknolojiler
- ASP.NET Core 8 (Web API)
- Entity Framework Core (Code First)
- Dapper
- PostgreSQL
- Redis
- Hangfire
- Docker & Docker Compose
- xUnit & Moq
- Swagger

## 📂 Proje Katmanları
- `RealTimeReporting.API` → Ana API katmanı, Swagger ve Hangfire arayüzü içerir.
- `RealTimeReporting.Application` → Servis katmanı (business logic).
- `RealTimeReporting.Domain` → Arayüzler ve modeller.
- `RealTimeReporting.Infrastructure` → Dapper ve EF Core implementasyonları.
- `RealTimeReporting.Jobs` → Hangfire üzerinden tetiklenen görevler.
- `RealTimeReporting.Tests` → Unit test senaryoları.

## ⚙️ Kurulum (Docker ile)
```bash
docker-compose down
docker-compose up --build
```
Bu komutlar şu işlemleri otomatik yapar:
- PostgreSQL ve Redis container'ını başlatır
- EF Core ile veritabanını oluşturur
- `DbInitializer.Seed()` ile örnek verileri yükler
- Swagger ve Hangfire arayüzlerini aktif eder

## 🔁 Zamanlanmış Görevler (Hangfire Jobs)
- `DailyTotalJob`: Her gece 00:00'da çalışarak önceki günün toplam satış tutarını Redis'e yazar.
- `HourlySummaryJob`: Her saat başı çalışır ve saatlik toplamı Redis'e yazar.
- `MinutelyDailyTotalJob`: Her dakika çalışır ve o güné ait toplam tutarı günceller.

## 🧪 Unit Testler
xUnit ile yazılmış test senaryoları:
- OrdersController
- ReportService
- CustomerService


## 🧠 Ek Özellikler
- Ortam bazlı konfigürasyon (Development, Docker)
- EF Core Code-First Migration desteği
- Redis üzerinden cache kontrolü
- Swagger üzerinden endpoint testleri
- Gerçek zamanlı (manual) job tetiklemeleri için özel endpointler

## 📝 Kullanım Senaryoları
- `/api/reports/daily`: Dünkü toplam tutarı getirir
- `/api/reports/hourly`: O saate ait toplam
- `/api/reports/minutely-daily`: Güncel günlük toplam

## 🧹 Temizlik & Reset Komutları
```bash
docker-compose down -v ## -v parametresi kullanılırsa Databasedeki tüm kayıtlar ile beraber temizler  kullanılmadığında ise sadece container'i temizler.
# veya
dotnet ef database drop --project RealTimeReporting.Infrastructure

dotnet ef database update --project RealTimeReporting.Infrastructure
```

## 🚀 Başlangıç: Projeyi Sıfırdan Ayağa Kaldırmak
### 📍 Ön Gereksinimler
- .NET 8 SDK
- Docker
- Docker Compose
- Git

### 🧰️ 1. Projeyi Klonlayın
```bash
git clone https://github.com/kullaniciadi/RealTimeReporting.git
cd RealTimeReporting
```

### 🐳 2. Docker Üzerinden Uygulamayı Başlatmak
```bash
docker-compose up --build
```
> Bu işlem migration ve seeding işlemlerini otomatik gerçekleştirir. Elle migration eklemeye genellikle gerek yoktur.

### ✅ 3. Kontrol Paneli ve Test Arayüzleri
- Swagger UI: http://localhost:5000/swagger
- Hangfire Dashboard: http://localhost:5000/hangfire

## 📁 init.sh Dosyası (Linux/macOS için Otomasyon Scripti)
Projeyi uçtan uca başlatmak için şu scripti `RealTimeReporting/init.sh` dosyası olarak ekleyip çalıştırabilirsiniz:

```bash
#!/bin/bash

echo "🔧 EF Core aracı yükleniyor..."
dotnet tool install --global dotnet-ef

echo "🛆 Bağımlılıklar yükleniyor..."
dotnet restore

echo "🧱 Migration uygulanıyor..."
dotnet ef database update --project RealTimeReporting.Infrastructure --startup-project RealTimeReporting.API

echo "🐳 Docker ortamı ayağa kaldırılıyor..."
docker-compose up --build
```

> Çalıştırmak için:
```bash
chmod +x init.sh
./init.sh
```

## ✅ Tamamlanan Özellikler (Check List)
- [x] PostgreSQL veritabanı kullanımı
- [x] Redis önbellekleme mekanizması
- [x] Docker Compose ile ayağa kaldırılabilir yapı
- [x] Hangfire ile zamanlanmış job altyapısı
- [x] Orders ve Customers tabloları için CRUD işlemleri
- [x] Code-First EF Core altyapısı ve Seed Data
- [x] Swagger arayüzü üzerinden API testleri
- [x] Unit Test altyapısı ve örnek senaryolar
- [x] Uygulama içi environment bazlı konfigürasyon (Docker vs Local)
- [x] Redis üzerinden rapor verisi cacheleme
- [x] Dakikalık, saatlik ve günlük job senaryoları
- [x] Docker build sonrası otomatik migration ve seeding işlemi
- [x] `init.sh` ile tek komutla kurulum ve başlatma
