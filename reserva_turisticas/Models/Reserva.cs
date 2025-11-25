using System;
using System.Collections.Generic;

namespace reserva_turisticas.Models;

public partial class Reserva
{
    public int Id { get; set; }

    public int? CodigoReserva { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public string? Estado { get; set; }

    public decimal? Total { get; set; }

    public int ContratoId { get; set; }

    public int MonedaId { get; set; }
    public int ClienteId { get; set; }
    public virtual Cliente Cliente { get; set; } = null!;
    public int HotelId { get; set; }
    public virtual Hotel Hotel { get; set; } = null!;
    

    public string? Politicas { get; set; }

    public virtual Contrato Contrato { get; set; } = null!;

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    public virtual Monedum Moneda { get; set; } = null!;

    public virtual ICollection<ReservaServicio> ReservaServicios { get; set; } = new List<ReservaServicio>();

    public virtual ICollection<ReservaTour> ReservaTours { get; set; } = new List<ReservaTour>();

    public virtual ICollection<Paquete> Paquetes { get; set; } = new List<Paquete>();
}
