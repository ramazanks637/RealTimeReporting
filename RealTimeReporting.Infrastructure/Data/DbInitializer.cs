using RealTimeReporting.Domain.Entities;
using RealTimeReporting.Domain.Models;

namespace RealTimeReporting.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.Customers.Any())
            {
                var customer = new Customer { Name = "Ramazan Köse" };
                context.Customers.Add(customer);

                context.Orders.Add(new Order
                {
                    Amount = 2500,
                    CreatedAt = DateTime.UtcNow,
                    Customer = customer
                });

                context.SaveChanges();
            }
        }
    }
}
