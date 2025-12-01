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

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100)]
        [Display(Name = "Razón Social / Nombre")]
        public string Nombre { get; set; }

        [MaxLength(20)]
        [Display(Name = "Teléfono")]
        [DataType(DataType.PhoneNumber)]
        public string? Telefono { get; set; } // <--- Nullable

        [MaxLength(200)]
        [Display(Name = "Dirección Física")]
        public string? Direccion { get; set; } // <--- Nullable

        [MaxLength(100)]
        [Display(Name = "Correo Electrónico")]
        [EmailAddress]
        public string? Email { get; set; } // <--- Nullable

        public ICollection<Articulo>? Articulos { get; set; }
        public ICollection<Pedido>? Pedidos { get; set; }
    }
}