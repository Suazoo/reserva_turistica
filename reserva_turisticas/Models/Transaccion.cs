using System;
using System.Collections.Generic;

namespace reserva_turisticas.Models;

public partial class Transaccion
{
    public int Id { get; set; }

    public string? Tipo { get; set; }

    public string? Estado { get; set; }

    public DateTime? FechaTransaccion { get; set; }

    public decimal? Monto { get; set; }

    public int PagoId { get; set; }

    public int MonedaId { get; set; }

    public virtual Monedum Moneda { get; set; } = null!;

    public virtual Pago Pago { get; set; } = null!;
}
