namespace reserva_turisticas.Dtos
{
    public class ReservaServicioInfoDto
    {
        public int ServicioID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public decimal Precio { get; set; }      // si tu columna es float, puedes usar double
        public string Duracion { get; set; } = string.Empty;
    }
}