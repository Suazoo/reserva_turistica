using Microsoft.AspNetCore.Mvc;
using reserva_turisticas.DTOs;
using reserva_turisticas.Services;

namespace reserva_turisticas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// REGISTRO MANUAL - Crea un nuevo usuario con datos completos
        /// POST /api/auth/registro
        /// </summary>
        [HttpPost("registro")]
        public IActionResult Registro([FromBody] RegistroDto registroDto)
        {
            // Validar ModelState
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, message, token) = _authService.RegistrarUsuarioManual(
                registroDto.Correo,
                registroDto.Password,
                registroDto.DNI,
                registroDto.PrimerNombre,
                registroDto.SegundoNombre,
                registroDto.PrimerApellido,
                registroDto.SegundoApellido,
                registroDto.Direccion
            );

            if (!success)
                return BadRequest(new { message });

            // Construir nombre completo para respuesta
            string nombreCompleto = $"{registroDto.PrimerNombre} {registroDto.SegundoNombre} {registroDto.PrimerApellido} {registroDto.SegundoApellido}".Trim();
            nombreCompleto = System.Text.RegularExpressions.Regex.Replace(nombreCompleto, @"\s+", " ");

            return Ok(new
            {
                message,
                token,
                usuario = new
                {
                    correo = registroDto.Correo,
                    nombreCompleto = nombreCompleto,
                    dni = registroDto.DNI
                }
            });
        }

        /// <summary>
        /// LOGIN MANUAL - Inicia sesión con correo y contraseña
        /// POST /api/auth/login
        /// </summary>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            // Validar ModelState
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, message, token) = _authService.LoginManual(
                loginDto.Correo,
                loginDto.Password
            );

            if (!success)
                return Unauthorized(new { message });

            return Ok(new { message, token });
        }

        /// <summary>


        /// <summary>
        /// VERIFICAR SI UN CORREO EXISTE
        /// GET /api/auth/verificar-correo?correo=test@example.com
        /// </summary>
        [HttpGet("verificar-correo")]
        public IActionResult VerificarCorreo([FromQuery] string correo)
        {
            if (string.IsNullOrEmpty(correo))
                return BadRequest(new { message = "Debe proporcionar un correo" });

            var existe = _authService.ExisteCorreo(correo);
            return Ok(new { existe });
        }

        /// <summary>
        /// VERIFICAR SI UN DNI EXISTE
        /// GET /api/auth/verificar-dni?dni=12345678
        /// </summary>
        [HttpGet("verificar-dni")]
        public IActionResult VerificarDNI([FromQuery] int dni)
        {
            if (dni <= 0)
                return BadRequest(new { message = "DNI inválido" });

            var existe = _authService.ExisteDNI(dni);
            return Ok(new { existe });
        }








        

    }
}