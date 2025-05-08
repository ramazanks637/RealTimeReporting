using RealTimeReporting.Application.Services;
using RealTimeReporting.Domain.Interfaces;
using System.Globalization;

namespace RealTimeReporting.Jobs
{
    public class DailyTotalJob
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IRedisCacheService _redis;

        public DailyTotalJob(IOrderRepository orderRepository, IRedisCacheService redis)
        {
            _orderRepository = orderRepository;
            _redis = redis;
        }

        public async Task Execute()
        {
            var date = DateTime.UtcNow.Date.AddDays(-1); // dünkü veri
            var total = await _orderRepository.GetDailyTotalAsync(date);
            await _redis.SetValueAsync($"daily_total:{date:yyyy-MM-dd}", total.ToString(CultureInfo.InvariantCulture), TimeSpan.FromDays(1));
        }
    }
}
