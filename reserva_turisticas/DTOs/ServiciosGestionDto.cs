namespace reserva_turisticas.Dtos
{
    public class ServiciosGestionDto
    {
        public int ServicioID { get; set; }
        public string CodigoServicio { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public double? Precio { get; set; }          // si en BD es decimal, cámbialo a decimal?
        public string? Duracion { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}