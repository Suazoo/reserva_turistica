using System;
using System.Collections.Generic;

namespace reserva_turisticas.Models;

public partial class ReservaTour
{
    public int ReservaId { get; set; }

    public int TourId { get; set; }

    public int? Cantidad { get; set; }

    public double? Subtotal { get; set; }

    public double? Total { get; set; }

    public virtual Reserva Reserva { get; set; } = null!;

    public virtual Tour Tour { get; set; } = null!;
}
