using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaAlmacenWeb.Models;
using Microsoft.AspNetCore.Http;

namespace SistemaAlmacenWeb.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly SistemaAlmacenContext _context;

        public ProveedoresController(SistemaAlmacenContext context)
        {
            _context = context;
        }

        private bool EsAdmin() => HttpContext.Session.GetString("Rol") == "Administrador";

        public async Task<IActionResult> Index()
        {
            return View(await _context.Proveedores.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var proveedor = await _context.Proveedores.Include(p => p.Articulos).FirstOrDefaultAsync(m => m.IdProveedor == id);
            if (proveedor == null) return NotFound();
            return View(proveedor);
        }


        public IActionResult Create()
        {
            if (!EsAdmin()) return RedirectToAction(nameof(Index));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Proveedor proveedor)
        {
            if (!EsAdmin()) return RedirectToAction(nameof(Index));
            if (ModelState.IsValid)
            {
                _context.Add(proveedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proveedor);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (!EsAdmin()) return RedirectToAction(nameof(Index));
            if (id == null) return NotFound();
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null) return NotFound();
            return View(proveedor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Proveedor proveedor)
        {
            if (!EsAdmin()) return RedirectToAction(nameof(Index));
            if (id != proveedor.IdProveedor) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(proveedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proveedor);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!EsAdmin()) return RedirectToAction(nameof(Index));
            if (id == null) return NotFound();
            var proveedor = await _context.Proveedores.FirstOrDefaultAsync(m => m.IdProveedor == id);
            if (proveedor == null) return NotFound();
            return View(proveedor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!EsAdmin()) return RedirectToAction(nameof(Index));
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor != null)
            {
                _context.Proveedores.Remove(proveedor);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}