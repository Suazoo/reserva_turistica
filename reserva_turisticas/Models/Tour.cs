using System;
using System.Collections.Generic;

namespace reserva_turisticas.Models;

public partial class Tour
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public TimeOnly? DuracionHora { get; set; }

    public string? Descripcion { get; set; }

    public decimal? Precio { get; set; }

    public virtual ICollection<DisponibilidadTour> DisponibilidadTours { get; set; } = new List<DisponibilidadTour>();

    public virtual ICollection<PaqueteTour> PaqueteTours { get; set; } = new List<PaqueteTour>();

    public virtual ICollection<ReservaTour> ReservaTours { get; set; } = new List<ReservaTour>();

    public virtual ICollection<SalidaTour> SalidaTours { get; set; } = new List<SalidaTour>();
}
