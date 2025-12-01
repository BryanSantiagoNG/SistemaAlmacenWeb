using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaAlmacenWeb.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El usuario es requerido")]
        [MaxLength(50)]
        [Display(Name = "Nombre de Usuario")]
        public string UsuarioNombre { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [MaxLength(255)]
        [DataType(DataType.Password)] // Esto hace que se vean puntitos (****) en la web
        [Display(Name = "Contraseña")]
        public string Contraseña { get; set; }

        [MaxLength(30)]
        [Display(Name = "Rol / Permisos")]
        public string Rol { get; set; }
    }
}