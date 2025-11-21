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
    public class ServicioHotelsController : ControllerBase
    {
        private readonly ReservaTuristicaContext _context;

        public ServicioHotelsController(ReservaTuristicaContext context)
        {
            _context = context;
        }

        // GET: api/ServicioHotels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicioHotel>>> GetServicioHotels()
        {
            return await _context.ServicioHotels.ToListAsync();
        }

        // GET: api/ServicioHotels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServicioHotel>> GetServicioHotel(int id)
        {
            var servicioHotel = await _context.ServicioHotels.FindAsync(id);

            if (servicioHotel == null)
            {
                return NotFound();
            }

            return servicioHotel;
        }

        // PUT: api/ServicioHotels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServicioHotel(int id, ServicioHotel servicioHotel)
        {
            if (id != servicioHotel.ServicioId)
            {
                return BadRequest();
            }

            _context.Entry(servicioHotel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServicioHotelExists(id))
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

        // POST: api/ServicioHotels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ServicioHotel>> PostServicioHotel(ServicioHotel servicioHotel)
        {
            _context.ServicioHotels.Add(servicioHotel);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ServicioHotelExists(servicioHotel.ServicioId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetServicioHotel", new { id = servicioHotel.ServicioId }, servicioHotel);
        }

        // DELETE: api/ServicioHotels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServicioHotel(int id)
        {
            var servicioHotel = await _context.ServicioHotels.FindAsync(id);
            if (servicioHotel == null)
            {
                return NotFound();
            }

            _context.ServicioHotels.Remove(servicioHotel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServicioHotelExists(int id)
        {
            return _context.ServicioHotels.Any(e => e.ServicioId == id);
        }
    }
}
