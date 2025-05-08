using RealTimeReporting.Domain.Interfaces;
using System.Globalization;

namespace RealTimeReporting.Jobs
{
    public class HourlySummaryJob
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IRedisCacheService _redis;

        public HourlySummaryJob(IOrderRepository orderRepository, IRedisCacheService redis)
        {
            _orderRepository = orderRepository;
            _redis = redis;
        }

        public async Task Execute()
        {
            var hour = DateTime.UtcNow.Date.AddHours(DateTime.UtcNow.Hour);// o anki saat
            var total = await _orderRepository.GetHourlyTotalAsync(hour);
            await _redis.SetValueAsync($"hourly_total:{hour:yyyy-MM-dd-HH}", total.ToString(CultureInfo.InvariantCulture), TimeSpan.FromDays(1)); // veriyi rediste 1 gün tutar sonra siler.
        }
    }
}
