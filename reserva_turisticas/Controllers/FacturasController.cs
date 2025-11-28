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
    public class FacturasController : ControllerBase
    {
        private readonly ReservaTuristicaContext _context;
        private readonly IDbConnection _db;

        public FacturasController(ReservaTuristicaContext context, IDbConnection db)
        {
            _context = context;
            _db = db;
        }

        // GET: api/Facturas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Factura>>> GetFacturas()
        {
            return await _context.Facturas.ToListAsync();
        }

        // GET: api/Facturas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Factura>> GetFactura(int id)
        {
            var factura = await _context.Facturas.FindAsync(id);

            if (factura == null)
            {
                return NotFound();
            }

            return factura;
        }

        // PUT: api/Facturas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFactura(int id, Factura factura)
        {
            if (id != factura.Id)
            {
                return BadRequest();
            }

            _context.Entry(factura).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FacturaExists(id))
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

        // POST: api/Facturas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Factura>> PostFactura(Factura factura)
        {
            _context.Facturas.Add(factura);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFactura", new { id = factura.Id }, factura);
        }

        // DELETE: api/Facturas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFactura(int id)
        {
            var factura = await _context.Facturas.FindAsync(id);
            if (factura == null)
            {
                return NotFound();
            }

            _context.Facturas.Remove(factura);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FacturaExists(int id)
        {
            return _context.Facturas.Any(e => e.Id == id);
        }

        // ------------------------------------------------------------
        // SP: dbo.SP_GENERAR_FACTURA
        // POST: api/Facturas/generar
        // Body: { "reservaID": 1, "monedaID": 1 }
        // ------------------------------------------------------------
        [HttpPost("generar")]
        public async Task<IActionResult> GenerarFactura([FromBody] GenerarFacturaDto dto)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@pnReservaID", dto.ReservaID);
            parametros.Add("@pnMonedaID", dto.MonedaID);

            parametros.Add("@pnFacturaID", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parametros.Add("@pnTipoMensaje", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parametros.Add("@pcMensaje", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);

            await _db.ExecuteAsync(
                "dbo.SP_GENERAR_FACTURA",
                parametros,
                commandType: CommandType.StoredProcedure
            );

            var tipoMensaje = parametros.Get<int>("@pnTipoMensaje");
            var mensaje = parametros.Get<string>("@pcMensaje") ?? string.Empty;
            var facturaId = parametros.Get<int>("@pnFacturaID");

            if (tipoMensaje != 0)
            {
                // Hubo error (por ejemplo, reserva no existe)
                return BadRequest(new { tipoMensaje, mensaje });
            }

            // Opcional: devolver también la factura creada
            var factura = await _context.Facturas
                .FirstOrDefaultAsync(f => f.Id == facturaId);

            return Ok(new
            {
                tipoMensaje,
                mensaje,
                facturaId,
                factura
            });
        }

        // ------------------------------------------------------------
        // SP: dbo.SP_OBTENER_FACTURA_COMPLETA
        // GET: api/Facturas/obtener-completa/{id}
        // ------------------------------------------------------------
        [HttpGet("obtener-completa/{id}")]
        public async Task<ActionResult<FacturaCompletaDto>> ObtenerFacturaCompleta(int id)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@pnFacturaID", id);

            using var multi = await _db.QueryMultipleAsync(
                "dbo.SP_OBTENER_FACTURA_COMPLETA",
                parametros,
                commandType: CommandType.StoredProcedure
            );

            // 1) Datos generales
            var datosGenerales = await multi.ReadFirstOrDefaultAsync<FacturaCompletaDto>();
            if (datosGenerales == null)
            {
                return NotFound(new { mensaje = "Factura no encontrada o sin datos." });
            }

            // 2) Servicios
            var servicios = (await multi.ReadAsync<FacturaServicioDto>()).ToList();
            // 3) Tours
            var tours = (await multi.ReadAsync<FacturaTourDto>()).ToList();
            // 4) Paquetes
            var paquetes = (await multi.ReadAsync<FacturaPaqueteDto>()).ToList();

            datosGenerales.Servicios = servicios;
            datosGenerales.Tours = tours;
            datosGenerales.Paquetes = paquetes;

            return Ok(datosGenerales);
        }

        // ------------------------------------------------------------
        // SP: dbo.SP_REPORTE_FACTURAS
        // POST: api/Facturas/reporte-facturas
        // Body: { "fechaInicio": "...", "fechaFin": "...", "clienteID": 1 }
        // ------------------------------------------------------------
        [HttpPost("reporte-facturas")]
        public async Task<ActionResult<IEnumerable<ReporteFacturasDto>>> GetReporteFacturas(
            [FromBody] ReporteFacturasFiltroDto filtro)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@pFechaInicio", filtro.FechaInicio);
            parametros.Add("@pFechaFin", filtro.FechaFin);
            parametros.Add("@pClienteID", filtro.ClienteID);

            var datos = await _db.QueryAsync<ReporteFacturasDto>(
                "dbo.SP_REPORTE_FACTURAS",
                parametros,
                commandType: CommandType.StoredProcedure
            );

            return Ok(datos);
        }

         // ------------------------------------------------------------
        // VISTA: dbo.VW_Facturas_Resumen
        // GET: api/Facturas/vista-resumen
        // ------------------------------------------------------------
        [HttpGet("vista-resumen")]
        public async Task<ActionResult<IEnumerable<FacturaResumenDto>>> GetVistaFacturasResumen()
        {
            const string sql = @"
                SELECT FacturaID      = FacturaID,
                       CodigoFactura,
                       Cliente,
                       ReservaID,
                       CodigoReserva,
                       Fecha_Emision = Fecha_Emision,
                       TotalFactura,
                       Estado,
                       Anio,
                       Mes,
                       MesNombre
                FROM dbo.VW_Facturas_Resumen";

            var datos = await _db.QueryAsync<FacturaResumenDto>(sql);
            return Ok(datos);
        }
    }
}
