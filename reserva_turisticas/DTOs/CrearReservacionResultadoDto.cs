namespace reserva_turisticas.Dtos
{
    public class CrearReservacionResultadoDto
    {
        public string NombreCliente { get; set; } = string.Empty;
        public decimal PrecioServicio { get; set; }
        public decimal PrecioTour { get; set; }
        public decimal PrecioHotel { get; set; }
        public decimal PrecioPaquete { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Total { get; set; }
        public int ReservaID { get; set; }
        public int TipoMensaje { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }
}