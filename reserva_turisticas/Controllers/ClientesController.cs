using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class ClientesController : ControllerBase
    {
        private readonly ReservaTuristicaContext _context;
        private readonly IDbConnection _db;   // igual que en ReservasController

        public ClientesController(ReservaTuristicaContext context, IDbConnection db)
        {
            _context = context;
            _db = db;
        }

        // GET: api/Clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            return await _context.Clientes.ToListAsync();
        }

        // GET: api/Clientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        // PUT: api/Clientes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest();
            }

            _context.Entry(cliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
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

        // POST: api/Clientes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCliente", new { id = cliente.Id }, cliente);
        }

        // DELETE: api/Clientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }

        // ------------------------------------------------------------
        // 1) Vista: dbo.VW_Clientes_Detalle
        // GET: api/Clientes/vista-detalle
        // ------------------------------------------------------------
        [HttpGet("vista-detalle")]
        public async Task<ActionResult<IEnumerable<ClienteDetalleDto>>> GetVistaClientesDetalle()
        {
            const string sql = @"SELECT ClienteID,
                                        Codigo_cliente,
                                        Fecha_registro,
                                        DNI,
                                        Primer_nombre,
                                        Segundo_nombre,
                                        Primer_apellido,
                                        Segundo_apellido,
                                        Correo_electronico,
                                        Direccion,
                                        Categoria,
                                        Prioridad
                                 FROM dbo.VW_Clientes_Detalle";

            var datos = await _db.QueryAsync<ClienteDetalleDto>(sql);
            return Ok(datos);
        }

        // ------------------------------------------------------------
        // 2) Vista: dbo.VW_Clientes_Gestion
        // GET: api/Clientes/vista-gestion
        // ------------------------------------------------------------
        [HttpGet("vista-gestion")]
        public async Task<ActionResult<IEnumerable<ClienteGestionDto>>> GetVistaClientesGestion()
        {
            const string sql = @"SELECT ClienteID,
                                        CodigoCliente,
                                        NombreCompleto,
                                        Email,
                                        Telefono,
                                        Fecha_registro,
                                        Categoria,
                                        Prioridad,
                                        CategoriaCliente_id,
                                        Estado
                                 FROM dbo.VW_Clientes_Gestion";

            var datos = await _db.QueryAsync<ClienteGestionDto>(sql);
            return Ok(datos);
        }

        
    }
}
