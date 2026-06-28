using chamcong.Application.DTOs;
using chamcong.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace chamcong.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionController : ControllerBase
    {
        private readonly IProductionService _productionService;

        public ProductionController(IProductionService productionService)
        {
            _productionService = productionService;
        }

        [HttpPost("manual")]
        public async Task<IActionResult> AddManualProduction([FromBody] ManualProductionCreateDto dto)
        {
            var result = await _productionService.AddManualProductionLogAsync(dto);
            if (!result.Success) return BadRequest(result.Message);
            return Ok(new { message = result.Message });
        }

        [HttpGet("manual/{employeeId}")]
        public async Task<IActionResult> GetManualProductions(int employeeId, [FromQuery] DateTime? date)
        {
            var filterDate = date ?? DateTime.Now.Date;
            var result = await _productionService.GetManualProductionLogsAsync(employeeId, filterDate);
            if (!result.Success) return BadRequest(result.Message);
            return Ok(result.Data);
        }
    }
}
