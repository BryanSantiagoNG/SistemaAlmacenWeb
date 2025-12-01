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

        // --- CORRECCIÓN AQUÍ ---
        [Display(Name = "Proveedor")]
        public int IdProveedor { get; set; } // La columna int de la BD

        [ForeignKey("IdProveedor")] // <--- ESTA LÍNEA ES LA QUE ARREGLA EL ERROR
        public Proveedor? Proveedor { get; set; } // El objeto de navegación
        // -----------------------

        public ICollection<DetallePedido>? DetallePedidos { get; set; }
    }
}