using CarRentCalculator.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentCalculator.Application.Services
{
    public interface IRentService
    {
        Task<Result<decimal>> GetEstimatedCost(RentCalculationRequest request);
    }
}
