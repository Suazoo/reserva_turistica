using System;
using System.Collections.Generic;

namespace reserva_turisticas.Models;

public partial class CategoriaCliente
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Prioridad { get; set; }

    public string? Estado { get; set; }

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
}
