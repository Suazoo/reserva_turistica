using System;

namespace reserva_turisticas.Dtos
{
    public class ReservasListadoDto
    {
        public int ReservaID { get; set; }
        public string CodigoReserva { get; set; } = string.Empty;
        public DateTime? Fecha_Creacion { get; set; }
        public string Estado { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string Contrato { get; set; } = string.Empty;
    }
}