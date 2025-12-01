using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaAlmacenWeb.Models
{
    [Table("Articulos")]
    public class Articulo
    {
        [Key]
        public int IdArticulo { get; set; }

        [Display(Name = "Código Interno")]
        [MaxLength(50)]
        public string CodigoInterno { get; set; }

        [Display(Name = "Código de Barras")]
        [MaxLength(50)]
        public string CodigoBarras { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [Display(Name = "Descripción del Producto")]
        [MaxLength(200)]
        public string Descripcion { get; set; }

        [Display(Name = "Marca")]
        [MaxLength(100)]
        public string Marca { get; set; }

        [Required]
        [Range(0, 999999, ErrorMessage = "La cantidad no puede ser negativa")]
        [Display(Name = "Stock Actual")]
        public int Cantidad { get; set; }

        // --- RELACIÓN CON PROVEEDORES ---
        [Display(Name = "Proveedor")]
        public int? IdProveedor { get; set; }

        [ForeignKey("IdProveedor")]
        public Proveedor Proveedor { get; set; }

        // --- RELACIÓN CON DISTRIBUIDORES ---
        [Display(Name = "Distribuidor")]
        public int? IdDistribuidor { get; set; }

        [ForeignKey("IdDistribuidor")]
        public Distribuidor Distribuidor { get; set; }

        // --- PRECIOS ---
        [Display(Name = "Precio de Compra")]
        [Column(TypeName = "decimal(10, 2)")] // Define el formato en SQL
        [DataType(DataType.Currency)]         // Le dice a la web que esto es dinero ($)
        public decimal PrecioCompra { get; set; }

        [Display(Name = "Precio de Venta")]
        [Column(TypeName = "decimal(10, 2)")]
        [DataType(DataType.Currency)]
        public decimal PrecioVenta { get; set; }
    }
}