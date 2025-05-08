using Microsoft.EntityFrameworkCore;
using RealTimeReporting.Domain.Entities;
using RealTimeReporting.Domain.Models;
using System.Collections.Generic;

namespace RealTimeReporting.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
    }
}
