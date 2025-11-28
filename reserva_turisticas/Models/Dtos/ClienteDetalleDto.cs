namespace reserva_turisticas.Models.Dtos
{
    public class ClienteDetalleDto
    {
        public int ClienteID { get; set; }
        public int? Codigo_cliente { get; set; }
        public DateTime? Fecha_registro { get; set; }
        public int? DNI { get; set; }
        public string? Primer_nombre { get; set; }
        public string? Segundo_nombre { get; set; }
        public string? Primer_apellido { get; set; }
        public string? Segundo_apellido { get; set; }
        public string? Correo_electronico { get; set; }
        public string? Direccion { get; set; }
        public string? Categoria { get; set; }
        public string? Prioridad { get; set; }
    }
}