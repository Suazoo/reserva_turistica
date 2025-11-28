namespace reserva_turisticas.Dtos
{
    public class FacturaResumenDto
    {
        public int FacturaID { get; set; }
        public string CodigoFactura { get; set; } = string.Empty;
        public string Cliente { get; set; } = string.Empty;
        public int ReservaID { get; set; }
        public string CodigoReserva { get; set; } = string.Empty;
        public DateTime? Fecha_Emision { get; set; }
        public double? TotalFactura { get; set; }
        public string Estado { get; set; } = string.Empty;
        public int? Anio { get; set; }
        public int? Mes { get; set; }
        public string? MesNombre { get; set; }
    }
}