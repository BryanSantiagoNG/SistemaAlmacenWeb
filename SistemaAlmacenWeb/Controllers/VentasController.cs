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
    public class VentasController : Controller
    {
        private readonly SistemaAlmacenContext _context;

        public VentasController(SistemaAlmacenContext context)
        {
            _context = context;
        }

        // 1. VISTA PRINCIPAL (PUNTO DE VENTA)
        public IActionResult Index()
        {
            // Cargar Clientes
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "Nombre");

            // Cargar Artículos (Solo los que tienen stock > 0)
            var articulos = _context.Articulos
                .Where(a => a.Cantidad > 0)
                .Select(a => new {
                    Id = a.IdArticulo,
                    // Texto que se ve en el combo: "Coca Cola - $15.00 (Disp: 100)"
                    Nombre = $"{a.Descripcion} - ${a.PrecioVenta} (Disp: {a.Cantidad})"
                })
                .ToList();

            ViewData["IdArticulo"] = new SelectList(articulos, "Id", "Nombre");

            return View();
        }

        // 2. API: OBTENER DATOS DE UN ARTÍCULO (PRECIO Y STOCK ACTUAL)
        [HttpGet]
        public async Task<IActionResult> GetDatosArticulo(int id)
        {
            var art = await _context.Articulos.FindAsync(id);
            if (art == null) return NotFound();

            return Ok(new
            {
                precio = art.PrecioVenta,
                stock = art.Cantidad,
                codigo = art.CodigoBarras ?? art.CodigoInterno
            });
        }

        // 3. PROCESAR VENTA (GUARDAR Y RESTAR STOCK)
        [HttpPost]
        public async Task<IActionResult> ProcesarVenta([FromBody] VentaDTO ventaData)
        {
            if (ventaData.Detalles == null || ventaData.Detalles.Count == 0)
                return BadRequest("El carrito está vacío.");

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // A) Crear Factura
                    var nuevaFactura = new Factura
                    {
                        Fecha = DateTime.Now,
                        IdCliente = ventaData.IdCliente,
                        Total = 0 // Lo calculamos abajo para seguridad
                    };

                    _context.Facturas.Add(nuevaFactura);
                    await _context.SaveChangesAsync();

                    decimal totalCalculado = 0;

                    // B) Procesar Detalles
                    foreach (var item in ventaData.Detalles)
                    {
                        var articuloDB = await _context.Articulos.FindAsync(item.IdArticulo);

                        if (articuloDB == null)
                            throw new Exception($"Artículo ID {item.IdArticulo} no encontrado.");

                        if (articuloDB.Cantidad < item.Cantidad)
                            throw new Exception($"Stock insuficiente para {articuloDB.Descripcion}. Stock actual: {articuloDB.Cantidad}");

                        // Crear Detalle
                        var detalle = new DetalleFactura
                        {
                            IdFactura = nuevaFactura.IdFactura,
                            IdArticulo = item.IdArticulo,
                            Cantidad = item.Cantidad,
                            PrecioUnitario = articuloDB.PrecioVenta // Usamos precio de BD por seguridad
                        };

                        _context.DetalleFacturas.Add(detalle);

                        // RESTAR STOCK
                        articuloDB.Cantidad -= item.Cantidad;

                        totalCalculado += (detalle.Cantidad * detalle.PrecioUnitario);
                    }

                    nuevaFactura.Total = totalCalculado;
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Ok(new { message = "Venta exitosa", idFactura = nuevaFactura.IdFactura });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, ex.Message);
                }
            }
        }
    }

    // DTOs para recibir datos del JS
    public class VentaDTO
    {
        public int IdCliente { get; set; }
        public List<DetalleVentaDTO> Detalles { get; set; }
    }
    public class DetalleVentaDTO
    {
        public int IdArticulo { get; set; }
        public int Cantidad { get; set; }
    }
}