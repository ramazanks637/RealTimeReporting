using Hangfire;
using Hangfire.PostgreSql;
using RealTimeReporting.Application.Services;
using RealTimeReporting.Domain.Interfaces;
using RealTimeReporting.Infrastructure.Cache;
using RealTimeReporting.Infrastructure.Data;
using RealTimeReporting.Jobs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// PostgreSQL ve Redis baðlantýlarý
var connectionString = builder.Configuration.GetConnectionString("PostgreSql");
var redisConn = builder.Configuration.GetValue<string>("Redis:Connection");

if (string.IsNullOrEmpty(redisConn))
    throw new Exception("Redis baðlantý dizesi boþ geldi. Lütfen appsettings.*.json dosyanýzý kontrol edin.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Uygulamanýn 80 portundan dinlemesi için
builder.WebHost.UseUrls("http://*:80");

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Hangfire (PostgreSQL ile)
builder.Services.AddHangfire(config =>
    config.UsePostgreSqlStorage(connectionString));
builder.Services.AddHangfireServer();

// IOC kayýtlarý
builder.Services.AddScoped<IOrderRepository>(_ => new OrderRepository(connectionString));
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

//builder.Services.AddScoped<IRedisCacheService>(_ => new RedisCacheService(redisConn));
builder.Services.AddScoped<IRedisCacheService>(_ => new RedisCacheService(redisConn!));

builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<DailyTotalJob>();
builder.Services.AddScoped<HourlySummaryJob>();
builder.Services.AddScoped<MinutelyDailyTotalJob>();


builder.Services.AddControllers();

var app = builder.Build();

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

// Yetkilendirme middleware
app.UseAuthorization();

// Hangfire Dashboard burada olmalý (MapControllers'tan önce)
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new AllowAllDashboardAuthorization() }
});

app.MapControllers();

// Zamanlanmýþ Job'lar
RecurringJob.AddOrUpdate<DailyTotalJob>(
    "daily-total-job",
    job => job.Execute(),
    "0 0 * * *" // her gün saat 00:00'da
);

RecurringJob.AddOrUpdate<HourlySummaryJob>(
    "hourly-total-job",
    job => job.Execute(),
    "0 * * * *" // her saat baþý
);

RecurringJob.AddOrUpdate<MinutelyDailyTotalJob>(
    "minutely-daily-total-job",
    job => job.Execute(),
    "* * * * *" // Her 5 dakika
);


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); // Migration'larý uygula
    DbInitializer.Seed(db); // Örnek veriyi ekle
}
 

app.Run();
