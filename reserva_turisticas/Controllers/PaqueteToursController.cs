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
    public class PaqueteToursController : ControllerBase
    {
        private readonly ReservaTuristicaContext _context;

        public PaqueteToursController(ReservaTuristicaContext context)
        {
            _context = context;
        }

        // GET: api/PaqueteTours
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaqueteTour>>> GetPaqueteTours()
        {
            return await _context.PaqueteTours.ToListAsync();
        }

        // GET: api/PaqueteTours/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaqueteTour>> GetPaqueteTour(int id)
        {
            var paqueteTour = await _context.PaqueteTours.FindAsync(id);

            if (paqueteTour == null)
            {
                return NotFound();
            }

            return paqueteTour;
        }

        // PUT: api/PaqueteTours/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaqueteTour(int id, PaqueteTour paqueteTour)
        {
            if (id != paqueteTour.PaqueteId)
            {
                return BadRequest();
            }

            _context.Entry(paqueteTour).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaqueteTourExists(id))
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

        // POST: api/PaqueteTours
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PaqueteTour>> PostPaqueteTour(PaqueteTour paqueteTour)
        {
            _context.PaqueteTours.Add(paqueteTour);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PaqueteTourExists(paqueteTour.PaqueteId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPaqueteTour", new { id = paqueteTour.PaqueteId }, paqueteTour);
        }

        // DELETE: api/PaqueteTours/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaqueteTour(int id)
        {
            var paqueteTour = await _context.PaqueteTours.FindAsync(id);
            if (paqueteTour == null)
            {
                return NotFound();
            }

            _context.PaqueteTours.Remove(paqueteTour);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaqueteTourExists(int id)
        {
            return _context.PaqueteTours.Any(e => e.PaqueteId == id);
        }
    }
}
