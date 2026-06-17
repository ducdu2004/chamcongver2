using chamcong.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace chamcong.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IBatchService _batchService;

        public ReportsController(IBatchService batchService)
        {
            _batchService = batchService;
        }

        [HttpGet("export-voucher/{batchId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExportVoucher(int batchId)
        {
            var result = await _batchService.ExportVoucherAsync(batchId);
            return result.Success ? Ok(result.Data) : StatusCode(result.StatusCode, result.Message);
        }
    }
}
