//// filepath: c:\BasaDatos_01\Proyecto\reserva_proyecto\clonar-bueno\reserva_turisticas\Dtos\GenerarPagoDto.cs
namespace reserva_turisticas.Dtos
{
    public class GenerarPagoDto
    {
        public int FacturaID { get; set; }
        public int MetodoPagoID { get; set; }
        public int MonedaID { get; set; }
        public string Observacion { get; set; } = string.Empty;
    }
}