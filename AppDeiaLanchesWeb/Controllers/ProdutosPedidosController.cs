using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppDeiaLanchesWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosPedidosController : ControllerBase
    {
        private readonly LanchesDbContext _context;

        public ProdutosPedidosController(LanchesDbContext context)
        {
            _context = context;
        }

        // GET: api/ProdutosPedidos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutosPedido>>> GetProdutosPedidos()
        {
            return await _context.ProdutosPedidos.ToListAsync();
        }

        // GET: api/ProdutosPedidos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutosPedido>> GetProdutosPedido(int id)
        {
            var produtosPedido = await _context.ProdutosPedidos.FindAsync(id);

            if (produtosPedido == null)
            {
                return NotFound();
            }

            return produtosPedido;
        }

        // PUT: api/ProdutosPedidos/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProdutosPedido(int id, ProdutosPedido produtosPedido)
        {
            if (id != produtosPedido.IdPedido)
            {
                return BadRequest();
            }

            _context.Entry(produtosPedido).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutosPedidoExists(id))
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

        // POST: api/ProdutosPedidos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ProdutosPedido>> PostProdutosPedido(ProdutosPedido produtosPedido)
        {
            _context.ProdutosPedidos.Add(produtosPedido);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProdutosPedidoExists(produtosPedido.IdPedido))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProdutosPedido", new { id = produtosPedido.IdPedido }, produtosPedido);
        }

        // DELETE: api/ProdutosPedidos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProdutosPedido>> DeleteProdutosPedido(int id)
        {
            var produtosPedido = await _context.ProdutosPedidos.FindAsync(id);
            if (produtosPedido == null)
            {
                return NotFound();
            }

            _context.ProdutosPedidos.Remove(produtosPedido);
            await _context.SaveChangesAsync();

            return produtosPedido;
        }

        private bool ProdutosPedidoExists(int id)
        {
            return _context.ProdutosPedidos.Any(e => e.IdPedido == id);
        }
    }
}