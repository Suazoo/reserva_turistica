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
    public class ReservaServiciosController : ControllerBase
    {
        private readonly ReservaTuristicaContext _context;

        public ReservaServiciosController(ReservaTuristicaContext context)
        {
            _context = context;
        }

        // GET: api/ReservaServicios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservaServicio>>> GetReservaServicios()
        {
            return await _context.ReservaServicios.ToListAsync();
        }

        // GET: api/ReservaServicios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReservaServicio>> GetReservaServicio(int id)
        {
            var reservaServicio = await _context.ReservaServicios.FindAsync(id);

            if (reservaServicio == null)
            {
                return NotFound();
            }

            return reservaServicio;
        }

        // PUT: api/ReservaServicios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReservaServicio(int id, ReservaServicio reservaServicio)
        {
            if (id != reservaServicio.ServicioId)
            {
                return BadRequest();
            }

            _context.Entry(reservaServicio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaServicioExists(id))
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

        // POST: api/ReservaServicios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ReservaServicio>> PostReservaServicio(ReservaServicio reservaServicio)
        {
            _context.ReservaServicios.Add(reservaServicio);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ReservaServicioExists(reservaServicio.ServicioId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetReservaServicio", new { id = reservaServicio.ServicioId }, reservaServicio);
        }

        // DELETE: api/ReservaServicios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservaServicio(int id)
        {
            var reservaServicio = await _context.ReservaServicios.FindAsync(id);
            if (reservaServicio == null)
            {
                return NotFound();
            }

            _context.ReservaServicios.Remove(reservaServicio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReservaServicioExists(int id)
        {
            return _context.ReservaServicios.Any(e => e.ServicioId == id);
        }
    }
}
