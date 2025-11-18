using System;
using System.Collections.Generic;

namespace reserva_turisticas.Models;

public partial class Persona
{
    public int Id { get; set; }

    public int? Dni { get; set; }

    public string? PrimerNombre { get; set; }

    public string? SegundoNombre { get; set; }

    public string? PrimerApellido { get; set; }

    public string? SegundoApellido { get; set; }

    public string? CorreoElectronico { get; set; }

    public string? Direccion { get; set; }

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();

    public virtual ICollection<Telefono> Telefonos { get; set; } = new List<Telefono>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

    public virtual ICollection<Idioma> Idiomas { get; set; } = new List<Idioma>();
}
