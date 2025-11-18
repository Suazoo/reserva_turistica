using System;
using System.Collections.Generic;

namespace reserva_turisticas.Models;

public partial class Hotel
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public DateTime? ChekinDesde { get; set; }

    public DateTime? ChekinHasta { get; set; }

    public int? CategoriaEstrellas { get; set; }

    public string? Politicas { get; set; }

    public int IdPais { get; set; }

    public int IdDepartamento { get; set; }

    public int IdCiudad { get; set; }

    public string? ReferenciaDireccion { get; set; }

    public virtual ICollection<Habitacion> Habitacions { get; set; } = new List<Habitacion>();

    public virtual Lugar IdCiudadNavigation { get; set; } = null!;

    public virtual Lugar IdDepartamentoNavigation { get; set; } = null!;

    public virtual Lugar IdPaisNavigation { get; set; } = null!;

    public virtual ICollection<ServicioHotel> ServicioHotels { get; set; } = new List<ServicioHotel>();

    public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
}
