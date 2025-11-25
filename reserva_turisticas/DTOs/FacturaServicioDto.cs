namespace reserva_turisticas.Dtos
{
    public class FacturaServicioDto
        {
            public string Servicio { get; set; } = string.Empty;
            public decimal Precio_unidad { get; set; }
            public decimal Subtotal { get; set; }
            public decimal Total { get; set; }
        }
}