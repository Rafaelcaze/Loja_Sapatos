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
    public class PromocaoController : Controller
    {

        private readonly HttpClient _httpClient;
        private readonly string _emailSerciceUrl;

        public PromocaoController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _emailSerciceUrl = configuration["VariablesEmailService:host"] + ":" + configuration["VariablesEmailService:port"];
        }

        // GET: Promocao
        public async Task<IActionResult> Index()
        {
            IEnumerable<Promocao> promocoes;

            try
            {
                promocoes = await _httpClient.GetFromJsonAsync<IEnumerable<Promocao>>($"{_emailSerciceUrl}/api/promocoes");
            }
            catch (HttpRequestException)
            {
                promocoes = new List<Promocao>(); 
            }

            return View(promocoes);
        }

        // GET: Promocao/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Promocao promocao;

            try
            {
                promocao = await _httpClient.GetFromJsonAsync<Promocao>($"{_emailSerciceUrl}/api/promocoes/{id}");
            }
            catch (HttpRequestException)
            {
                return NotFound();
            }

            if (promocao == null)
            {
                return NotFound();
            }

            return View(promocao);
        }

        // GET: Promocao/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Promocao/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Assunto,Descricao")] Promocao promocao)
        {
           if (ModelState.IsValid)
            {
                try
                {
                    var response = await _httpClient.PostAsJsonAsync(_emailSerciceUrl + "/api/promocoes", promocao);
                    response.EnsureSuccessStatusCode();
                    return RedirectToAction(nameof(Index));
                }
                catch (HttpRequestException)
                {
                    // Tratar o erro conforme necessário
                }
            }
            return View(promocao);
        }

        // GET: Promocao/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
           if (id == null)
            {
                return NotFound();
            }

            Promocao promocao;

            try
            {
                promocao = await _httpClient.GetFromJsonAsync<Promocao>($"{_emailSerciceUrl}/api/promocoes/{id}");
            }
            catch (HttpRequestException)
            {
                return NotFound();
            }

            if (promocao == null)
            {
                return NotFound();
            }

            return View(promocao);
        }

        // POST: Promocao/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Assunto,Descricao")] Promocao promocao)
        {
            if (id != promocao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _httpClient.PutAsJsonAsync($"{_emailSerciceUrl}/api/promocoes/{id}", promocao);
                    response.EnsureSuccessStatusCode();
                    return RedirectToAction(nameof(Index));
                }
                catch (HttpRequestException)
                {
                    // Tratar o erro conforme necessário

                }
            }
            return View(promocao);
        }

        // GET: Promocao/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Promocao promocao;

            try
            {
                promocao = await _httpClient.GetFromJsonAsync<Promocao>($"{_emailSerciceUrl}/api/promocoes/{id}");
            }
            catch (HttpRequestException)
            {
                return NotFound();
            }

            if (promocao == null)
            {
                return NotFound();
            }

            return View(promocao);
        }

        // POST: Promocao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           try
            {
                var response = await _httpClient.DeleteAsync($"{_emailSerciceUrl}/api/promocoes/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException)
            {
                // Tratar o erro conforme necessário
            }

            return RedirectToAction(nameof(Index));
        }

        // private bool PromocaoExists(int id)
        // {
        //     return _context.Promocao.Any(e => e.Id == id);
        // }
    }
}
