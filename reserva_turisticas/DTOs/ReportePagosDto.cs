namespace reserva_turisticas.Dtos
{
    public class ReportePagosDto
    {
        public int PagoID { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string MetodoPago { get; set; } = string.Empty;

        public int FacturaID { get; set; }
        public decimal TotalFactura { get; set; }
        public DateTime Fecha_Emision { get; set; }

        public int ClienteID { get; set; }
        public int Codigo_cliente { get; set; }
        public string Cliente { get; set; } = string.Empty;
    }
}