namespace reserva_turisticas.Dtos
{
    public class ReporteFacturasDto
    {
        public int FacturaID { get; set; }
        public DateTime Fecha_Emision { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; } = string.Empty;

        public int Codigo_reserva { get; set; }
        public decimal TotalReserva { get; set; }

        public int Codigo_cliente { get; set; }
        public string Cliente { get; set; } = string.Empty;
    }
}