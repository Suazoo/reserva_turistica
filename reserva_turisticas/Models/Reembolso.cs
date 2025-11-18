using System;
using System.Collections.Generic;

namespace reserva_turisticas.Models;

public partial class Reembolso
{
    public int Id { get; set; }

    public DateOnly? Fecha { get; set; }

    public decimal? Monto { get; set; }

    public string? Motivo { get; set; }

    public string? Estado { get; set; }

    public int PagoId { get; set; }

    public virtual Pago Pago { get; set; } = null!;
}
