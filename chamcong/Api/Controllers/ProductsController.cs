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
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string query = "", [FromQuery] int? distributorId = null)
        {
            var productsQuery = _context.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.Distributor)
                .Include(p => p.ProductComponents)
                .ThenInclude(pc => pc.GarmentPart)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                productsQuery = productsQuery.Where(p => p.ProductName.Contains(query) || p.ProductCode.Contains(query));
            }

            if (distributorId.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.DistributorId == distributorId.Value);
            }

            var products = await productsQuery
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    ProductCode = p.ProductCode,
                    ProductName = p.ProductName,
                    DefaultUnitPrice = p.DefaultUnitPrice,
                    Size = p.Size,
                    ProductCategoryId = p.ProductCategoryId,
                    ProductCategoryName = p.ProductCategory != null ? p.ProductCategory.Name : null,
                    DistributorId = p.DistributorId,
                    DistributorName = p.Distributor != null ? p.Distributor.Name : null,
                    Quantity = p.Quantity,
                    Components = p.ProductComponents.Select(pc => new ProductComponentDto
                    {
                        Id = pc.Id,
                        ProductId = pc.ProductId,
                        GarmentPartId = pc.GarmentPartId,
                        GarmentPartName = pc.GarmentPart.Name,
                        QuantityPerProduct = pc.QuantityPerProduct
                    }).ToList()
                })
                .ToListAsync();

            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
        {
            var productCode = dto.ProductCode;
            if (string.IsNullOrWhiteSpace(productCode))
            {
                productCode = $"SP-{DateTime.Now:yyyyMMddHHmmss}";
            }

            var product = new Product
            {
                ProductCode = productCode,
                ProductName = dto.ProductName,
                DefaultUnitPrice = dto.DefaultUnitPrice,
                Size = dto.Size ?? "",
                ProductCategoryId = dto.ProductCategoryId,
                DistributorId = dto.DistributorId,
                Quantity = dto.Quantity
            };

            foreach (var compDto in dto.Components)
            {
                product.ProductComponents.Add(new ProductComponent
                {
                    GarmentPartId = compDto.GarmentPartId,
                    QuantityPerProduct = compDto.QuantityPerProduct
                });
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(new { product.Id });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
