using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using RealTimeReporting.Domain.Interfaces;
using RealTimeReporting.Domain.Models;
using Dapper;


namespace RealTimeReporting.Infrastructure.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            const string sql = "SELECT * FROM \"Orders\"";
            using var connection = CreateConnection();
            return await connection.QueryAsync<Order>(sql);
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            const string sql = "SELECT * FROM \"Orders\" WHERE \"Id\" = @Id";
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Order>(sql, new { Id = id });
        }

        public async Task AddAsync(Order order)
        {
            const string sql = @"INSERT INTO ""Orders"" (""CustomerId"", ""Amount"", ""CreatedAt"") 
                             VALUES (@CustomerId, @Amount, @CreatedAt)";
            using var connection = CreateConnection();
            await connection.ExecuteAsync(sql, order);
        }

        public async Task UpdateAsync(Order order)
        {
            const string sql = @"UPDATE ""Orders"" 
                             SET ""CustomerId"" = @CustomerId, ""Amount"" = @Amount, ""CreatedAt"" = @CreatedAt 
                             WHERE ""Id"" = @Id";
            using var connection = CreateConnection();
            await connection.ExecuteAsync(sql, order);
        }

        public async Task DeleteAsync(int id)
        {
            const string sql = "DELETE FROM \"Orders\" WHERE \"Id\" = @Id";
            using var connection = CreateConnection();
            await connection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<decimal> GetDailyTotalAsync(DateTime date)
        {
            const string sql = @"SELECT COALESCE(SUM(""Amount""), 0) 
                             FROM ""Orders"" 
                             WHERE DATE(""CreatedAt"") = @Date";
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<decimal>(sql, new { Date = date.Date });
        }

        public async Task<decimal> GetHourlyTotalAsync(DateTime dateTime)
        {
            const string sql = @"SELECT COALESCE(SUM(""Amount""), 0) 
                             FROM ""Orders"" 
                             WHERE DATE_TRUNC('hour', ""CreatedAt"") = @Hour";
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<decimal>(sql, new { Hour = dateTime });
        }
    }

}
