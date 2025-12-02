using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaAlmacenWeb.Models
{
    [Table("Distribuidores")]
    public class Distribuidor
    {
        [Key]
        public int IdDistribuidor { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Nombre del Distribuidor")]
        public string Nombre { get; set; }

        [MaxLength(20)]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [MaxLength(200)]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [MaxLength(200)]
        [Display(Name = "Catálogo / URL")]
        public string Catalogo { get; set; }

        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        public ICollection<Articulo> Articulos { get; set; }
    }
}