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
    public class ReservasController : ControllerBase
    {
        private readonly ReservaTuristicaContext _context;

        public ReservasController(ReservaTuristicaContext context)
        {
            _context = context;
        }

        // GET: api/Reservas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetReservas()
        {
            return await _context.Reservas.ToListAsync();
        }

        // GET: api/Reservas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reserva>> GetReserva(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);

            if (reserva == null)
            {
                return NotFound();
            }

            return reserva;
        }

        // PUT: api/Reservas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReserva(int id, Reserva reserva)
        {
            if (id != reserva.Id)
            {
                return BadRequest();
            }

            _context.Entry(reserva).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaExists(id))
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

        // POST: api/Reservas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Reserva>> PostReserva(Reserva reserva)
        {
            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReserva", new { id = reserva.Id }, reserva);
        }

        // DELETE: api/Reservas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReserva(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.Id == id);
        }


        [Route("api/Servicio-Cliente")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetReservasServicioCliente()
        {
            var reservas = await _context.Reservas
                .Include(r => r.Cliente)
                    .ThenInclude(c => c.Persona)    // Nombre del cliente
                .Include(r => r.ReservaServicios)
                    .ThenInclude(rs => rs.Servicio) // Nombre del servicio
                .Select(r => new
                {
                    id = r.Id,

                    // Nombre completo del cliente
                    cliente =
                        (r.Cliente.Persona.PrimerNombre + " " +
                        r.Cliente.Persona.PrimerApellido).Trim(),

                    // Nombre del servicio
                    servicio = r.ReservaServicios
                                .Select(x => x.Servicio.Nombre)
                                .FirstOrDefault() ?? "—",

                    estado = r.Estado,
                    total = r.Total
                })
                .ToListAsync();

            return Ok(reservas);
        }


    }
}
