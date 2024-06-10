using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PeDeOutro.Data;
using PeDeOutro.Models;

namespace PeDeOutro.Controllers
{
    public class PedidosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PedidosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pedidos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Pedido.Include(p => p.Cliente);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Pedidos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedido
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            var itensPedido = await _context.ItensPedido
                .Include(i => i.Produto)
                .Where(i => i.PedidoId == id)
                .ToListAsync();
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // GET: Pedidos/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Nome");
            ViewData["ProdutoId"] = _context.Produto.ToList();
            return View();
        }

        // POST: Pedidos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Total,ClienteId")] Pedido pedido, int[] ItensPedido)
        {
            if (ModelState.IsValid)
            {
                // Calcula o total novamente, caso o usuário tenha alterado manualmente
                pedido.Total = 0;
                foreach (var produtoId in ItensPedido)
                {
                    var produto = await _context.Produto.FindAsync(produtoId);
                    if (produto != null)
                    {
                        pedido.Total += produto.Preco;
                        // Adiciona o produto ao pedido
                        pedido.ItensPedido.Add(new ItensPedido { ProdutoId = produtoId });
                    }
                }

                // Adiciona o pedido ao contexto
                _context.Add(pedido);
                
                // Salva as alterações no banco de dados
                await _context.SaveChangesAsync();
                
                // Redireciona para a página de listagem de pedidos
                return RedirectToAction(nameof(Index));
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Id", pedido.ClienteId);
            ViewData["ProdutoId"] = _context.Produto.ToList();
            return View(pedido);
        }

        // GET: Pedidos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedido.FindAsync(id);

            if (pedido == null)
            {
                return NotFound();
            }
            
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Nome", pedido.ClienteId);
            ViewData["Produtos"] = _context.Produto.ToList();
            ViewData["ItensPedido"] = _context.ItensPedido.Where(i => i.PedidoId == id).ToList();
            return View(pedido);
        }

        // POST: Pedidos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Total,ClienteId")] Pedido pedido, int[] ItensPedido)
        {
            if (id != pedido.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Remove os itens do pedido existentes
                    var itensPedidoDB = await _context.ItensPedido.Where(ip => ip.PedidoId == id).ToListAsync();
                    _context.ItensPedido.RemoveRange(itensPedidoDB);

                    // Adiciona os novos itens do pedido com base nos produtos selecionados
                    foreach (var produtoId in ItensPedido)
                    {
                        _context.ItensPedido.Add(new ItensPedido { PedidoId = id, ProdutoId = produtoId });
                    }

                    // Recalcula o total do pedido com base nos produtos selecionados
                    decimal total = 0;
                    foreach (var produtoId in ItensPedido)
                    {
                        var produto = await _context.Produto.FindAsync(produtoId);
                        if (produto != null)
                        {
                            total += produto.Preco;
                        }
                    }

                    pedido.Total = total;

                    _context.Update(pedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoExists(pedido.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Id", pedido.ClienteId);
            ViewData["Produtos"] = _context.Produto.ToList();
            ViewData["ItensPedido"] = _context.ItensPedido.Where(i => i.PedidoId == id).ToList();
            return View(pedido);
        }

        // GET: Pedidos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedido
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            var itensPedido = await _context.ItensPedido
                .Include(i => i.Produto)
                .Where(i => i.PedidoId == id)
                .ToListAsync();
            if (pedido == null)
            {
                return NotFound();
            }
            return View(pedido);
        }

        // POST: Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedido = await _context.Pedido.FindAsync(id);
            if (pedido != null)
            {
                _context.Pedido.Remove(pedido);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PedidoExists(int id)
        {
            return _context.Pedido.Any(e => e.Id == id);
        }
    }
}
