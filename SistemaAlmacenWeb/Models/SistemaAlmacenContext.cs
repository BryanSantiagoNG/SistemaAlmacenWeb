using Microsoft.EntityFrameworkCore;

namespace SistemaAlmacenWeb.Models
{
    public class SistemaAlmacenContext : DbContext
    {
        public SistemaAlmacenContext(DbContextOptions<SistemaAlmacenContext> options)
            : base(options)
        {
        }


        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Articulo> Articulos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<DetallePedido> DetallePedidos { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<DetalleFactura> DetalleFacturas { get; set; }
    }
}