using Microsoft.EntityFrameworkCore;

namespace SistemaAlmacenWeb.Models
{
    public class SistemaAlmacenContext : DbContext
    {
        // ESTE CONSTRUCTOR ES OBLIGATORIO PARA QUE FUNCIONE EL GENERADOR
        public SistemaAlmacenContext(DbContextOptions<SistemaAlmacenContext> options)
            : base(options)
        {
        }

        // Si tienes un método 'OnConfiguring' aquí abajo, BÓRRALO o COMÉNTALO.
        // La conexión ya se está pasando desde Program.cs

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