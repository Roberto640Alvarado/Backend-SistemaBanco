using SistemaBancoFront.Models.DTO;

namespace SistemaBancoFront.Models.ViewModels
{
    public class PagoViewModel
    {
        public Cliente Cliente { get; set; }
        public PagoDTO Pago { get; set; }
    }
}
