using AppDeiaLanchesWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppDeiaLanchesWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContasPedidosController : ControllerBase
    {
        private readonly LanchesDbContext _context;

        public ContasPedidosController(LanchesDbContext context)
        {
            _context = context;
        }

        // GET: api/ContasPedidos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContasPedido>>> GetContasPedidos()
        {
            return await _context.ContasPedidos.ToListAsync();
        }

        // GET: api/ContasPedidos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContasPedido>> GetContasPedido(int id)
        {
            var contasPedido = await _context.ContasPedidos.FindAsync(id);

            if (contasPedido == null)
            {
                return NotFound();
            }

            return contasPedido;
        }

        // PUT: api/ContasPedidos/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContasPedido(int id, ContasPedido contasPedido)
        {
            if (id != contasPedido.Id)
            {
                return BadRequest();
            }

            _context.Entry(contasPedido).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContasPedidoExists(id))
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

        // POST: api/ContasPedidos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ContasPedido>> PostContasPedido(ContasPedido contasPedido)
        {
            _context.ContasPedidos.Add(contasPedido);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContasPedido", new { id = contasPedido.Id }, contasPedido);
        }

        // DELETE: api/ContasPedidos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ContasPedido>> DeleteContasPedido(int id)
        {
            var contasPedido = await _context.ContasPedidos.FindAsync(id);
            if (contasPedido == null)
            {
                return NotFound();
            }

            _context.ContasPedidos.Remove(contasPedido);
            await _context.SaveChangesAsync();

            return contasPedido;
        }

        private bool ContasPedidoExists(int id)
        {
            return _context.ContasPedidos.Any(e => e.Id == id);
        }
    }
}