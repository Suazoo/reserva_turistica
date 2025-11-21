using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reserva_turisticas.Data;
using reserva_turisticas.Models;

namespace reserva_turisticas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaToursController : ControllerBase
    {
        private readonly ReservaTuristicaContext _context;

        public ReservaToursController(ReservaTuristicaContext context)
        {
            _context = context;
        }

        // GET: api/ReservaTours
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservaTour>>> GetReservaTours()
        {
            return await _context.ReservaTours.ToListAsync();
        }

        // GET: api/ReservaTours/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReservaTour>> GetReservaTour(int id)
        {
            var reservaTour = await _context.ReservaTours.FindAsync(id);

            if (reservaTour == null)
            {
                return NotFound();
            }

            return reservaTour;
        }

        // PUT: api/ReservaTours/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReservaTour(int id, ReservaTour reservaTour)
        {
            if (id != reservaTour.ReservaId)
            {
                return BadRequest();
            }

            _context.Entry(reservaTour).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaTourExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ReservaTours
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ReservaTour>> PostReservaTour(ReservaTour reservaTour)
        {
            _context.ReservaTours.Add(reservaTour);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ReservaTourExists(reservaTour.ReservaId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetReservaTour", new { id = reservaTour.ReservaId }, reservaTour);
        }

        // DELETE: api/ReservaTours/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservaTour(int id)
        {
            var reservaTour = await _context.ReservaTours.FindAsync(id);
            if (reservaTour == null)
            {
                return NotFound();
            }

            _context.ReservaTours.Remove(reservaTour);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReservaTourExists(int id)
        {
            return _context.ReservaTours.Any(e => e.ReservaId == id);
        }
    }
}
