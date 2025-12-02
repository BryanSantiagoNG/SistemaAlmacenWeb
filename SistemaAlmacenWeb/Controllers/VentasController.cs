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

        public IActionResult Index()
        {
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "Nombre");

            var articulos = _context.Articulos
                .Where(a => a.Cantidad > 0)
                .Select(a => new {
                    Id = a.IdArticulo,
                    Nombre = $"{a.Descripcion} - ${a.PrecioVenta} (Disp: {a.Cantidad})"
                })
                .ToList();

            ViewData["IdArticulo"] = new SelectList(articulos, "Id", "Nombre");

            return View();
        }
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

        [HttpPost]
        public async Task<IActionResult> ProcesarVenta([FromBody] VentaDTO ventaData)
        {
            if (ventaData.Detalles == null || ventaData.Detalles.Count == 0)
                return BadRequest("El carrito está vacío.");

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var nuevaFactura = new Factura
                    {
                        Fecha = DateTime.Now,
                        IdCliente = ventaData.IdCliente,
                        Total = 0 
                    };

                    _context.Facturas.Add(nuevaFactura);
                    await _context.SaveChangesAsync();

                    decimal totalCalculado = 0;

                    foreach (var item in ventaData.Detalles)
                    {
                        var articuloDB = await _context.Articulos.FindAsync(item.IdArticulo);

                        if (articuloDB == null)
                            throw new Exception($"Artículo ID {item.IdArticulo} no encontrado.");

                        if (articuloDB.Cantidad < item.Cantidad)
                            throw new Exception($"Stock insuficiente para {articuloDB.Descripcion}. Stock actual: {articuloDB.Cantidad}");

                        var detalle = new DetalleFactura
                        {
                            IdFactura = nuevaFactura.IdFactura,
                            IdArticulo = item.IdArticulo,
                            Cantidad = item.Cantidad,
                            PrecioUnitario = articuloDB.PrecioVenta 
                        };

                        _context.DetalleFacturas.Add(detalle);

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