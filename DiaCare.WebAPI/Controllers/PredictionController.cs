using DiaCare.Application.Helpers;
using DiaCare.Application.Interfaces;
using DiaCare.Application.Services;
using DiaCare.Domain.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiaCare.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PredictionController : Controller
    {
        private readonly IPredictionService _predictionService;
        public PredictionController(IPredictionService predictionService)
        {
            _predictionService = predictionService; 
        }
        [HttpPost("predict")]
       
        public async Task<IActionResult> Predict(PredictionInputDto dto)
        {
            if (dto == null)
                return BadRequest(Result<string>.Failure("Data is required"));

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var result = await _predictionService.PredictAsync(dto, userId);

            if (result == null || result.RiskCategory == "Service unavailable")
                return StatusCode(500, Result<PredictionResultDto>.Failure("AI Model is down", 500));

            return Ok(Result<PredictionResultDto>.Success(result, "Prediction completed successfully"));
        }
    }
}
