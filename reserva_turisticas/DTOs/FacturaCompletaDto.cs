namespace reserva_turisticas.Dtos
{
    public class FacturaCompletaDto
        {
            // Datos generales
            public int FacturaID { get; set; }
            public DateTime Fecha_Emision { get; set; }
            public decimal TotalFactura { get; set; }
            public string EstadoFactura { get; set; } = string.Empty;

            public int ReservaID { get; set; }
            public int Codigo_reserva { get; set; }
            public DateTime FechaReserva { get; set; }
            public decimal TotalReserva { get; set; }

            public int ClienteID { get; set; }
            public int Codigo_cliente { get; set; }
            public string ClienteNombreCompleto { get; set; } = string.Empty;
            public string? DNI { get; set; }
            public string? Correo_electronico { get; set; }
            public string? Direccion { get; set; }

            public List<FacturaServicioDto> Servicios { get; set; } = new();
            public List<FacturaTourDto> Tours { get; set; } = new();
            public List<FacturaPaqueteDto> Paquetes { get; set; } = new();
        }
}