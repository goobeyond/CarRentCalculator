using CarRentCalculator.Application.Dtos;
using CarRentCalculator.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarRentCalculator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RentController : ControllerBase
    {
        private readonly IRentService _rentService;

        public RentController(IRentService rentService)
        {
            _rentService = rentService;
        }

        /// <summary>
        /// Calcualte the estimated cost of renting a car based on subscription type, car type and estimated travel.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Estimated cost.</returns>
        [HttpPost("Estimate")]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromBody] RentCalculationRequest request)
        {
            var result = await _rentService.GetEstimatedCost(request);

            return result.ResultType switch
            {
                ResultType.Success => Ok(result.Data),
                ResultType.Failure => BadRequest(result.ErrorMessage),
                _ => throw new InvalidOperationException(result.ErrorMessage)
            };
        }
    }
}