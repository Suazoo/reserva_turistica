using System;
using System.Collections.Generic;

namespace reserva_turisticas.Models;

public partial class Monedum
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? CodigoIso { get; set; }

    public string? Simbolo { get; set; }

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    public virtual ICollection<Transaccion> Transaccions { get; set; } = new List<Transaccion>();
}
