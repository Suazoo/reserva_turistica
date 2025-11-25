namespace reserva_turisticas.Dtos
{
    public class ServiciosMasReservadosDto
    {
        public string Servicio { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public int TotalReservas { get; set; }
        public double? Ingresos { get; set; }        // si es decimal, cámbialo a decimal?
    }
}