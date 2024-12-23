using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SistemaBancoBack.Models
{
    public class Cliente
    {
        [Key]
        public int CodigoCliente { get; set; }

        [Required]
        [StringLength(100)]
        public string? Nombre { get; set; } 

        [Required]
        [StringLength(100)]
        public string? Apellido { get; set; }

        [Required]
        [StringLength(16)]
        public string? NumeroTarjeta { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal LimiteCredito { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal SaldoDisponible { get; set; }

        [JsonIgnore]
        public ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
    }
}
