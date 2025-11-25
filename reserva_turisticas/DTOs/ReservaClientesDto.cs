namespace reserva_turisticas.Dtos
{
    public class ReservaClientesDto
    {
        public int ClienteID { get; set; }
        public string CodigoCliente { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
    }
}