using chamcong.Application.DTOs;
using chamcong.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace chamcong.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BatchesController : ControllerBase
    {
        private readonly IBatchService _batchService;

        public BatchesController(IBatchService batchService)
        {
            _batchService = batchService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetBatches()
        {
            // Simple mock role check. In real app, extract from Claims
            bool isAdmin = User.IsInRole("Admin");
            int? workshopId = null;

            if (!isAdmin)
            {
                var workshopClaim = User.FindFirst("WorkshopId");
                if (workshopClaim != null && int.TryParse(workshopClaim.Value, out int wId))
                {
                    workshopId = wId;
                }
            }

            var result = await _batchService.GetBatchesAsync(workshopId, isAdmin);
            return result.Success ? Ok(result.Data) : StatusCode(result.StatusCode, result.Message);
        }

        [HttpPost("sub-batch")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateSubBatch([FromBody] SubBatchCreateDto dto)
        {
            var result = await _batchService.CreateSubBatchAsync(dto);
            return result.Success ? StatusCode(result.StatusCode, result.Data) : StatusCode(result.StatusCode, result.Message);
        }
    }
}
