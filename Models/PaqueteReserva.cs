using System.ComponentModel.DataAnnotations.Schema;

namespace ReservaTuristica.Models
{
    // Entidad de unión explícita para la tabla dbo.Paquete_Reserva
    [Table("Paquete_Reserva", Schema = "dbo")]
    public class PaqueteReserva
    {
        // Columnas según la tabla: Paquete_id y Reserva_id
        public int PaqueteId { get; set; }
        public Paquete Paquete { get; set; } = null!;

        public int ReservaId { get; set; }
        public Reserva Reserva { get; set; } = null!;
    }
}