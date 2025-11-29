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
        [HttpPost]
        public async Task<ActionResult<Reserva>> PostReserva([FromBody] ReservaCrearDto dto)
        {
            var reserva = new Reserva
            {
                
                CodigoReserva = dto.CodigoReserva,
                FechaCreacion = dto.FechaCreacion,
                Estado        = dto.Estado,
                Total         = dto.Total,
                ContratoId    = dto.ContratoId,
                MonedaId      = dto.MonedaId,
                ClienteId     = dto.ClienteId,
                HotelId       = dto.HotelId,
                Politicas     = dto.Politicas
            };

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReserva), new { id = reserva.Id }, reserva);
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


        // ------------------------------------------------------------
        // 5) Vista: dbo.VW_Dashboard_Resumen
        // GET: api/Reservas/vista-dashboard-resumen
        // ------------------------------------------------------------
        [HttpGet("vista-dashboard-resumen")]
        public async Task<ActionResult<IEnumerable<DashboardResumenDto>>> GetVistaDashboardResumen()
        {
            const string sql = @"SELECT Tipo, Valor1, Valor2, Valor3
                                 FROM dbo.VW_Dashboard_Resumen";

            var datos = await _db.QueryAsync<DashboardResumenDto>(sql);
            return Ok(datos);
        }



        

        // ------------------------------------------------------------
        // 6) SP: dbo.SP_ACTUALIZAR_RESERVA
        // PUT: api/Reservas/actualizar-reserva
        // ------------------------------------------------------------
        [HttpPut("actualizar-reserva")]
        public async Task<IActionResult> ActualizarReserva([FromBody] ActualizarReservaDto dto)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@pnReservaID",  dto.ReservaID);
            parametros.Add("@pnClienteID",  dto.ClienteID);
            parametros.Add("@pnContratoID", dto.ContratoID);
            parametros.Add("@pnMonedaID",  dto.MonedaID);
            parametros.Add("@pcEstado",    dto.Estado);
            parametros.Add("@pcPoliticas", dto.Politicas);
            parametros.Add("@pnTotalNuevo",dto.TotalNuevo);

            parametros.Add("@pnTipoMensaje", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parametros.Add("@pcMensaje",     dbType: DbType.String, size: 300, direction: ParameterDirection.Output);

            await _db.ExecuteAsync(
                "dbo.SP_ACTUALIZAR_RESERVA",
                parametros,
                commandType: CommandType.StoredProcedure
            );

            var tipoMensaje = parametros.Get<int>("@pnTipoMensaje");
            var mensaje     = parametros.Get<string>("@pcMensaje");

            if (tipoMensaje != 0)
            {
                // Error desde el SP
                return BadRequest(new { tipoMensaje, mensaje });
            }

            return Ok(new { tipoMensaje, mensaje });
        }





        // ------------------------------------------------------------
        // 7) SP: dbo.SP_CREAR_RESERVACION 
        // POST: api/Reservas/crear-reservacion
        // ------------------------------------------------------------
        // ------------------------------------------------------------
        
        [HttpPost("crear-reservacion")]
        public async Task<IActionResult> CrearReservacion([FromBody] CrearReservacionDto dto)
        {
            var parametros = new DynamicParameters();

            // ENTRADAS
            parametros.Add("@pnClienteID", dto.ClienteID);
            parametros.Add("@pnServicioID", dto.ServicioID);
            parametros.Add("@pnTourID", dto.TourID);
            parametros.Add("@pnHotelID", dto.HotelID);
            parametros.Add("@pnPaqueteID", dto.PaqueteID);
            parametros.Add("@pFechaReserva", dto.FechaReserva.Date);

            
            parametros.Add("@pnTotal", dto.pnTotal, dbType: DbType.Decimal, direction: ParameterDirection.Input);

            // SALIDAS
            parametros.Add("@pcNombreCliente", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            parametros.Add("@pnReservaID", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parametros.Add("@pnTipoMensaje", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parametros.Add("@pcMensaje", dbType: DbType.String, size: 500, direction: ParameterDirection.Output);

            await _db.ExecuteAsync(
                "dbo.SP_CREAR_RESERVACION",
                parametros,
                commandType: CommandType.StoredProcedure
            );

            var tipoMensaje = parametros.Get<int>("@pnTipoMensaje");
            var mensaje = parametros.Get<string>("@pcMensaje") ?? string.Empty;
            var reservaID = parametros.Get<int>("@pnReservaID");
            var nombreCliente = parametros.Get<string>("@pcNombreCliente") ?? string.Empty;

            return Ok(new
            {
                tipoMensaje,
                mensaje,
                reservaID,
                nombreCliente,
                total = dto.pnTotal
            });
        }


        // ------------------------------------------------------------
        // 8) SP: dbo.SP_ELIMINAR_RESERVA_COMPLETA
        // DELETE: api/Reservas/eliminar-reserva-completa/{id}
        // ------------------------------------------------------------
        [HttpDelete("eliminar-reserva-completa/{id}")]
        public async Task<IActionResult> EliminarReservaCompleta(int id)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@pnReservaID", id);
            parametros.Add("@pnTipoMensaje", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parametros.Add("@pcMensaje", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);

            await _db.ExecuteAsync(
                "dbo.SP_ELIMINAR_RESERVA_COMPLETA",
                parametros,
                commandType: CommandType.StoredProcedure
            );

            var tipoMensaje = parametros.Get<int>("@pnTipoMensaje");
            var mensaje = parametros.Get<string>("@pcMensaje") ?? string.Empty;

            if (tipoMensaje != 0)
            {
                return BadRequest(new { tipoMensaje, mensaje });
            }

            return Ok(new { tipoMensaje, mensaje });
        }

        // ------------------------------------------------------------
        // 9) SP: dbo.SP_REPORTE_RESERVAS
        // POST: api/Reservas/reporte-reservas
        // Body: { "fechaInicio": "...", "fechaFin": "...", "clienteID": 1 }
        // ------------------------------------------------------------
        [HttpPost("reporte-reservas")]
        public async Task<ActionResult<IEnumerable<ReporteReservasDto>>> GetReporteReservas(
            [FromBody] ReporteReservasFiltroDto filtro)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@pFechaInicio", filtro.FechaInicio);
            parametros.Add("@pFechaFin", filtro.FechaFin);
            parametros.Add("@pClienteID", filtro.ClienteID);

            var datos = await _db.QueryAsync<ReporteReservasDto>(
                "dbo.SP_REPORTE_RESERVAS",
                parametros,
                commandType: CommandType.StoredProcedure
            );

            return Ok(datos);
        }

        

    }
}
