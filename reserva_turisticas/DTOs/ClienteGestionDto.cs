namespace reserva_turisticas.Dtos
{
    public class ClienteGestionDto
    {
        public int ClienteID { get; set; }
        public string CodigoCliente { get; set; } = null!;
        public string NombreCompleto { get; set; } = null!;
        public string? Email { get; set; }
        public int? Telefono { get; set; }
        public DateTime? Fecha_registro { get; set; }
        public string? Categoria { get; set; }
        public string? Prioridad { get; set; }
        public int? CategoriaCliente_id { get; set; }
        public string Estado { get; set; } = null!;
    }
}