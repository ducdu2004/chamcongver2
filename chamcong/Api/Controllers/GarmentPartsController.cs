using chamcong.Application.DTOs;
using chamcong.Domain.Entities;
using chamcong.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace chamcong.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class GarmentPartsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GarmentPartsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var parts = await _context.GarmentParts
                .Select(p => new GarmentPartDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    ProductCategoryId = p.ProductCategoryId,
                    DefaultUnitPrice = p.DefaultUnitPrice
                })
                .ToListAsync();
            return Ok(parts);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var parts = await _context.GarmentParts
                .Where(p => p.ProductCategoryId == categoryId)
                .Select(p => new GarmentPartDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    ProductCategoryId = p.ProductCategoryId,
                    DefaultUnitPrice = p.DefaultUnitPrice
                })
                .ToListAsync();
            return Ok(parts);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GarmentPartDto dto)
        {
            var part = new GarmentPart
            {
                Name = dto.Name,
                ProductCategoryId = dto.ProductCategoryId,
                DefaultUnitPrice = dto.DefaultUnitPrice
            };
            _context.GarmentParts.Add(part);
            await _context.SaveChangesAsync();
            dto.Id = part.Id;
            return Ok(dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] GarmentPartDto dto)
        {
            var part = await _context.GarmentParts.FindAsync(id);
            if (part == null) return NotFound();

            part.Name = dto.Name;
            part.ProductCategoryId = dto.ProductCategoryId;
            part.DefaultUnitPrice = dto.DefaultUnitPrice;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var part = await _context.GarmentParts.FindAsync(id);
            if (part == null) return NotFound();

            _context.GarmentParts.Remove(part);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
