using System;
using System.Collections.Generic;

namespace reserva_turisticas.Models;

public partial class Lugar
{
    public int IdLugar { get; set; }

    public string? Nombre { get; set; }

    public int? LugarIdLugar { get; set; }

    public virtual ICollection<Hotel> HotelIdCiudadNavigations { get; set; } = new List<Hotel>();

    public virtual ICollection<Hotel> HotelIdDepartamentoNavigations { get; set; } = new List<Hotel>();

    public virtual ICollection<Hotel> HotelIdPaisNavigations { get; set; } = new List<Hotel>();

    public virtual ICollection<Lugar> InverseLugarIdLugarNavigation { get; set; } = new List<Lugar>();

    public virtual Lugar? LugarIdLugarNavigation { get; set; }
}
