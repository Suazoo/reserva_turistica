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
    public class DisponibilidadToursController : ControllerBase
    {
        private readonly ReservaTuristicaContext _context;

        public DisponibilidadToursController(ReservaTuristicaContext context)
        {
            _context = context;
        }

        // GET: api/DisponibilidadTours
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DisponibilidadTour>>> GetDisponibilidadTours()
        {
            return await _context.DisponibilidadTours.ToListAsync();
        }

        // GET: api/DisponibilidadTours/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DisponibilidadTour>> GetDisponibilidadTour(int id)
        {
            var disponibilidadTour = await _context.DisponibilidadTours.FindAsync(id);

            if (disponibilidadTour == null)
            {
                return NotFound();
            }

            return disponibilidadTour;
        }

        // PUT: api/DisponibilidadTours/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDisponibilidadTour(int id, DisponibilidadTour disponibilidadTour)
        {
            if (id != disponibilidadTour.Id)
            {
                return BadRequest();
            }

            _context.Entry(disponibilidadTour).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DisponibilidadTourExists(id))
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

        // POST: api/DisponibilidadTours
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DisponibilidadTour>> PostDisponibilidadTour(DisponibilidadTour disponibilidadTour)
        {
            _context.DisponibilidadTours.Add(disponibilidadTour);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDisponibilidadTour", new { id = disponibilidadTour.Id }, disponibilidadTour);
        }

        // DELETE: api/DisponibilidadTours/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDisponibilidadTour(int id)
        {
            var disponibilidadTour = await _context.DisponibilidadTours.FindAsync(id);
            if (disponibilidadTour == null)
            {
                return NotFound();
            }

            _context.DisponibilidadTours.Remove(disponibilidadTour);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DisponibilidadTourExists(int id)
        {
            return _context.DisponibilidadTours.Any(e => e.Id == id);
        }
    }
}
