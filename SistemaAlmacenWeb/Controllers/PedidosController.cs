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
    public class PedidosController : Controller
    {
        private readonly SistemaAlmacenContext _context;

        public PedidosController(SistemaAlmacenContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetJson()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Proveedor)
                .OrderByDescending(p => p.Fecha)
                .Select(p => new {
                    idPedido = p.IdPedido,
                    fecha = p.Fecha.ToString("dd/MM/yyyy"),
                    proveedor = p.Proveedor.Nombre,
                    estado = p.Estado,
                    total = p.DetallePedidos.Sum(d => d.Cantidad * d.PrecioUnitario)
                })
                .ToListAsync();

            return Json(pedidos);
        }

        public IActionResult Create()
        {
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores, "IdProveedor", "Nombre");

            var articulos = _context.Articulos.Select(a => new {
                Id = a.IdArticulo,
                Nombre = $"{a.CodigoInterno} - {a.Descripcion} (Stock: {a.Cantidad})",
                Precio = a.PrecioCompra
            }).ToList();

            ViewBag.Articulos = new SelectList(articulos, "Id", "Nombre");

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetArticulosPorProveedor(int idProveedor)
        {
            var articulos = await _context.Articulos
                .Where(a => a.IdProveedor == idProveedor) 
                .Select(a => new {
                    id = a.IdArticulo,
                    nombre = $"{a.CodigoInterno} - {a.Descripcion} (Stock: {a.Cantidad})",
                    precio = a.PrecioCompra 
                })
                .ToListAsync();

            return Json(articulos);
        }
        [HttpGet]
        public async Task<IActionResult> GetPrecioArticulo(int id)
        {
            var art = await _context.Articulos.FindAsync(id);
            if (art == null) return NotFound();
            return Ok(new { precio = art.PrecioCompra });
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarPedido([FromBody] PedidoDTO pedidoData)
        {
            if (pedidoData.Detalles == null || pedidoData.Detalles.Count == 0)
            {
                return BadRequest("No hay productos en el pedido.");
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var nuevoPedido = new Pedido
                    {
                        Fecha = DateTime.Now,
                        IdProveedor = pedidoData.IdProveedor,
                        TipoPedido = "Compra",
                        Estado = "Completado"
                    };

                    _context.Pedidos.Add(nuevoPedido);
                    await _context.SaveChangesAsync();

                    foreach (var item in pedidoData.Detalles)
                    {
                        var detalle = new DetallePedido
                        {
                            IdPedido = nuevoPedido.IdPedido,
                            IdArticulo = item.IdArticulo,
                            Cantidad = item.Cantidad,
                            PrecioUnitario = item.PrecioUnitario
                        };
                        _context.DetallePedidos.Add(detalle);

                        var articuloDB = await _context.Articulos.FindAsync(item.IdArticulo);
                        if (articuloDB != null)
                        {
                            articuloDB.Cantidad += item.Cantidad; 
                            articuloDB.PrecioCompra = item.PrecioUnitario; 
                        }
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync(); 

                    return Ok(new { message = "Pedido guardado y stock actualizado." });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, "Error interno: " + ex.Message);
                }
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var pedido = await _context.Pedidos
                .Include(p => p.Proveedor)
                .Include(p => p.DetallePedidos)
                .ThenInclude(d => d.Articulo)
                .FirstOrDefaultAsync(m => m.IdPedido == id);

            if (pedido == null) return NotFound();

            return View(pedido);
        }
    }
    public class PedidoDTO
    {
        public int IdProveedor { get; set; }
        public List<DetallePedidoDTO> Detalles { get; set; }
    }
    public class DetallePedidoDTO
    {
        public int IdArticulo { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}