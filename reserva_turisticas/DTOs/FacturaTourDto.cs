
namespace reserva_turisticas.Dtos
{
    public class FacturaTourDto
        {
            public string Tour { get; set; } = string.Empty;
            public decimal PrecioUnitario { get; set; }
            public int Cantidad { get; set; }
            public decimal Subtotal { get; set; }
            public decimal Total { get; set; }
        }
}