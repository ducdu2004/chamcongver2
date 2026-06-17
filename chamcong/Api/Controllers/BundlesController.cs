using chamcong.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace chamcong.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BundlesController : ControllerBase
    {
        private readonly IProductionService _productionService;

        public BundlesController(IProductionService productionService)
        {
            _productionService = productionService;
        }

        public class GenerateBundleRequest
        {
            public int BatchId { get; set; }
            public int NumberOfBundles { get; set; }
            public string StepName { get; set; }
            public decimal StepPrice { get; set; }
            public int QuantityPerBundle { get; set; }
        }

        [HttpPost("generate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GenerateBundles([FromBody] GenerateBundleRequest req)
        {
            var result = await _productionService.GenerateBundlesAsync(req.BatchId, req.NumberOfBundles, req.StepName, req.StepPrice, req.QuantityPerBundle);
            return result.Success ? Ok(result.Message) : StatusCode(result.StatusCode, result.Message);
        }

        public class CompleteBundleRequest
        {
            public int EmployeeId { get; set; }
        }

        [HttpPost("{id}/complete")]
        [Authorize]
        public async Task<IActionResult> CompleteBundle(int id, [FromBody] CompleteBundleRequest req)
        {
            var result = await _productionService.CompleteBundleAsync(id, req.EmployeeId);
            return result.Success ? Ok(result.Message) : StatusCode(result.StatusCode, result.Message);
        }
    }
}
