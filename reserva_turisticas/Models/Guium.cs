using System;
using System.Collections.Generic;

namespace reserva_turisticas.Models;

public partial class Guium
{
    public int Id { get; set; }

    public string? Lincencia { get; set; }

    public int? CalificacionProm { get; set; }

    public int EmpleadoId { get; set; }

    public virtual Empleado Empleado { get; set; } = null!;

    public virtual ICollection<Paquete> Paquetes { get; set; } = new List<Paquete>();

    public virtual ICollection<SalidaTour> SalidaTours { get; set; } = new List<SalidaTour>();
}
