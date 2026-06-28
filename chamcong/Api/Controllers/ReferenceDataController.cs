using chamcong.Application.Interfaces;
using chamcong.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace chamcong.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReferenceDataController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReferenceDataController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("products")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products
                .Select(p => new { p.Id, p.ProductName })
                .ToListAsync();
            return Ok(products);
        }

        [HttpGet("workshops")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetWorkshops()
        {
            var workshops = await _context.Workshops
                .Select(w => new { w.Id, w.Name })
                .ToListAsync();
            return Ok(workshops);
        }

        [HttpGet("distributors")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDistributors()
        {
            var distributors = await _context.Distributors
                .Select(d => new { d.Id, d.Name })
                .ToListAsync();
            return Ok(distributors);
        }
    }
}
