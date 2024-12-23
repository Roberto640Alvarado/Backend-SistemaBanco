using System.ComponentModel.DataAnnotations;

namespace SistemaBancoBack.Models
{
    public class TipoTransaccion
    {
        [Key]
        public int CodigoTipoTransaccion { get; set; }

        [Required]
        [StringLength(50)]
        public string? Descripcion { get; set; }

        public ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
    }
}
