using System;
using System.Collections.Generic;

namespace reserva_turisticas.Models;

public partial class Cliente
{
    public int Id { get; set; }

    public int? CodigoCliente { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public int PersonaId { get; set; }

    public int CategoriaClienteId { get; set; }

    public virtual CategoriaCliente CategoriaCliente { get; set; } = null!;

    public virtual Persona Persona { get; set; } = null!;
}
