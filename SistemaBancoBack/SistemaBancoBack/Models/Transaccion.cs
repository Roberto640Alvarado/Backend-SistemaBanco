using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SistemaBancoBack.Models
{
    public class Transaccion
    {
        [Key]
        public int CodigoTransaccion { get; set; }

        [Required]
        public int CodigoCliente { get; set; }

        [Required]
        public int CodigoTipoTransaccion { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [StringLength(255)]
        public string? Descripcion { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Monto { get; set; }

        //Relación con Cliente
        [ForeignKey("CodigoCliente")]
        [JsonIgnore]
        public Cliente Cliente { get; set; }

        //Relación con TipoTransaccion
        [ForeignKey("CodigoTipoTransaccion")]
        [JsonIgnore]
        public TipoTransaccion TipoTransaccion { get; set; } 
    }
}
