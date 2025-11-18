using System;
using System.Collections.Generic;

namespace reserva_turisticas.Models;

public partial class SalidaTour
{
    public int Id { get; set; }

    public DateTime? FechaInicio { get; set; }

    public int? CupoDisponible { get; set; }

    public double? Precio { get; set; }

    public string? Estado { get; set; }

    public int TourId { get; set; }

    public virtual Tour Tour { get; set; } = null!;

    public virtual ICollection<Guium> Guia { get; set; } = new List<Guium>();
}
