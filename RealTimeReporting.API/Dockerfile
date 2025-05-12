FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["RealTimeReporting.API/RealTimeReporting.API.csproj", "RealTimeReporting.API/"]
COPY ["RealTimeReporting.Application/RealTimeReporting.Application.csproj", "RealTimeReporting.Application/"]
COPY ["RealTimeReporting.Domain/RealTimeReporting.Domain.csproj", "RealTimeReporting.Domain/"]
COPY ["RealTimeReporting.Infrastructure/RealTimeReporting.Infrastructure.csproj", "RealTimeReporting.Infrastructure/"]
COPY ["RealTimeReporting.Jobs/RealTimeReporting.Jobs.csproj", "RealTimeReporting.Jobs/"]

RUN dotnet restore "RealTimeReporting.API/RealTimeReporting.API.csproj"

COPY . .
WORKDIR "/src/RealTimeReporting.API"
RUN dotnet publish "RealTimeReporting.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

# wait-for-postgres.sh scriptini kopyala ve çalıştırılabilir yap
COPY RealTimeReporting.API/wait-for-postgres.sh ./wait-for-postgres.sh
RUN chmod +x ./wait-for-postgres.sh

COPY --from=build /app/publish .

ENTRYPOINT ["./wait-for-postgres.sh", "dotnet", "RealTimeReporting.API.dll"]
