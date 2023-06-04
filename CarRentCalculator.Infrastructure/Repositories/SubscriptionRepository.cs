using CarRentCalculator.Infrastructure.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CarRentCalculator.Infrastructure.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        internal string FileName = "AllSubs.Json";
        private readonly ILogger _logger;


        public SubscriptionRepository(ILogger<SubscriptionRepository> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<Subscription>> GetSubscriptions()
        {
            using var reader = new StreamReader(Path.Combine(AppContext.BaseDirectory, FileName));
            var file =  await reader.ReadToEndAsync();
            var subscriptions = Enumerable.Empty<Subscription>();

            try
            {
                subscriptions = JsonSerializer.Deserialize<IEnumerable<Subscription>>(file);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not read subscriptions.");
                throw;
            }

            return subscriptions;

        }
    }
}
