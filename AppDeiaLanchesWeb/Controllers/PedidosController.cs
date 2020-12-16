using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppDeiaLanchesWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly LanchesDbContext _context;

        public PedidosController(LanchesDbContext context)
        {
            _context = context;
        }

        // GET: api/Pedidos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos()
        {
            return await _context.Pedidos.ToListAsync();
        }

        //Pesquisar produtos dos pedidos
        [HttpGet("PesquisarProdutos/{id}")]
        public async Task<ActionResult<List<Produto>>> GetProdutosOfPedidoAsync(int id)
        {
            List<ProdutosPedido> produtosPedidos = new List<ProdutosPedido>();
            produtosPedidos = _context.ProdutosPedidos.ToList();

            ProdutosController pc = new ProdutosController(_context);

            List<Produto> produtos = new List<Produto>();

            foreach (ProdutosPedido pp in produtosPedidos)
            {
                if (pp.IdPedido == id)
                {
                    produtos.Add(await pc.GetProduto(pp.IdProduto));
                }
            }

            return produtos;
        }

        // GET: api/Pedidos/5
        [HttpGet("{id}")]
        public async Task<Pedido> GetPedido(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null)
            {
                return null;
            }

            return pedido;
        }

        [HttpPut("AdicionarProdutos/{id}")]
        public async Task<IActionResult> AdicionarProdutos(int id, Pedido pedido1)
        {
            Pedido pedido = await GetPedido(id);

            //Insere as tabelas de relacionamento Produto-Pedido referente ao pedido(id)
            for (int i = 0; i < pedido1.Produtos.Count; i++)
            {
                ProdutosPedido produtoPedido = new ProdutosPedido();
                produtoPedido.IdProduto = pedido1.Produtos[i];
                produtoPedido.IdPedido = id;

                ProdutosPedidosController ppc = new ProdutosPedidosController(_context);
                await ppc.PostProdutosPedido(produtoPedido);
            }

            //Adiciona o valor do pedido
            for (int i = 0; i < pedido1.Produtos.Count; i++)
            {
                ProdutosController pc = new ProdutosController(_context);
                Produto produto = await pc.GetProduto(pedido1.Produtos[i]);

                //Promoção de Xtudo sexta feira
                if(pedido.DataPedido.DayOfWeek == System.DayOfWeek.Friday && produto.Id == 13)
                {
                    pedido.Valor += (produto.Preco - 3);
                    pedido.Descricao += " | X-TUDO em promoção.";
                }
                else
                {
                    pedido.Valor += produto.Preco;
                }
            }

            _context.Entry(pedido).State = EntityState.Modified;

            try
            {
                 await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PedidoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPedido", new { id = pedido.Id }, pedido);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Pedido>> PutPedido(int id, Pedido pedido)
        {
            if (id != pedido.Id)
            {
                return BadRequest();
            }

            _context.Entry(pedido).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PedidoExists(id))
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

        // POST: api/Pedidos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult<Pedido> PostPedido(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
            _context.SaveChanges();

            return CreatedAtAction("GetPedido", new { id = pedido.Id }, pedido);
        }

        // DELETE: api/Pedidos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Pedido>> DeletePedido(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();

            return pedido;
        }

        private bool PedidoExists(int id)
        {
            return _context.Pedidos.Any(e => e.Id == id);
        }
    }
}