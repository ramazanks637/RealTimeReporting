using RealTimeReporting.Domain.Interfaces;
using System.Globalization;

namespace RealTimeReporting.Jobs
{
    public class MinutelyDailyTotalJob
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IRedisCacheService _redis;

        public MinutelyDailyTotalJob(IOrderRepository orderRepository, IRedisCacheService redis)
        
        {
            _orderRepository = orderRepository;
            _redis = redis;
        }

        public async Task Execute()
        {
            var today = DateTime.UtcNow.Date;
            var total = await _orderRepository.GetDailyTotalAsync(today);
            await _redis.SetValueAsync($"minutely_daily_total:{today:yyyy-MM-dd}", total.ToString(CultureInfo.InvariantCulture), TimeSpan.FromDays(1));
        }
    }
}
