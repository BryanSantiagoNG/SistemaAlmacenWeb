using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaAlmacenWeb.Models
{
    [Table("Facturas")]
    public class Factura
    {
        [Key]
        public int IdFactura { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Emisión")]
        public DateTime Fecha { get; set; }

        [Display(Name = "Cliente")]
        public int? IdCliente { get; set; }

        [ForeignKey("IdCliente")]
        public Cliente Cliente { get; set; }


        [Display(Name = "Total Factura")]
        [Column(TypeName = "decimal(10, 2)")]
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }

        public ICollection<DetalleFactura> DetalleFacturas { get; set; }
    }
}