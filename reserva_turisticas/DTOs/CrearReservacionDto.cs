using System;
using System.Collections.Generic;

namespace reserva_turisticas.Dtos
{
    public class CrearReservacionDto
    {
        public int ClienteID { get; set; }
        public int ServicioID { get; set; }
        public int TourID { get; set; }
        public int HotelID { get; set; }
        public int PaqueteID { get; set; }
        public DateTime FechaReserva { get; set; }
    }
}