using Moq;
using RealTimeReporting.Application.Services;
using RealTimeReporting.Domain.Interfaces;
using RealTimeReporting.Domain.Entities;
using Xunit;

namespace RealTimeReporting.Tests
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _repoMock;
        private readonly CustomerService _service;

        public CustomerServiceTests()
        {
            _repoMock = new Mock<ICustomerRepository>();
            _service = new CustomerService(_repoMock.Object);
        }

        [Fact]
        public async Task AddAsync_ValidCustomer_CallsRepository()
        {
            var customer = new Customer { Name = "Test Müşteri" };

            await _service.AddAsync(customer);

            _repoMock.Verify(x => x.AddAsync(It.Is<Customer>(c => c.Name == "Test Müşteri")), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsCustomers()
        {
            var customers = new List<Customer> { new Customer { Name = "Ali" } };
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(customers);

            var result = await _service.GetAllAsync();

            Assert.Single(result);
            Assert.Equal("Ali", result.First().Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectCustomer()
        {
            var customer = new Customer { Id = 1, Name = "Ayşe" };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(customer);

            var result = await _service.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Ayşe", result.Name);
        }
    }
}
