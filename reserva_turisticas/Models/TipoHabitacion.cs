using System;
using System.Collections.Generic;

namespace reserva_turisticas.Models;

public partial class TipoHabitacion
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public int? Capacidad { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<Habitacion> Habitacions { get; set; } = new List<Habitacion>();
}
