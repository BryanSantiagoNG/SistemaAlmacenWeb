using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaAlmacenWeb.Models
{
    [Table("Proveedores")]
    public class Proveedor
    {
        [Key]
        public int IdProveedor { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [MaxLength(20)]
        public string? Telefono { get; set; } 

        [MaxLength(200)]
        public string? Direccion { get; set; } 

        [MaxLength(100)]
        public string? Email { get; set; } 

        public ICollection<Articulo>? Articulos { get; set; }
        public ICollection<Pedido>? Pedidos { get; set; }
    }
}