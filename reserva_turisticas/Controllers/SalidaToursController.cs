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
    public class SalidaToursController : ControllerBase
    {
        private readonly ReservaTuristicaContext _context;

        public SalidaToursController(ReservaTuristicaContext context)
        {
            _context = context;
        }

        // GET: api/SalidaTours
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalidaTour>>> GetSalidaTours()
        {
            return await _context.SalidaTours.ToListAsync();
        }

        // GET: api/SalidaTours/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SalidaTour>> GetSalidaTour(int id)
        {
            var salidaTour = await _context.SalidaTours.FindAsync(id);

            if (salidaTour == null)
            {
                return NotFound();
            }

            return salidaTour;
        }

        // PUT: api/SalidaTours/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalidaTour(int id, SalidaTour salidaTour)
        {
            if (id != salidaTour.Id)
            {
                return BadRequest();
            }

            _context.Entry(salidaTour).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalidaTourExists(id))
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

        // POST: api/SalidaTours
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SalidaTour>> PostSalidaTour(SalidaTour salidaTour)
        {
            _context.SalidaTours.Add(salidaTour);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSalidaTour", new { id = salidaTour.Id }, salidaTour);
        }

        // DELETE: api/SalidaTours/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalidaTour(int id)
        {
            var salidaTour = await _context.SalidaTours.FindAsync(id);
            if (salidaTour == null)
            {
                return NotFound();
            }

            _context.SalidaTours.Remove(salidaTour);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SalidaTourExists(int id)
        {
            return _context.SalidaTours.Any(e => e.Id == id);
        }
    }
}
