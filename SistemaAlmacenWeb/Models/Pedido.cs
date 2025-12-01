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

        public DateTime Fecha { get; set; }

        [MaxLength(30)]
        public string TipoPedido { get; set; }

        [MaxLength(30)]
        public string Estado { get; set; }

        public int IdProveedor { get; set; }
        public Proveedor Proveedor { get; set; }

        public ICollection<DetallePedido> DetallePedidos { get; set; }
    }
}
