using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaAlmacenWeb.Models;
using Microsoft.AspNetCore.Http;

namespace SistemaAlmacenWeb.Controllers
{
    public class ClientesController : Controller
    {
        private readonly SistemaAlmacenContext _context;

        public ClientesController(SistemaAlmacenContext context)
        {
            _context = context;
        }

        private bool EsAdmin() => HttpContext.Session.GetString("Rol") == "Administrador";

        public async Task<IActionResult> Index()
        {
            return View(await _context.Clientes.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var cliente = await _context.Clientes.Include(c => c.Facturas).FirstOrDefaultAsync(m => m.IdCliente == id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        public IActionResult Create()
        {
            if (!EsAdmin()) return RedirectToAction(nameof(Index));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (!EsAdmin()) return RedirectToAction(nameof(Index));
            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (!EsAdmin()) return RedirectToAction(nameof(Index));
            if (id == null) return NotFound();
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cliente cliente)
        {
            if (!EsAdmin()) return RedirectToAction(nameof(Index));
            if (id != cliente.IdCliente) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!EsAdmin()) return RedirectToAction(nameof(Index));
            if (id == null) return NotFound();
            var cliente = await _context.Clientes.FirstOrDefaultAsync(m => m.IdCliente == id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!EsAdmin()) return RedirectToAction(nameof(Index));
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}