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
    public class ProductCategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductCategoriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _context.ProductCategories
                .Select(c => new ProductCategoryDto { Id = c.Id, Name = c.Name })
                .ToListAsync();
            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCategoryDto dto)
        {
            var category = new ProductCategory { Name = dto.Name };
            _context.ProductCategories.Add(category);
            await _context.SaveChangesAsync();
            dto.Id = category.Id;
            return Ok(dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductCategoryDto dto)
        {
            var category = await _context.ProductCategories.FindAsync(id);
            if (category == null) return NotFound();

            category.Name = dto.Name;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.ProductCategories.FindAsync(id);
            if (category == null) return NotFound();

            _context.ProductCategories.Remove(category);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
