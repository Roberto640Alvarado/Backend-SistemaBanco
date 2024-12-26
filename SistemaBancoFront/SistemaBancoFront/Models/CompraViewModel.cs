using SistemaBancoFront.Models.DTO;

namespace SistemaBancoFront.Models.ViewModels
{
    public class CompraViewModel
    {
        public Cliente Cliente { get; set; }
        public CompraDTO Compra { get; set; }
    }
}
