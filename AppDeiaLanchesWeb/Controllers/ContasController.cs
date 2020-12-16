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
    public class ContasController : ControllerBase
    {
        private readonly LanchesDbContext _context;

        public ContasController(LanchesDbContext context)
        {
            _context = context;
        }

        // GET: api/Contas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Conta>>> GetContas()
        {
            return await _context.Contas.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<Conta> GetContaAsync(int id)
        {
            Conta conta = await _context.Contas.FindAsync(id);

            if (conta == null)
            {
                return null;
            }

            return conta;
        }

        //Confere quais pedidos existe em uma conta
        [HttpGet("PesquisarPedidos/{id}")]
        public async Task<List<Pedido>> GetPedidosOfContasAsync(int id)
        {
            List<ContasPedido> contasPedidos = new List<ContasPedido>();
            contasPedidos = _context.ContasPedidos.ToList();

            PedidosController pc = new PedidosController(_context);

            List<Pedido> pedidos = new List<Pedido>();

            foreach (ContasPedido cp in contasPedidos)
            {
                if (cp.IdConta == id)
                {
                    pedidos.Add(await pc.GetPedido(cp.IdPedido));
                }
            }

            return pedidos;
        }

        //Adiciona Pedidos a uma Conta vinculando na entidade de relacionamento
        [HttpPut("AdicionarPedido/{id}")]
        public async Task<ActionResult<List<Pedido>>> AddPedidoToContaAsync(int id, Conta conta1)
        {
            Conta conta = await GetContaAsync(id);

            PedidosController pc = new PedidosController(_context);
            Pedido pedido = await pc.GetPedido(conta1.IdPedido);

            conta.Valor += pedido.Valor;

            ContasPedidosController cpc = new ContasPedidosController(_context);

            ContasPedido contasPedido = new ContasPedido();
            contasPedido.IdConta = id;
            contasPedido.IdPedido = conta1.IdPedido;

            await cpc.PostContasPedido(contasPedido);

            List<Pedido> pedidos = await GetPedidosOfContasAsync(id);

            return pedidos;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutConta(int id, Conta conta)
        {
            if (id != conta.Id)
            {
                return BadRequest();
            }

            _context.Entry(conta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContaExists(id))
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

        // POST: api/Contas
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Conta>> PostConta(Conta conta)
        {
            _context.Contas.Add(conta);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConta", new { id = conta.Id }, conta);
        }

        // DELETE: api/Contas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Conta>> DeleteConta(int id)
        {
            var conta = await _context.Contas.FindAsync(id);
            if (conta == null)
            {
                return NotFound();
            }

            _context.Contas.Remove(conta);
            await _context.SaveChangesAsync();

            return conta;
        }

        private bool ContaExists(int id)
        {
            return _context.Contas.Any(e => e.Id == id);
        }
    }
}