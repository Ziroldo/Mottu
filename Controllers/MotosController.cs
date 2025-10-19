using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mottu.Backend.Data;
using Mottu.Backend.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Mottu.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotosController : ControllerBase
    {
        private readonly AppDbContext _db;
        public MotosController(AppDbContext db) => _db = db;

        // GET: api/motos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAll()
        {
            var items = await _db.Motos
                .Select(m => new { m.Id, m.Placa })
                .ToListAsync();
            return Ok(items);
        }

        // GET: api/motos/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<object>> GetById(int id)
        {
            var moto = await _db.Motos
                .Where(m => m.Id == id)
                .Select(m => new { m.Id, m.Placa })
                .FirstOrDefaultAsync();

            if (moto is null) return NotFound();
            return Ok(moto);
        }

        // POST: api/motos
        public class MotoCreateDto { public string Placa { get; set; } = string.Empty; }

        [HttpPost]
        public async Task<ActionResult<object>> Create([FromBody] MotoCreateDto dto)
        {
            // conflito 409 se placa já existe
            var exists = await _db.Motos.AnyAsync(m => m.Placa == dto.Placa);
            if (exists) return Conflict($"Placa '{dto.Placa}' já cadastrada.");

            var entity = new Moto { Placa = dto.Placa.Trim().ToUpperInvariant() };
            _db.Motos.Add(entity);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, new { entity.Id, entity.Placa });
        }

        // PUT: api/motos/5
        public class MotoUpdateDto { public string Placa { get; set; } = string.Empty; }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] MotoUpdateDto dto)
        {
            var entity = await _db.Motos.FindAsync(id);
            if (entity is null) return NotFound();

            var placaEmUso = await _db.Motos.AnyAsync(m => m.Id != id && m.Placa == dto.Placa);
            if (placaEmUso) return Conflict($"Placa '{dto.Placa}' já cadastrada.");

            entity.Placa = dto.Placa.Trim().ToUpperInvariant();
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/motos/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _db.Motos.FindAsync(id);
            if (entity is null) return NotFound();
            _db.Motos.Remove(entity);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}

