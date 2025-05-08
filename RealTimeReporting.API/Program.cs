using Hangfire;
using Hangfire.PostgreSql;
using RealTimeReporting.Application.Services;
using RealTimeReporting.Domain.Interfaces;
using RealTimeReporting.Infrastructure.Cache;
using RealTimeReporting.Infrastructure.Data;
using RealTimeReporting.Jobs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// PostgreSQL ve Redis ba�lant�lar�
var connectionString = builder.Configuration.GetConnectionString("PostgreSql");
var redisConn = builder.Configuration.GetValue<string>("Redis:Connection");

if (string.IsNullOrEmpty(redisConn))
    throw new Exception("Redis ba�lant� dizesi bo� geldi. L�tfen appsettings.*.json dosyan�z� kontrol edin.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Uygulaman�n 80 portundan dinlemesi i�in
builder.WebHost.UseUrls("http://*:80");

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Hangfire (PostgreSQL ile)
builder.Services.AddHangfire(config =>
    config.UsePostgreSqlStorage(connectionString));
builder.Services.AddHangfireServer();

// IOC kay�tlar�
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

// Hangfire Dashboard burada olmal� (MapControllers'tan �nce)
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new AllowAllDashboardAuthorization() }
});

app.MapControllers();

// Zamanlanm�� Job'lar
RecurringJob.AddOrUpdate<DailyTotalJob>(
    "daily-total-job",
    job => job.Execute(),
    "0 0 * * *" // her g�n saat 00:00'da
);

RecurringJob.AddOrUpdate<HourlySummaryJob>(
    "hourly-total-job",
    job => job.Execute(),
    "0 * * * *" // her saat ba��
);

RecurringJob.AddOrUpdate<MinutelyDailyTotalJob>(
    "minutely-daily-total-job",
    job => job.Execute(),
    "* * * * *" // Her 5 dakika
);


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); // Migration'lar� uygula
    DbInitializer.Seed(db); // �rnek veriyi ekle
}
 

app.Run();
