using CarRentCalculator.Application.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CarRentCalculator.Application.Dtos
{
    public class RentCalculationRequest
    {

        [JsonConverter(typeof(StringEnumConverter))]
        public SubscriptionType SubscriptionType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public CarType CarType { get; set; }

        /// <summary>
        /// Start time of rental.
        /// </summary>
        public DateTime From { get; set; }

        /// <summary>
        /// End time of rental.
        /// </summary>
        public DateTime To { get; set; }

        /// <summary>
        /// Estimated distance in Kilometers.
        /// </summary>
        public double EstimatedDistanceKm { get; set; }

    }
}
