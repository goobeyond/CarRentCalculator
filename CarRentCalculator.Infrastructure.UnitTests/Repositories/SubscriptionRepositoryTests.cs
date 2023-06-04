using CarRentCalculator.Infrastructure.Repositories;
using Moq.AutoMock;
using System.Text.Json;

namespace CarRentCalculator.Infrastructure.UnitTests.Repositories
{
    public class SubscriptionRepositoryTests
    {
        private readonly AutoMocker mocker = new();

        [Fact]
        public async Task GetSubscriptions_ValidData_SubscriptionIsReturned()
        {
            var repo = mocker.CreateInstance<SubscriptionRepository>();

            repo.FileName = "AllSubs - Valid.json";

            var result = await repo.GetSubscriptions();

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetSubscriptions_InvalidData_ThrowsException()
        {
            var repo = mocker.CreateInstance<SubscriptionRepository>();

            repo.FileName = "AllSubs - Broken.json";

            var exception = await Assert.ThrowsAsync<JsonException>(async () => await repo.GetSubscriptions());

            Assert.StartsWith("The JSON value could not be converted", exception.Message);
        }
    }
}