using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PeDeOuro.Data;
using PeDeOuro.Models;

namespace PeDeOuro.Controllers
{
    public class EmailsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly string _emailSerciceUrl;

        public EmailsController(ApplicationDbContext context,HttpClient httpClient, IConfiguration configuration)
        {
            _context = context;
            _httpClient = httpClient;
            _emailSerciceUrl = configuration["VariablesEmailService:host"] + ":" + configuration["VariablesEmailService:port"];
        }

        // GET: Emails
        public async Task<IActionResult> Index()
        {
            // var applicationDbContext = _context.Emails.Include(e => e.Promocao);
            // return View(await applicationDbContext.ToListAsync());
            return View();
        }

        // GET: Emails/Details/5
        // public async Task<IActionResult> Details(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var emails = await _context.Emails
        //         .Include(e => e.Promocao)
        //         .FirstOrDefaultAsync(m => m.Id == id);
        //     if (emails == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(emails);
        // }

        // GET: Emails/Create
        public async Task<IActionResult> Create()
        {   
            var promocoes = await _httpClient.GetFromJsonAsync<IEnumerable<Promocao>>($"{_emailSerciceUrl}/api/promocoes");
            var clientes = await _context.Cliente.ToListAsync();
            ViewData["PromocaoId"] = new SelectList(promocoes, "Id", "Assunto");
            ViewData["ClienteId"] = clientes;
            return View();
        }

        // POST: Emails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Emails emails, List<string> SelectedClientes)
        {   
            bool valid = false;
            if (SelectedClientes.Count > 0 && emails.PromocaoId != 0)
            {
                valid = true;
            }
            if (valid)
            {
                try
                {
                    var promocao = await _httpClient.GetFromJsonAsync<Promocao>($"{_emailSerciceUrl}/api/promocoes/{emails.PromocaoId}");
                    
                    // Processar os emails dos clientes selecionados
                    foreach (var emailCliente in SelectedClientes)
                    {
                        var email = new Emails
                        {
                            PromocaoId = emails.PromocaoId,
                            emailDestinatario = emailCliente,
                        };
                        Console.WriteLine(email);
                        var response = await _httpClient.PostAsJsonAsync($"{_emailSerciceUrl}/api/emails", email);
                        response.EnsureSuccessStatusCode();
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (HttpRequestException)
                {
                    // Tratar o erro conforme necessário
                    ModelState.AddModelError("", "Erro ao enviar email.");
                    return View(emails);
                }
            }
            var promocoes = await _httpClient.GetFromJsonAsync<IEnumerable<Promocao>>($"{_emailSerciceUrl}/api/promocoes");
            var clientes = await _context.Cliente.ToListAsync();
            ViewData["PromocaoId"] = new SelectList(promocoes, "Id", "Assunto");
            ViewData["ClienteId"] = clientes;
            return View(emails);
        }

        // GET: Emails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emails = await _context.Emails.FindAsync(id);
            if (emails == null)
            {
                return NotFound();
            }
            ViewData["PromocaoId"] = new SelectList(_context.Promocao, "Id", "Assunto", emails.PromocaoId);
            return View(emails);
        }

        // POST: Emails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PromocaoId,emailDestinatario")] Emails emails)
        {
            if (id != emails.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(emails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmailsExists(emails.Id))
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
            ViewData["PromocaoId"] = new SelectList(_context.Promocao, "Id", "Assunto", emails.PromocaoId);
            return View(emails);
        }

        // GET: Emails/Delete/5
        // public async Task<IActionResult> Delete(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var emails = await _context.Emails
        //         .Include(e => e.Promocao)
        //         .FirstOrDefaultAsync(m => m.Id == id);
        //     if (emails == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(emails);
        // }

        // POST: Emails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emails = await _context.Emails.FindAsync(id);
            if (emails != null)
            {
                _context.Emails.Remove(emails);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmailsExists(int id)
        {
            return _context.Emails.Any(e => e.Id == id);
        }
    }
}
