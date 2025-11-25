namespace reserva_turisticas.Dtos
{
    public class ReporteFacturasFiltroDto
    {
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int? ClienteID { get; set; }
    }
}