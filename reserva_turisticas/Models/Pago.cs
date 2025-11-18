using System;
using System.Collections.Generic;

namespace reserva_turisticas.Models;

public partial class Pago
{
    public int Id { get; set; }

    public DateOnly? Fecha { get; set; }

    public decimal? Monto { get; set; }

    public string? Estado { get; set; }

    public string? Observacion { get; set; }

    public int MetodoPagoId { get; set; }

    public int FacturaId { get; set; }

    public int MonedaId { get; set; }

    public virtual Factura Factura { get; set; } = null!;

    public virtual MetodoPago MetodoPago { get; set; } = null!;

    public virtual Monedum Moneda { get; set; } = null!;

    public virtual ICollection<Reembolso> Reembolsos { get; set; } = new List<Reembolso>();

    public virtual ICollection<Transaccion> Transaccions { get; set; } = new List<Transaccion>();
}
