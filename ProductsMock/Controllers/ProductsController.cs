using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ProductApi.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using ProductsMock.Helpers;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly Cloudinary _cloudinary;

        public ProductsController(AppDbContext context, IWebHostEnvironment env, IOptions<CloudinarySettings> config)
        {
            _context = context;
            _env = env;
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct([FromForm] ProductCreateDto dto)
        {
            string imageUrl = "";

            if (dto.Image != null && dto.Image.Length > 0)
            {
                await using var stream = dto.Image.OpenReadStream();

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(dto.Image.FileName, stream),
                    Folder = "products" // optional folder name on Cloudinary
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                    return BadRequest(uploadResult.Error.Message);

                imageUrl = uploadResult.SecureUrl.AbsoluteUri;
            }

            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                ImageUrl = imageUrl
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProducts), new { id = product.Id }, product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
