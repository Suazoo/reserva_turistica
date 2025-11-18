using System;
using System.Collections.Generic;

namespace reserva_turisticas.Models;

public partial class PaqueteTour
{
    public int PaqueteId { get; set; }

    public int TourId { get; set; }

    public string? Nombre { get; set; }

    public int? Orden { get; set; }

    public string? Descripcion { get; set; }

    public TimeOnly? DuracionEstimada { get; set; }

    public virtual Paquete Paquete { get; set; } = null!;

    public virtual Tour Tour { get; set; } = null!;
}
