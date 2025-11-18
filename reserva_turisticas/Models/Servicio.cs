using System;
using System.Collections.Generic;

namespace reserva_turisticas.Models;

public partial class Servicio
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public string? AplicaA { get; set; }

    public string? Estado { get; set; }

    public string? Politicas { get; set; }

    public virtual ICollection<ReservaServicio> ReservaServicios { get; set; } = new List<ReservaServicio>();

    public virtual ICollection<ServicioHotel> ServicioHotels { get; set; } = new List<ServicioHotel>();
}
