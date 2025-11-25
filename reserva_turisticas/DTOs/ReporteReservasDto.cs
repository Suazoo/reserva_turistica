namespace reserva_turisticas.Dtos
{
    public class ReporteReservasDto
    {
        public int ReservaID { get; set; }
        public int Codigo_reserva { get; set; }
        public DateTime Fecha_Creacion { get; set; }
        public decimal TotalReserva { get; set; }
        public string Estado { get; set; } = string.Empty;

        public int ClienteID { get; set; }
        public int Codigo_cliente { get; set; }
        public string Cliente { get; set; } = string.Empty;

        public string? Servicio { get; set; }
        public decimal? PrecioServicio { get; set; }

        public string? Tour { get; set; }
        public decimal? PrecioTour { get; set; }

        public string? Paquete { get; set; }
        public decimal? PrecioPaquete { get; set; }

        public string? Hotel { get; set; }
        public decimal? PrecioHotel { get; set; }
    }
}