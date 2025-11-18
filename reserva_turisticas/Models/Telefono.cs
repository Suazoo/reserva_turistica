using System;
using System.Collections.Generic;

namespace reserva_turisticas.Models;

public partial class Telefono
{
    public int Id { get; set; }

    public int? Numero { get; set; }

    public string? Estado { get; set; }

    public string? TipoTelefono { get; set; }

    public int PersonaId { get; set; }

    public virtual Persona Persona { get; set; } = null!;
}
