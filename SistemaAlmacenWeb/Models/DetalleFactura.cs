using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaAlmacenWeb.Models
{
    [Table("DetalleFacturas")]
    public class DetalleFactura
    {
        [Key]
        public int IdDetalleFactura { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Display(Name = "Precio Unitario")]
        [Column(TypeName = "decimal(10, 2)")]
        [DataType(DataType.Currency)]
        public decimal PrecioUnitario { get; set; }

        public int IdFactura { get; set; }

        [ForeignKey("IdFactura")]
        public Factura Factura { get; set; }

        public int IdArticulo { get; set; }

        [ForeignKey("IdArticulo")]
        public Articulo Articulo { get; set; }
    }
}