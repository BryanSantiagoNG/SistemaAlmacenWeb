using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaAlmacenWeb.Models;

namespace SistemaAlmacenWeb.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly SistemaAlmacenContext _context;

        public UsuariosController(SistemaAlmacenContext context)
        {
            _context = context;
        }

        private bool EsAdmin()
        {
            var rol = HttpContext.Session.GetString("Rol");
            return rol == "Administrador";
        }

        public async Task<IActionResult> Index()
        {
            if (!EsAdmin()) return RedirectToAction("Index", "Home"); 

            return View(await _context.Usuarios.ToListAsync());
        }

        public IActionResult Create()
        {
            if (!EsAdmin()) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                if (_context.Usuarios.Any(u => u.UsuarioNombre == usuario.UsuarioNombre))
                {
                    ModelState.AddModelError("UsuarioNombre", "Este usuario ya existe.");
                    return View(usuario);
                }

                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (!EsAdmin()) return RedirectToAction("Index", "Home");
            if (id == null) return NotFound();

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            return View(usuario);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Usuario usuario)
        {
            if (id != usuario.IdUsuario) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Usuarios.Any(e => e.IdUsuario == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (!EsAdmin()) return RedirectToAction("Index", "Home");
            if (id == null) return NotFound();

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuario == null) return NotFound();

            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                var miId = HttpContext.Session.GetString("UsuarioId");
                if (miId == id.ToString())
                {
                    TempData["Error"] = "No puedes eliminar tu propia cuenta.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}