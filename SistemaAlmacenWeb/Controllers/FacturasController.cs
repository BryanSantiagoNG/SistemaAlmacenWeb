using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaAlmacenWeb.Models;

namespace SistemaAlmacenWeb.Controllers
{
    public class FacturasController : Controller
    {
        private readonly SistemaAlmacenContext _context;

        public FacturasController(SistemaAlmacenContext context)
        {
            _context = context;
        }

        // 1. VISTA PRINCIPAL (HISTORIAL)
        public IActionResult Index()
        {
            return View();
        }

        // 2. DATOS JSON PARA LA TABLA (Carga rápida)
        [HttpGet]
        public async Task<IActionResult> GetJson()
        {
            var facturas = await _context.Facturas
                .Include(f => f.Cliente)
                .OrderByDescending(f => f.Fecha) // Las más recientes primero
                .Select(f => new {
                    idFactura = f.IdFactura,
                    fecha = f.Fecha.ToString("dd/MM/yyyy HH:mm"),
                    cliente = f.Cliente != null ? f.Cliente.Nombre : "Público General",
                    total = f.Total
                })
                .ToListAsync();

            return Json(facturas);
        }

        // 3. VISTA DE DETALLE (EL TICKET)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var factura = await _context.Facturas
                .Include(f => f.Cliente)
                .Include(f => f.DetalleFacturas)
                .ThenInclude(d => d.Articulo)
                .FirstOrDefaultAsync(m => m.IdFactura == id);

            if (factura == null) return NotFound();

            return View(factura);
        }
    }
}