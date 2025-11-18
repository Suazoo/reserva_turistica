using System;
using System.Collections.Generic;

namespace reserva_turisticas.Models;

public partial class Habitacion
{
    public int Id { get; set; }

    public int? CodigoInterno { get; set; }

    public int? Piso { get; set; }

    public string? Estado { get; set; }

    public int HotelId { get; set; }

    public int TipoHabitacionId { get; set; }

    public virtual Hotel Hotel { get; set; } = null!;

    public virtual TipoHabitacion TipoHabitacion { get; set; } = null!;
}
