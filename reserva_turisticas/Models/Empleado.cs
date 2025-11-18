using System;
using System.Collections.Generic;

namespace reserva_turisticas.Models;

public partial class Empleado
{
    public int Id { get; set; }

    public int? NumEmpleado { get; set; }

    public int PersonaId { get; set; }

    public string? Cargo { get; set; }

    public DateOnly? FechaIngreso { get; set; }

    public string? Estado { get; set; }

    public virtual ICollection<Guium> Guia { get; set; } = new List<Guium>();

    public virtual Persona Persona { get; set; } = null!;

    public virtual ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();
}
