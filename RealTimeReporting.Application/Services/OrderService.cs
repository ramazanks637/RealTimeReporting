using RealTimeReporting.Domain.Interfaces;
using RealTimeReporting.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeReporting.Application.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task AddOrderAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order)); // veya direkt return;

            await _orderRepository.AddAsync(order);
        }


        public async Task UpdateOrderAsync(Order order)
        {
            await _orderRepository.UpdateAsync(order);
        }

        public async Task DeleteOrderAsync(int id)
        {
            await _orderRepository.DeleteAsync(id);
        }

        public async Task<decimal> GetDailyTotalAsync(DateTime date)
        {
            return await _orderRepository.GetDailyTotalAsync(date);
        }

        public async Task<decimal> GetHourlyTotalAsync(DateTime dateTime)
        {
            return await _orderRepository.GetHourlyTotalAsync(dateTime);
        }
    }
}
