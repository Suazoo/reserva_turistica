namespace reserva_turisticas.Dtos
{
    public class ReportePagosFiltroDto
    {
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int? ClienteID { get; set; }
    }
}