using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reserva_turisticas.Data;
using reserva_turisticas.Models;
using System.Data;
using Dapper;
using reserva_turisticas.Dtos;

namespace reserva_turisticas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagosController : ControllerBase
    {
        private readonly ReservaTuristicaContext _context;
        private readonly IDbConnection _db;

        public PagosController(ReservaTuristicaContext context, IDbConnection db)
        {
            _context = context;
            _db = db;
        }

        // GET: api/Pagos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pago>>> GetPagos()
        {
            return await _context.Pagos.ToListAsync();
        }

        // GET: api/Pagos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pago>> GetPago(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);

            if (pago == null)
            {
                return NotFound();
            }

            return pago;
        }

        // PUT: api/Pagos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPago(int id, Pago pago)
        {
            if (id != pago.Id)
            {
                return BadRequest();
            }

            _context.Entry(pago).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PagoExists(id))
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

        // POST: api/Pagos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pago>> PostPago(Pago pago)
        {
            _context.Pagos.Add(pago);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPago", new { id = pago.Id }, pago);
        }

        // DELETE: api/Pagos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePago(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null)
            {
                return NotFound();
            }

            _context.Pagos.Remove(pago);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PagoExists(int id)
        {
            return _context.Pagos.Any(e => e.Id == id);
        }

        // ------------------------------------------------------------
        // SP: dbo.SP_GENERAR_PAGO
        // POST: api/Pagos/generar
        // Body: { "facturaID": 1, "metodoPagoID": 1, "monedaID": 1, "observacion": "opcional" }
        // ------------------------------------------------------------
        [HttpPost("generar")]
        public async Task<IActionResult> GenerarPago([FromBody] GenerarPagoDto dto)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@pnFacturaID", dto.FacturaID);
            parametros.Add("@pnMetodoPagoID", dto.MetodoPagoID);
            parametros.Add("@pnMonedaID", dto.MonedaID);
            parametros.Add("@pcObservacion", dto.Observacion);

            parametros.Add("@pnPagoID", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parametros.Add("@pnTipoMensaje", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parametros.Add("@pcMensaje", dbType: DbType.String, size: 300, direction: ParameterDirection.Output);

            await _db.ExecuteAsync(
                "dbo.SP_GENERAR_PAGO",
                parametros,
                commandType: CommandType.StoredProcedure
            );

            var resultado = new GenerarPagoResultadoDto
            {
                PagoID = parametros.Get<int>("@pnPagoID"),
                TipoMensaje = parametros.Get<int>("@pnTipoMensaje"),
                Mensaje = parametros.Get<string>("@pcMensaje") ?? string.Empty
            };

            if (resultado.TipoMensaje != 0)
            {
                return BadRequest(resultado);
            }

            return Ok(resultado);
        }

        // ------------------------------------------------------------
        // SP: dbo.SP_REPORTE_PAGOS
        // POST: api/Pagos/reporte-pagos
        // Body: { "fechaInicio": "...", "fechaFin": "...", "clienteID": 1 }
        // ------------------------------------------------------------
        [HttpPost("reporte-pagos")]
        public async Task<ActionResult<IEnumerable<ReportePagosDto>>> GetReportePagos(
            [FromBody] ReportePagosFiltroDto filtro)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@pFechaInicio", filtro.FechaInicio);
            parametros.Add("@pFechaFin", filtro.FechaFin);
            parametros.Add("@pClienteID", filtro.ClienteID);

            var datos = await _db.QueryAsync<ReportePagosDto>(
                "dbo.SP_REPORTE_PAGOS",
                parametros,
                commandType: CommandType.StoredProcedure
            );

            return Ok(datos);
        }
    }
}
