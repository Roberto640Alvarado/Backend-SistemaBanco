namespace SistemaBancoFront.Models
{
    public class ClienteTransaccionesViewModel
    {
        public int CodigoCliente { get; set; }
        public string Nombre { get; set; }

        public string Apellido { get; set; }
        public string NumeroTarjeta { get; set; }
        public List<Transaccion> Transacciones { get; set; }
    }
}
