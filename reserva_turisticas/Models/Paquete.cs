using System;
using System.Collections.Generic;

namespace reserva_turisticas.Models;

public partial class Paquete
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public int? DuracionDias { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public decimal? PrecioPorPersona { get; set; }

    public decimal? PrecioTotal { get; set; }

    public virtual ICollection<PaqueteTour> PaqueteTours { get; set; } = new List<PaqueteTour>();

    public virtual ICollection<Guium> Guia { get; set; } = new List<Guium>();

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
