using System;
using System.Collections.Generic;

namespace reserva_turisticas.Models;

public partial class ReservaServicio
{
    public int ServicioId { get; set; }

    public int ReservaId { get; set; }

    public DateOnly? FechaFin { get; set; }

    public DateOnly? FechaInicio { get; set; }

    public double? PrecioUnidad { get; set; }

    public double? Subtotal { get; set; }

    public double? Total { get; set; }

    public virtual Reserva Reserva { get; set; } = null!;

    public virtual Servicio Servicio { get; set; } = null!;
}
