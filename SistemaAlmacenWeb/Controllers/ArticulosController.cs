using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaAlmacenWeb.Models;
using Microsoft.AspNetCore.Http;

namespace SistemaAlmacenWeb.Controllers
{
    public class ArticulosController : Controller
    {
        private readonly SistemaAlmacenContext _context;

        public ArticulosController(SistemaAlmacenContext context)
        {
            _context = context;
        }

        private bool EsAdmin()
        {
            return HttpContext.Session.GetString("Rol") == "Administrador";
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetJson()
        {
            var articulos = await _context.Articulos
                .Include(a => a.Proveedor)
                .Select(a => new {
                    idArticulo = a.IdArticulo,
                    codigo = a.CodigoInterno,
                    codigoBarras = a.CodigoBarras,
                    descripcion = a.Descripcion,
                    marca = a.Marca,
                    stock = a.Cantidad,
                    precioVenta = a.PrecioVenta,
                    proveedor = a.Proveedor != null ? a.Proveedor.Nombre : "Sin Asignar"
                })
                .ToListAsync();
            return Json(articulos);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var articulo = await _context.Articulos.Include(a => a.Proveedor).Include(a => a.Distribuidor).FirstOrDefaultAsync(m => m.IdArticulo == id);
            if (articulo == null) return NotFound();
            return View(articulo);
        }


        public IActionResult Create()
        {
            if (!EsAdmin()) return RedirectToAction(nameof(Index)); 

            ViewData["IdDistribuidor"] = new SelectList(_context.Set<Distribuidor>(), "IdDistribuidor", "Nombre");
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores, "IdProveedor", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Articulo articulo)
        {
            if (!EsAdmin()) return RedirectToAction(nameof(Index)); 
            ModelState.Remove("Proveedor");
            ModelState.Remove("Distribuidor");

            if (ModelState.IsValid)
            {
                _context.Add(articulo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdDistribuidor"] = new SelectList(_context.Set<Distribuidor>(), "IdDistribuidor", "Nombre", articulo.IdDistribuidor);
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores, "IdProveedor", "Nombre", articulo.IdProveedor);
            return View(articulo);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (!EsAdmin()) return RedirectToAction(nameof(Index));

            if (id == null) return NotFound();
            var articulo = await _context.Articulos.FindAsync(id);
            if (articulo == null) return NotFound();
            ViewData["IdDistribuidor"] = new SelectList(_context.Set<Distribuidor>(), "IdDistribuidor", "Nombre", articulo.IdDistribuidor);
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores, "IdProveedor", "Nombre", articulo.IdProveedor);
            return View(articulo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Articulo articulo)
        {
            if (!EsAdmin()) return RedirectToAction(nameof(Index));

            if (id != articulo.IdArticulo) return NotFound();
            ModelState.Remove("Proveedor");
            ModelState.Remove("Distribuidor");

            if (ModelState.IsValid)
            {
                _context.Update(articulo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdDistribuidor"] = new SelectList(_context.Set<Distribuidor>(), "IdDistribuidor", "Nombre", articulo.IdDistribuidor);
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores, "IdProveedor", "Nombre", articulo.IdProveedor);
            return View(articulo);
        }

        [HttpPost]
        [Route("Articulos/DeleteConfirmedApi/{id}")]
        public async Task<IActionResult> DeleteConfirmedApi(int id)
        {
            if (!EsAdmin()) return Unauthorized();

            var articulo = await _context.Articulos.FindAsync(id);
            if (articulo != null)
            {
                _context.Articulos.Remove(articulo);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }
    }
}