using System;
using System.Collections.Generic;

namespace reserva_turisticas.Models;

public partial class Idioma
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? CodigoIso { get; set; }

    public virtual ICollection<Persona> Personas { get; set; } = new List<Persona>();
}
