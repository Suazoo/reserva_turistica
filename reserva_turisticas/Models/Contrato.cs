using System;
using System.Collections.Generic;

namespace reserva_turisticas.Models;

public partial class Contrato
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public string? Estado { get; set; }

    public DateOnly? FechaRegistro { get; set; }

    public string? UltimaVersion { get; set; }

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
