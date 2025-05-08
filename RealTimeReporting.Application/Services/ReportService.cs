using RealTimeReporting.Domain.Interfaces;
using System.Globalization;

namespace RealTimeReporting.Application.Services
{
    public class ReportService
    {
        private readonly IRedisCacheService _redis;
        private readonly IOrderRepository _orderRepository;

        public ReportService(IRedisCacheService redis, IOrderRepository orderRepository)
        {
            _redis = redis;
            _orderRepository = orderRepository;
        }

        public async Task<decimal> GetDailyReportAsync(DateTime date)
        {
            var redisKey = $"daily_total:{date:yyyy-MM-dd}";
            var cached = await _redis.GetValueAsync(redisKey);

            if (!string.IsNullOrEmpty(cached))
            {
                if (decimal.TryParse(cached, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedValue))
                    return parsedValue;
            }

            var total = await _orderRepository.GetDailyTotalAsync(date);
            await _redis.SetValueAsync(redisKey, total.ToString(CultureInfo.InvariantCulture), TimeSpan.FromHours(2)); // cache'e yazarken de noktalı format

            return total;
        }

        public async Task<decimal> GetHourlyReportAsync(DateTime dateTime)
        {
            var redisKey = $"hourly_total:{dateTime:yyyy-MM-dd-HH}";
            var cached = await _redis.GetValueAsync(redisKey);

            if (!string.IsNullOrEmpty(cached) &&
                decimal.TryParse(cached, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed))
                return parsed;

            var total = await _orderRepository.GetHourlyTotalAsync(dateTime);
            await _redis.SetValueAsync(redisKey, total.ToString(CultureInfo.InvariantCulture), TimeSpan.FromHours(1));
            return total;
        }

        public async Task<decimal> GetMinutelyDailyReportAsync(DateTime date)
        {
            var redisKey = $"minutely_daily_total:{date:yyyy-MM-dd}";
            var cached = await _redis.GetValueAsync(redisKey);

            if (!string.IsNullOrEmpty(cached) &&
                decimal.TryParse(cached, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed))
                return parsed;

            var total = await _orderRepository.GetDailyTotalAsync(date);
            await _redis.SetValueAsync(redisKey, total.ToString(CultureInfo.InvariantCulture), TimeSpan.FromMinutes(1));
            return total;
        }

    }
}
