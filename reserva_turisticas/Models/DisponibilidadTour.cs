using System;
using System.Collections.Generic;

namespace reserva_turisticas.Models;

public partial class DisponibilidadTour
{
    public int Id { get; set; }

    public DateTime? FechaInicio { get; set; }

    public int? CupoTotal { get; set; }

    public int? CupoDisponible { get; set; }

    public int TourId { get; set; }

    public virtual Tour Tour { get; set; } = null!;
}
