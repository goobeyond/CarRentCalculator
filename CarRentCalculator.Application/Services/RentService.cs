using CarRentCalculator.Application.Dtos;
using CarRentCalculator.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace CarRentCalculator.Application.Services
{
    public class RentService : IRentService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ILogger<RentService> _logger;

        public RentService(ISubscriptionRepository repo, ILogger<RentService> logger)
        {
            _subscriptionRepository = repo;
            _logger = logger;
        }

        public async Task<Result<decimal>> GetEstimatedCost(RentCalculationRequest request)
        {
            try
            {
                ValidateRequest(request);

                var subs = await _subscriptionRepository.GetSubscriptions();

                var sub = subs.FirstOrDefault(x => x.SubscriptionType == request.SubscriptionType.ToString());

                if (sub == null)
                {
                    return new Result<decimal>(ResultType.Failure, "Invalid subscription type.");
                }

                var rate = sub?.Rates.FirstOrDefault(x => x.CarType == request.CarType.ToString());

                if (rate == null)
                {
                    return new Result<decimal>(ResultType.Failure, "Invalid car type.");
                }

                var duration = (request.To - request.From).TotalHours;

                var cost = (rate.PerHour * (decimal)duration) + (rate.PerKm * (decimal)request.EstimatedDistanceKm);

                return new Result<decimal>(cost);

            }
            catch (Exception e)
            {
                return new Result<decimal>(ResultType.Failure, e.Message);
            }
        }

        private void ValidateRequest(RentCalculationRequest request)
        {
            if (request == null)
            {
                _logger.LogError("Request cannot be empty.");
                throw new Exception("Request cannot be empty.");
            }

            if(request.EstimatedDistanceKm < 0)
            {
                _logger.LogError("Distance cannot be negative.");
                throw new Exception("Distance cannot be negative.");
            }

            if(request.From == DateTime.MinValue || request.From == DateTime.MaxValue || request.To == DateTime.MinValue || request.To == DateTime.MaxValue) 
            {
                _logger.LogError("Invalid From or To date.");
                throw new Exception("Invalid From or To date.");
            }

            if(request.From > request.To) 
            {
                _logger.LogError("From date cannot be after To date.");
                throw new Exception("From date cannot be after To date.");
            }

            if (request.From == request.To)
            {
                _logger.LogError("From date cannot be same as To date.");
                throw new Exception("From date cannot be same as To date.");
            }
        }
    }
}