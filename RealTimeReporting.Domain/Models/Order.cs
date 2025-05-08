using RealTimeReporting.Domain.Entities;

namespace RealTimeReporting.Domain.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }

        // EF için navigation
        public Customer Customer { get; set; }
    }
}
