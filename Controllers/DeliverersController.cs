using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mottu.Backend.Data;
using Mottu.Backend.Services;

namespace Mottu.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliverersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly StorageService _storage;

        public DeliverersController(AppDbContext context, StorageService storage)
        {
            _context = context;
            _storage = storage;
        }

        [HttpPut("{id}/cnh")]
        public async Task<IActionResult> UploadCnh(int id, IFormFile file)
        {
            var deliverer = await _context.Deliverers.FirstOrDefaultAsync(d => d.Id == id);
            if (deliverer == null) return NotFound("Deliverer not found.");
            if (file == null || file.Length == 0) return BadRequest("No file uploaded.");

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (ext != ".png" && ext != ".bmp")
                return BadRequest("Invalid file format. Only PNG or BMP are allowed.");

            var objectName = $"cnh_{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            using var stream = file.OpenReadStream();
            await _storage.UploadFileAsync(objectName, stream, file.ContentType, file.Length);

            deliverer.CNHImagePath = objectName;
            await _context.SaveChangesAsync();

            return Ok(new { deliverer.Id, deliverer.Name, deliverer.CNHImagePath });
        }
    }
}
