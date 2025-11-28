using System;

namespace reserva_turisticas.Dtos
{
    public class ReservaCrearDto
    {
        
        public int? CodigoReserva { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string? Estado { get; set; }
        public decimal? Total { get; set; }

        public int ContratoId { get; set; }
        public int MonedaId { get; set; }
        public int ClienteId { get; set; }
        public int HotelId { get; set; }

        public string? Politicas { get; set; }
    }
}