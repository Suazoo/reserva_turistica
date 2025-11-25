using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reserva_turisticas.Data;
using reserva_turisticas.Models;
using reserva_turisticas.Dtos;
using System.Data;
using Dapper;

namespace reserva_turisticas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservasController : ControllerBase
    {
        private readonly ReservaTuristicaContext _context;
        private readonly IDbConnection _db;        // <-- NUEVO

        // Constructor actualizado: recibe también IDbConnection
        public ReservasController(ReservaTuristicaContext context, IDbConnection db)
        {
            _context = context;
            _db = db;                              // <-- NUEVO
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
                    .ThenInclude(c => c.Persona)
                .Include(r => r.ReservaServicios)
                    .ThenInclude(rs => rs.Servicio)
                .Select(r => new
                {
                    id = r.Id,
                    cliente =
                        (r.Cliente.Persona.PrimerNombre + " " +
                        r.Cliente.Persona.PrimerApellido).Trim(),
                    servicio = r.ReservaServicios
                                .Select(x => x.Servicio.Nombre)
                                .FirstOrDefault() ?? "—",
                    estado = r.Estado,
                    total = r.Total
                })
                .ToListAsync();

            return Ok(reservas);
        }

        // ------------------------------------------------------------
        // 1) Vista: dbo.VW_Reserva_Clientes
        // GET: api/Reservas/vista-clientes
        // ------------------------------------------------------------
        [HttpGet("vista-clientes")]
        public async Task<ActionResult<IEnumerable<ReservaClientesDto>>> GetVistaReservaClientes()
        {
            const string sql = @"SELECT ClienteID, CodigoCliente, NombreCompleto
                                 FROM dbo.VW_Reserva_Clientes";

            var datos = await _db.QueryAsync<ReservaClientesDto>(sql);
            return Ok(datos);
        }

        // ------------------------------------------------------------
        // 2) Vista: dbo.VW_Reserva_ServicioInfo
        // GET: api/Reservas/vista-servicios-info
        // ------------------------------------------------------------
        [HttpGet("vista-servicios-info")]
        public async Task<ActionResult<IEnumerable<ReservaServicioInfoDto>>> GetVistaReservaServicioInfo()
        {
            const string sql = @"SELECT ServicioID, Nombre, Tipo, Precio, Duracion
                                 FROM dbo.VW_Reserva_ServicioInfo";

            var datos = await _db.QueryAsync<ReservaServicioInfoDto>(sql);
            return Ok(datos);
        }

        // ------------------------------------------------------------
        // 3) Vista: dbo.VW_Reserva_Servicios
        // GET: api/Reservas/vista-servicios
        // ------------------------------------------------------------
        [HttpGet("vista-servicios")]
        public async Task<ActionResult<IEnumerable<ReservaServiciosDto>>> GetVistaReservaServicios()
        {
            const string sql = @"SELECT ServicioID, Nombre, Tipo
                                 FROM dbo.VW_Reserva_Servicios";

            var datos = await _db.QueryAsync<ReservaServiciosDto>(sql);
            return Ok(datos);
        }

        // ------------------------------------------------------------
        // 4) Vista: dbo.VW_Reservas_Listado
        // GET: api/Reservas/vista-reservas-listado
        // ------------------------------------------------------------
        [HttpGet("vista-reservas-listado")]
        public async Task<ActionResult<IEnumerable<ReservasListadoDto>>> GetVistaReservasListado()
        {
            const string sql = @"SELECT ReservaID, CodigoReserva, Fecha_Creacion, Estado, Total, Cliente, Contrato
                                 FROM dbo.VW_Reservas_Listado";

            var datos = await _db.QueryAsync<ReservasListadoDto>(sql);
            return Ok(datos);
        }
    }
}
