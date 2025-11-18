using System;
using System.Collections.Generic;

namespace reserva_turisticas.Models;

public partial class Factura
{
    public int Id { get; set; }

    public DateOnly? FechaEmision { get; set; }

    public double? Total { get; set; }

    public string? Estado { get; set; }

    public int ReservaId { get; set; }

    public int MonedaId { get; set; }

    public virtual Monedum Moneda { get; set; } = null!;

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    public virtual Reserva Reserva { get; set; } = null!;
}
