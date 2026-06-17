using chamcong.Application.DTOs;
using chamcong.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace chamcong.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FingerprintsController : ControllerBase
    {
        private readonly IFingerprintService _fingerprintService;

        public FingerprintsController(IFingerprintService fingerprintService)
        {
            _fingerprintService = fingerprintService;
        }

        // IoT device hits this endpoint without Auth, or with a simple API Key
        // So we keep it AllowAnonymous for ESP32 simplicity
        [HttpPost("scan")]
        public async Task<IActionResult> Scan([FromBody] FingerprintScanDto req)
        {
            var result = await _fingerprintService.ProcessFingerprintScanAsync(req.FingerprintId);
            return result.Success ? Ok(result.Message) : StatusCode(result.StatusCode, result.Message);
        }
    }
}
