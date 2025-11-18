using System;
using System.Collections.Generic;

namespace reserva_turisticas.Models;

public partial class ServicioHotel
{
    public int ServicioId { get; set; }

    public int HotelId { get; set; }

    public double? CostoAdicional { get; set; }

    public string? Descripcion { get; set; }

    public virtual Hotel Hotel { get; set; } = null!;

    public virtual Servicio Servicio { get; set; } = null!;
}
