using chamcong.Application.DTOs;
using chamcong.Domain.Entities;
using chamcong.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace chamcong.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class DistributorsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DistributorsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string query = "")
        {
            var distributorsQuery = _context.Distributors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                distributorsQuery = distributorsQuery.Where(d => d.Name.Contains(query) || d.Phone.Contains(query));
            }

            var distributors = await distributorsQuery
                .Select(d => new DistributorDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Phone = d.Phone,
                    Email = d.Email,
                    Address = d.Address
                })
                .ToListAsync();

            return Ok(distributors);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DistributorCreateDto dto)
        {
            var distributor = new Distributor
            {
                Name = dto.Name,
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address
            };

            _context.Distributors.Add(distributor);
            await _context.SaveChangesAsync();

            return Ok(new { distributor.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DistributorCreateDto dto)
        {
            var distributor = await _context.Distributors.FindAsync(id);
            if (distributor == null) return NotFound();

            distributor.Name = dto.Name;
            distributor.Phone = dto.Phone;
            distributor.Email = dto.Email;
            distributor.Address = dto.Address;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var distributor = await _context.Distributors.FindAsync(id);
            if (distributor == null) return NotFound();

            // Check if there are any products attached
            var hasProducts = await _context.Products.AnyAsync(p => p.DistributorId == id);
            if (hasProducts)
            {
                return BadRequest("Không thể xóa nhà phân phối đã có sản phẩm.");
            }

            _context.Distributors.Remove(distributor);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
