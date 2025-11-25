//// filepath: c:\BasaDatos_01\Proyecto\reserva_proyecto\clonar-bueno\reserva_turisticas\Dtos\GenerarPagoResultadoDto.cs
namespace reserva_turisticas.Dtos
{
    public class GenerarPagoResultadoDto
    {
        public int PagoID { get; set; }
        public int TipoMensaje { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }
}