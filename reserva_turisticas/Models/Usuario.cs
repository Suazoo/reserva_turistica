using System;
using System.Collections.Generic;

namespace reserva_turisticas.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Contrasena { get; set; }

    public string? Estado { get; set; }

    public int PersonaId { get; set; }
    public string? LoginType { get; set; }

    public virtual Persona Persona { get; set; } = null!;
}
