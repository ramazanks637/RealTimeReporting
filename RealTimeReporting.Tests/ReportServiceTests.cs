using Moq;
using RealTimeReporting.Application.Services;
using RealTimeReporting.Domain.Interfaces;
using Xunit;

namespace RealTimeReporting.Tests
{
    public class ReportServiceTests
    {
        [Fact]
        public async Task GetDailyReportAsync_ReturnsFromRedis_IfExists()
        {
            // Arrange
            var redisMock = new Mock<IRedisCacheService>();
            var repoMock = new Mock<IOrderRepository>();
            var date = DateTime.UtcNow.Date;

            redisMock.Setup(x => x.GetValueAsync(It.IsAny<string>()))
            .ReturnsAsync("500.25");

            var service = new ReportService(redisMock.Object, repoMock.Object);

            // Act
            var result = await service.GetDailyReportAsync(date);

            // Assert
            Assert.Equal(500.25m, result);
            repoMock.Verify(x => x.GetDailyTotalAsync(It.IsAny<DateTime>()), Times.Never);
        }
    }
}
