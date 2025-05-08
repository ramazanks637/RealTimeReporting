using Hangfire;
using Microsoft.AspNetCore.Mvc;
using RealTimeReporting.Application.Services;
using RealTimeReporting.Jobs;

namespace RealTimeReporting.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly ReportService _reportService;

        public ReportsController(ReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("daily")]
        public async Task<IActionResult> GetDailyReport()
        {
            // Job'ı anlık tetikle
            BackgroundJob.Enqueue<DailyTotalJob>(job => job.Execute());

            var date = DateTime.UtcNow.Date.AddDays(-1);
            var result = await _reportService.GetDailyReportAsync(date);
            return Ok(result);
        }

        [HttpGet("hourly")]
        public async Task<IActionResult> GetHourlyReport([FromQuery] DateTime? date)
        {
            BackgroundJob.Enqueue<HourlySummaryJob>(job => job.Execute());

            var target = date ?? DateTime.UtcNow.Date.AddHours(DateTime.UtcNow.Hour);
            var result = await _reportService.GetHourlyReportAsync(target);
            return Ok(result);
        }

        [HttpGet("minutely-daily")]
        public async Task<IActionResult> GetMinutelyDailyReport([FromQuery] DateTime? date)
        {
            BackgroundJob.Enqueue<MinutelyDailyTotalJob>(job => job.Execute());

            var target = date ?? DateTime.Today;
            var result = await _reportService.GetMinutelyDailyReportAsync(target);
            return Ok(result);
        }
    }
}
