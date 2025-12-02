using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaAlmacenWeb.Models
{
    [Table("Pedidos")]
    public class Pedido
    {
        [Key]
        public int IdPedido { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha del Pedido")]
        public DateTime Fecha { get; set; }

        [MaxLength(30)]
        [Display(Name = "Tipo")]
        public string? TipoPedido { get; set; }

        [MaxLength(30)]
        [Display(Name = "Estado")]
        public string? Estado { get; set; }

        [Display(Name = "Proveedor")]
        public int IdProveedor { get; set; } 

        [ForeignKey("IdProveedor")] 
        public Proveedor? Proveedor { get; set; }

        public ICollection<DetallePedido>? DetallePedidos { get; set; }
    }
}