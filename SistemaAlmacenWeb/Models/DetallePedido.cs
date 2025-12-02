using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaAlmacenWeb.Models
{
    [Table("DetallePedidos")]
    public class DetallePedido
    {
        [Key]
        public int IdDetallePedido { get; set; }

        [Required]
        [Display(Name = "Cantidad Solicitada")]
        public int Cantidad { get; set; }

        [Display(Name = "Costo Unitario")]
        [Column(TypeName = "decimal(10, 2)")]
        [DataType(DataType.Currency)]
        public decimal PrecioUnitario { get; set; }

        public int IdPedido { get; set; }

        [ForeignKey("IdPedido")]
        public Pedido Pedido { get; set; }

        public int IdArticulo { get; set; }

        [ForeignKey("IdArticulo")]
        public Articulo Articulo { get; set; }
    }
}