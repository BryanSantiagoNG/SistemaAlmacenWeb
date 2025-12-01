using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaAlmacenWeb.Models
{
    [Table("Clientes")]
    public class Cliente
    {
        [Key]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100)]
        [Display(Name = "Nombre Completo / Razón Social")]
        public string Nombre { get; set; } // Este se queda igual, es Required

        [MaxLength(20)]
        [Display(Name = "RFC")]
        public string? RFC { get; set; } // <--- AGREGAR EL ?

        [MaxLength(200)]
        [Display(Name = "Dirección")]
        public string? Direccion { get; set; } // <--- AGREGAR EL ?

        [MaxLength(20)]
        [Display(Name = "Teléfono")]
        [DataType(DataType.PhoneNumber)]
        public string? Telefono { get; set; } // <--- AGREGAR EL ?

        [MaxLength(100)]
        [Display(Name = "Correo Electrónico")]
        [EmailAddress]
        public string? Email { get; set; } // <--- AGREGAR EL ?

        public ICollection<Factura>? Facturas { get; set; } // También aquí por si acaso
    }
}