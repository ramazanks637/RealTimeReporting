using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RealTimeReporting.API.Controllers;
using RealTimeReporting.Application.Services;
using RealTimeReporting.Domain.Interfaces;
using RealTimeReporting.Domain.Models;
using Xunit;

namespace RealTimeReporting.Tests
{
    public class OrdersControllerTests
    {
        private readonly OrdersController _controller;
        private readonly Mock<IOrderRepository> _repoMock;

        public OrdersControllerTests()
        {
            _repoMock = new Mock<IOrderRepository>();
            var orderService = new OrderService(_repoMock.Object);
            _controller = new OrdersController(orderService);
        }

        [Fact]
        public async Task Get_ReturnsAllOrders()
        {
            var orders = new List<Order>
            {
                new Order { Id = 1, Amount = 100, CustomerId = 1, CreatedAt = DateTime.UtcNow },
                new Order { Id = 2, Amount = 200, CustomerId = 2, CreatedAt = DateTime.UtcNow }
            };

            _repoMock.Setup(x => x.GetAllAsync()).ReturnsAsync(orders);

            var result = await _controller.Get() as OkObjectResult;

            Assert.NotNull(result);
            var returned = Assert.IsAssignableFrom<IEnumerable<Order>>(result.Value);
            Assert.Equal(2, returned.Count());
        }

        [Fact]
        public async Task GetById_ExistingId_ReturnsOrder()
        {
            var order = new Order { Id = 1, Amount = 150, CustomerId = 1, CreatedAt = DateTime.UtcNow };
            _repoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(order);

            var result = await _controller.GetById(1) as OkObjectResult;

            Assert.NotNull(result);
            Assert.IsType<Order>(result.Value);
        }

        [Fact]
        public async Task GetById_NonExistingId_ReturnsNotFound()
        {
            _repoMock.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((Order)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Post_NullOrder_ReturnsBadRequest()
        {
            var result = await _controller.Post(null);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Delete_ValidId_ReturnsNoContent()
        {
            var result = await _controller.Delete(1);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task GetById_ValidId_ReturnsOrder()
        {
            // Arrange
            var order = new Order { Id = 1, Amount = 100, CustomerId = 1, CreatedAt = DateTime.UtcNow };
            _repoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(order);

            // Act
            var result = await _controller.GetById(1) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var returned = Assert.IsType<Order>(result.Value);
            Assert.Equal(order.Id, returned.Id);
        }


        [Fact]
        public async Task GetById_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _repoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Order)null);

            // Act
            var result = await _controller.GetById(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }



    }
}
