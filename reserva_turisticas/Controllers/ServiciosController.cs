using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reserva_turisticas.Data;
using reserva_turisticas.Models;
using reserva_turisticas.Dtos;
using Dapper;

namespace reserva_turisticas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiciosController : ControllerBase
    {
        private readonly ReservaTuristicaContext _context;
        private readonly IDbConnection _db;

        public ServiciosController(ReservaTuristicaContext context, IDbConnection db)
        {
            _context = context;
            _db = db;
        }

        // GET: api/Servicios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Servicio>>> GetServicios()
        {
            return await _context.Servicios.ToListAsync();
        }

        // GET: api/Servicios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Servicio>> GetServicio(int id)
        {
            var servicio = await _context.Servicios.FindAsync(id);

            if (servicio == null)
            {
                return NotFound();
            }

            return servicio;
        }

        // PUT: api/Servicios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServicio(int id, Servicio servicio)
        {
            if (id != servicio.Id)
            {
                return BadRequest();
            }

            _context.Entry(servicio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServicioExists(id))
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

        // POST: api/Servicios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Servicio>> PostServicio(Servicio servicio)
        {
            _context.Servicios.Add(servicio);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetServicio", new { id = servicio.Id }, servicio);
        }

        // DELETE: api/Servicios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServicio(int id)
        {
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null)
            {
                return NotFound();
            }

            _context.Servicios.Remove(servicio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServicioExists(int id)
        {
            return _context.Servicios.Any(e => e.Id == id);
        }


        // ------------------------------------------------------------
        // 1) Vista: dbo.VW_Servicios_Gestion
        // GET: api/Servicios/vista-gestion
        // ------------------------------------------------------------
        [HttpGet("vista-gestion")]
        public async Task<ActionResult<IEnumerable<ServiciosGestionDto>>> GetVistaServiciosGestion()
        {
            const string sql = @"SELECT ServicioID, CodigoServicio, Nombre, Tipo, Precio, Duracion, Estado
                                 FROM dbo.VW_Servicios_Gestion";

            var datos = await _db.QueryAsync<ServiciosGestionDto>(sql);
            return Ok(datos);
        }

        // ------------------------------------------------------------
        // 2) Vista: dbo.VW_ServiciosMasReservados
        // GET: api/Servicios/vista-mas-reservados
        // ------------------------------------------------------------
        [HttpGet("vista-mas-reservados")]
        public async Task<ActionResult<IEnumerable<ServiciosMasReservadosDto>>> GetVistaServiciosMasReservados()
        {
            const string sql = @"SELECT Servicio, Tipo, TotalReservas, Ingresos
                                 FROM dbo.VW_ServiciosMasReservados";

            var datos = await _db.QueryAsync<ServiciosMasReservadosDto>(sql);
            return Ok(datos);
        }




    }
}
