using RealTimeReporting.Domain.Models;
using System.Text.Json.Serialization;

namespace RealTimeReporting.Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<Order>? Orders { get; set; }
    }
}
