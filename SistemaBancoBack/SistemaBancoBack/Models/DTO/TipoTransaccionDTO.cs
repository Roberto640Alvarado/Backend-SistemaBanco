using System.ComponentModel.DataAnnotations;

namespace SistemaBancoBack.Models.DTO
{
    public class TipoTransaccionDTO
    {
        [Required]
        [StringLength(50, ErrorMessage = "La descripción no puede superar los 50 caracteres.")]
        public string Descripcion { get; set; } = string.Empty;
    }
}
