using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaAlmacenWeb.Models;

namespace SistemaAlmacenWeb.Controllers
{
    public class ArticulosController : Controller
    {
        private readonly SistemaAlmacenContext _context;

        public ArticulosController(SistemaAlmacenContext context)
        {
            _context = context;
        }

        // GET: Articulos
        // Muestra la lista completa incluyendo los nombres de Proveedor y Distribuidor
        public async Task<IActionResult> Index()
        {
            var sistemaAlmacenContext = _context.Articulos
                .Include(a => a.Distribuidor)
                .Include(a => a.Proveedor);
            return View(await sistemaAlmacenContext.ToListAsync());
        }

        // GET: Articulos/Details/5
        // Muestra el detalle de un solo artículo
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Articulos == null)
            {
                return NotFound();
            }

            var articulo = await _context.Articulos
                .Include(a => a.Distribuidor)
                .Include(a => a.Proveedor)
                .FirstOrDefaultAsync(m => m.IdArticulo == id);

            if (articulo == null)
            {
                return NotFound();
            }

            return View(articulo);
        }

        // GET: Articulos/Create
        // Muestra el formulario vacío
        public IActionResult Create()
        {
            // Llenamos los "ViewBag" para los menús desplegables (ComboBox)
            // El primer parámetro es la lista, el segundo el ID (valor), el tercero el Nombre (texto a mostrar)

            // Nota: Usamos _context.Set<Distribuidor>() por si no tienes el DbSet declarado explícitamente, 
            // pero si tienes "public DbSet<Distribuidor> Distribuidores" usa _context.Distribuidores
            ViewData["IdDistribuidor"] = new SelectList(_context.Set<Distribuidor>(), "IdDistribuidor", "Nombre");
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores, "IdProveedor", "Nombre");

            return View();
        }

        // POST: Articulos/Create
        // Recibe los datos del formulario para guardar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdArticulo,CodigoInterno,CodigoBarras,Descripcion,Marca,Cantidad,IdProveedor,IdDistribuidor,PrecioCompra,PrecioVenta")] Articulo articulo)
        {
            // Quitamos la validación de las propiedades de navegación para evitar errores falsos
            // (El sistema a veces se queja de que el objeto "Proveedor" es null, aunque tengamos el "IdProveedor")
            ModelState.Remove("Proveedor");
            ModelState.Remove("Distribuidor");

            if (ModelState.IsValid)
            {
                _context.Add(articulo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Si algo falla, recargamos las listas para que el usuario no pierda el menú
            ViewData["IdDistribuidor"] = new SelectList(_context.Set<Distribuidor>(), "IdDistribuidor", "Nombre", articulo.IdDistribuidor);
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores, "IdProveedor", "Nombre", articulo.IdProveedor);
            return View(articulo);
        }

        // GET: Articulos/Edit/5
        // Muestra el formulario con los datos cargados para editar
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Articulos == null)
            {
                return NotFound();
            }

            var articulo = await _context.Articulos.FindAsync(id);
            if (articulo == null)
            {
                return NotFound();
            }

            // Seleccionamos el valor actual en los menús desplegables
            ViewData["IdDistribuidor"] = new SelectList(_context.Set<Distribuidor>(), "IdDistribuidor", "Nombre", articulo.IdDistribuidor);
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores, "IdProveedor", "Nombre", articulo.IdProveedor);
            return View(articulo);
        }

        // POST: Articulos/Edit/5
        // Guarda los cambios de la edición
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdArticulo,CodigoInterno,CodigoBarras,Descripcion,Marca,Cantidad,IdProveedor,IdDistribuidor,PrecioCompra,PrecioVenta")] Articulo articulo)
        {
            if (id != articulo.IdArticulo)
            {
                return NotFound();
            }

            ModelState.Remove("Proveedor");
            ModelState.Remove("Distribuidor");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(articulo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticuloExists(articulo.IdArticulo))
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
            ViewData["IdDistribuidor"] = new SelectList(_context.Set<Distribuidor>(), "IdDistribuidor", "Nombre", articulo.IdDistribuidor);
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores, "IdProveedor", "Nombre", articulo.IdProveedor);
            return View(articulo);
        }

        // GET: Articulos/Delete/5
        // Muestra la pantalla de confirmación de borrado
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Articulos == null)
            {
                return NotFound();
            }

            var articulo = await _context.Articulos
                .Include(a => a.Distribuidor)
                .Include(a => a.Proveedor)
                .FirstOrDefaultAsync(m => m.IdArticulo == id);
            if (articulo == null)
            {
                return NotFound();
            }

            return View(articulo);
        }

        // POST: Articulos/Delete/5
        // Ejecuta el borrado real
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Articulos == null)
            {
                return Problem("Entity set 'SistemaAlmacenContext.Articulos' is null.");
            }
            var articulo = await _context.Articulos.FindAsync(id);
            if (articulo != null)
            {
                _context.Articulos.Remove(articulo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticuloExists(int id)
        {
            return (_context.Articulos?.Any(e => e.IdArticulo == id)).GetValueOrDefault();
        }
    }
}