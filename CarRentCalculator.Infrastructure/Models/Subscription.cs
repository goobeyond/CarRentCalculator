using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentCalculator.Infrastructure.Models
{
    public class Subscription
    {
        public string SubscriptionType { get; set; }
        public IEnumerable<Rate> Rates { get; set; }
    }

    public class Rate
    {
        public string CarType { get; set; }
        public decimal PerHour { get; set; }
        public decimal PerKm { get; set; }
    }
}
