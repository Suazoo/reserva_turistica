namespace reserva_turisticas.Dtos
{
    public class ActualizarReservaDto
    {
        public int ReservaID { get; set; }
        public int ClienteID { get; set; }
        public int ContratoID { get; set; }
        public int MonedaID { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string Politicas { get; set; } = string.Empty;
        public decimal TotalNuevo { get; set; }
    }
}