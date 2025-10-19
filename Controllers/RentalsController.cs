using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mottu.Backend.Data;
using Mottu.Backend.Models;

namespace Mottu.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RentalsController(AppDbContext context) => _context = context;

        [HttpPost]
        public async Task<IActionResult> CreateRental([FromBody] Rental rental)
        {
            var deliverer = await _context.Deliverers.FirstOrDefaultAsync(d => d.Id == rental.DelivererId);
            if (deliverer == null) return NotFound("Deliverer not found.");
            if (deliverer.CNHType != "A" && deliverer.CNHType != "A+B")
                return BadRequest("Deliverer not authorized to rent a motorcycle.");

            var moto = await _context.Motos.FirstOrDefaultAsync(m => m.Id == rental.MotoId);
            if (moto == null) return NotFound("Moto not found.");

            var dailyPrice = rental.Plan switch
            {
                7  => 30m,
                15 => 28m,
                30 => 22m,
                45 => 20m,
                50 => 18m,
                _  => 0m
            };
            if (dailyPrice == 0m) return BadRequest("Invalid rental plan.");

            rental.Id = 0;
            rental.DailyPrice = dailyPrice;
            rental.StartDate  = DateTime.UtcNow.Date.AddDays(1);              // início = amanhã
            rental.EndDate    = rental.StartDate.AddDays(rental.Plan - 1);    // previsão = inclusiva
            rental.ReturnDate = null;
            rental.TotalCost  = dailyPrice * rental.Plan;

            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRental), new { id = rental.Id }, rental);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRental(int id)
        {
            var rental = await _context.Rentals.FirstOrDefaultAsync(r => r.Id == id);
            if (rental == null) return NotFound();
            return Ok(rental);
        }

        [HttpPut("{id}/return")]
        public async Task<IActionResult> ReturnRental(int id, [FromBody] DateTime returnDate)
        {
            var rental = await _context.Rentals.FirstOrDefaultAsync(r => r.Id == id);
            if (rental == null) return NotFound("Rental not found.");

            if (returnDate.Kind != DateTimeKind.Utc)
                returnDate = DateTime.SpecifyKind(returnDate, DateTimeKind.Utc);

            var total = rental.TotalCost;

            if (returnDate < rental.EndDate)
            {
                var usedDays   = Math.Max(0, (returnDate.Date - rental.StartDate.Date).Days + 1);
                var unusedDays = Math.Max(0, rental.Plan - usedDays);

                var penaltyRate = rental.Plan switch
                {
                    7  => 0.20m,
                    15 => 0.40m,
                    _  => 0m
                };

                var penalty = penaltyRate * (unusedDays * rental.DailyPrice);
                total = (usedDays * rental.DailyPrice) + penalty;
            }
            else if (returnDate > rental.EndDate)
            {
                var extraDays = Math.Max(0, (returnDate.Date - rental.EndDate.Date).Days);
                total += extraDays * 50m;
            }

            rental.ReturnDate = returnDate;
            rental.TotalCost  = total;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                rental.Id,
                rental.Plan,
                rental.DailyPrice,
                rental.StartDate,
                rental.EndDate,
                rental.ReturnDate,
                TotalToPay = total
            });
        }
    }
}
