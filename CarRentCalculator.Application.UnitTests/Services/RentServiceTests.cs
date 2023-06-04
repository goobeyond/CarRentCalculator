using CarRentCalculator.Application.Dtos;
using CarRentCalculator.Application.Models;
using CarRentCalculator.Application.Services;
using CarRentCalculator.Infrastructure.Models;
using CarRentCalculator.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.AutoMock;

namespace CarRentCalculator.Application.UnitTests.Services
{
    public class RentServiceTests
    {
        private readonly AutoMocker mocker = new();

        [Fact]
        public async Task GetEstimatedCost_RequestIsEmtpy_ResultIsFailure()
        {
            var service = mocker.CreateInstance<RentService>();

            var result = await service.GetEstimatedCost(null);

            Assert.Equal(ResultType.Failure, result.ResultType);
            AssertLog(mocker.GetMock<ILogger<RentService>>(), LogLevel.Error, "Request cannot be empty.");
        }

        [Fact]
        public async Task GetEstimatedCost_NegativeDistance_ResultIsFailure()
        {
            var service = mocker.CreateInstance<RentService>();

            var request = new RentCalculationRequest()
            {
                EstimatedDistanceKm = -1
            };

            var result = await service.GetEstimatedCost(request);

            Assert.Equal(ResultType.Failure, result.ResultType);
            AssertLog(mocker.GetMock<ILogger<RentService>>(), LogLevel.Error, "Distance cannot be negative.");
        }

        [Fact]
        public async Task GetEstimatedCost_InvalidDates_ResultIsFailure()
        {
            var service = mocker.CreateInstance<RentService>();

            var request = new RentCalculationRequest()
            {
                From = DateTime.MinValue
            };

            var result = await service.GetEstimatedCost(request);

            Assert.Equal(ResultType.Failure, result.ResultType);
            AssertLog(mocker.GetMock<ILogger<RentService>>(), LogLevel.Error, "Invalid From or To date.");
        }

        [Fact]
        public async Task GetEstimatedCost_FromAndToAreReversed_ResultIsFailure()
        {
            var service = mocker.CreateInstance<RentService>();

            var request = new RentCalculationRequest()
            {
                From = DateTime.UtcNow.AddDays(1),
                To = DateTime.UtcNow.AddDays(-1)
            };

            var result = await service.GetEstimatedCost(request);

            Assert.Equal(ResultType.Failure, result.ResultType);
            AssertLog(mocker.GetMock<ILogger<RentService>>(), LogLevel.Error, "From date cannot be after To date.");
        }

        [Fact]
        public async Task GetEstimatedCost_FromAndToAreEqual_ResultIsFailure()
        {
            var service = mocker.CreateInstance<RentService>();
            var time = DateTime.UtcNow;
            var request = new RentCalculationRequest()
            {
                From = time,
                To = time
            };

            var result = await service.GetEstimatedCost(request);

            Assert.Equal(ResultType.Failure, result.ResultType);
            AssertLog(mocker.GetMock<ILogger<RentService>>(), LogLevel.Error, "From date cannot be same as To date.");
        }

        [Fact]
        public async Task GetEstimatedCost_InvalidSubscription_ResultIsFailure()
        {
            var service = mocker.CreateInstance<RentService>();
            mocker.GetMock<ISubscriptionRepository>().Setup(x => x.GetSubscriptions())
                .ReturnsAsync(new List<Subscription>()
                {
                    GetSubscription("SomeSubscription", "Electric")
                });

            var request = GetRequest();

            var result = await service.GetEstimatedCost(request);

            Assert.Equal(ResultType.Failure, result.ResultType);
        }

        [Fact]
        public async Task GetEstimatedCost_InvalidCarType_ResultIsFailure()
        {
            var service = mocker.CreateInstance<RentService>();
            mocker.GetMock<ISubscriptionRepository>().Setup(x => x.GetSubscriptions())
                .ReturnsAsync(new List<Subscription>()
                {
                    GetSubscription("Occasional", "Electric")
                });

            var request = GetRequest();

            var result = await service.GetEstimatedCost(request);

            Assert.Equal(ResultType.Failure, result.ResultType);
        }

        /// <summary>
        /// We asume the cost per KM and cost per Hour is 1 to make calculation easier
        /// </summary>
        [Theory]
        [MemberData(nameof(TestScenarios))]
        public async Task GetEstimatedCost_ValidRequest_CalculatesCost(DateTime from, DateTime to, double estimatedKm, decimal expectedCost)
        {
            var service = mocker.CreateInstance<RentService>();
            mocker.GetMock<ISubscriptionRepository>().Setup(x => x.GetSubscriptions())
                .ReturnsAsync(new List<Subscription>()
                {
                    GetSubscription("Occasional", "Electric")
                });

            var request = new RentCalculationRequest()
            {
                SubscriptionType = Models.SubscriptionType.Occasional,
                CarType = Models.CarType.Electric,
                EstimatedDistanceKm = estimatedKm,
                From = from,
                To = to,
            };

            var result = await service.GetEstimatedCost(request);

            Assert.Equal(ResultType.Success, result.ResultType);
            Assert.Equal(expectedCost, result.Data);
        }
        public static IEnumerable<object[]> TestScenarios =>
        new List<object[]>
        {
            new object[] { new DateTime(2023, 1, 1), new DateTime(2023, 1, 2), 10, 34 },
            new object[] { new DateTime(2023, 1, 1, 12, 0, 0), new DateTime(2023, 1, 1, 13, 0, 0), 5, 6 },
            new object[] { new DateTime(2023, 1, 1, 12, 0, 0), new DateTime(2023, 1, 1, 18, 0, 0), 100, 106 },
        };


        private Subscription GetSubscription(string subscriptionName, string carType)
        {
            return new Subscription()
            {
                SubscriptionType = subscriptionName,
                Rates = new List<Rate>
                        {
                            new Rate()
                            {
                                CarType = carType,
                                PerHour = 1,
                                PerKm = 1,
                            }
                        }
            };
        }

        private RentCalculationRequest GetRequest()
        {
            return new RentCalculationRequest()
            {
                From = DateTime.UtcNow,
                To = DateTime.UtcNow.AddDays(1),
                CarType = CarType.Small,
                SubscriptionType = SubscriptionType.Occasional
        };
        }

        private void AssertLog<T>(Mock<ILogger<T>> mockLogger, LogLevel level, string message)
        {
            mockLogger.Verify(m => m.Log(
                level,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(message)),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)
                ), Times.Once);
        }
    }
}