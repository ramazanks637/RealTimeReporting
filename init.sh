#!/bin/bash

echo "🔧 EF Core aracı yükleniyor..."
dotnet tool install --global dotnet-ef

echo "🛆 Bağımlılıklar yükleniyor..."
dotnet restore

echo "🧱 Migration uygulanıyor..."
dotnet ef database update --project RealTimeReporting.Infrastructure --startup-project RealTimeReporting.API

echo "🐳 Docker ortamı ayağa kaldırılıyor..."
docker-compose up --build