using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using reserva_turisticas.Data;
using reserva_turisticas.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Google.Apis.Auth;

namespace reserva_turisticas.Services
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;
        private readonly ReservaTuristicaContext _context;

        public AuthService(IConfiguration configuration, ReservaTuristicaContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        /// <summary>
        /// REGISTRO MANUAL: Crea un nuevo usuario con todos los datos de Persona
        /// </summary>
        public (bool success, string message, string token) RegistrarUsuarioManual(
            string correo,
            string password,
            int dni,
            string primerNombre,
            string segundoNombre,
            string primerApellido,
            string segundoApellido,
            string direccion)
        {
            // Validar que no exista el correo
            var existeCorreo = _context.Usuarios
                .Include(u => u.Persona)
                .Any(u => u.Persona.CorreoElectronico == correo);

            if (existeCorreo)
                return (false, "El correo ya está registrado", null);

            // Validar que no exista el DNI
            var existeDNI = _context.Personas.Any(p => p.Dni == dni);
            if (existeDNI)
                return (false, "El DNI ya está registrado", null);

            // Validar contraseña
            if (string.IsNullOrEmpty(password) || password.Length < 6)
                return (false, "La contraseña debe tener al menos 6 caracteres", null);

            try
            {
                // 1. Crear Persona con todos sus datos
                var persona = new Persona
                {
                    Dni = dni,
                    CorreoElectronico = correo,
                    PrimerNombre = primerNombre,
                    SegundoNombre = segundoNombre,
                    PrimerApellido = primerApellido,
                    SegundoApellido = segundoApellido,
                    Direccion = direccion
                };
                _context.Personas.Add(persona);
                _context.SaveChanges();

                // 2. Crear Usuario con contraseña hasheada
                // Nombre de usuario: concatenar nombres completos
                string nombreCompleto = $"{primerNombre} {segundoNombre} {primerApellido} {segundoApellido}".Trim();
                nombreCompleto = System.Text.RegularExpressions.Regex.Replace(nombreCompleto, @"\s+", " ");

                var usuario = new Usuario
                {
                    Nombre = nombreCompleto,
                    Contrasena = BCrypt.Net.BCrypt.HashPassword(password),
                    Estado = "A", // Activo
                    PersonaId = persona.Id,
                    Persona = persona
                };
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();

                // 3. Generar token
                var token = GenerarToken(usuario);
                return (true, "Usuario registrado exitosamente", token);
            }
            catch (Exception ex)
            {
                return (false, $"Error al registrar: {ex.Message}", null);
            }
        }

        /// <summary>
        /// LOGIN MANUAL: Verifica credenciales y genera token
        /// </summary>
        public (bool success, string message, string token) LoginManual(string correo, string password)
        {
            // Buscar usuario activo
            var usuario = _context.Usuarios
                .Include(u => u.Persona)
                .FirstOrDefault(u =>
                    u.Persona.CorreoElectronico == correo &&
                    u.Estado == "A");

            if (usuario == null)
                return (false, "Usuario no encontrado o inactivo", null);

            // Verificar que tenga contraseña (no es usuario de Google)
            if (string.IsNullOrEmpty(usuario.Contrasena))
                return (false, "Este usuario se registró con Google. Use 'Iniciar con Google'", null);

            // Verificar contraseña
            if (!BCrypt.Net.BCrypt.Verify(password, usuario.Contrasena))
                return (false, "Contraseña incorrecta", null);

            var token = GenerarToken(usuario);
            return (true, "Login exitoso", token);
        }

        /// <summary>
        /// LOGIN CON GOOGLE: Valida IdToken, crea usuario si no existe
        /// </summary>
        public async Task<(bool success, string message, string token)> LoginGoogle(
            string idToken,
            string email,
            string name,
            string givenName = null,
            string familyName = null)
        {
            try
            {
                // 1. VALIDAR EL ID TOKEN DE GOOGLE
                var payload = await ValidarGoogleToken(idToken);
                if (payload == null || payload.Email != email)
                    return (false, "Token de Google inválido", null);

                // 2. Buscar usuario existente
                var usuario = _context.Usuarios
                    .Include(u => u.Persona)
                    .FirstOrDefault(u => u.Persona.CorreoElectronico == email);

                // 3. Si no existe, crear usuario nuevo
                if (usuario == null)
                {
                    // Separar nombre completo de Google
                    string primerNombre = !string.IsNullOrEmpty(givenName) ? givenName : name.Split(' ')[0];
                    string primerApellido = !string.IsNullOrEmpty(familyName) ? familyName :
                        (name.Split(' ').Length > 1 ? name.Split(' ')[1] : "");

                    var persona = new Persona
                    {
                        CorreoElectronico = email,
                        PrimerNombre = primerNombre,
                        PrimerApellido = primerApellido,
                        Dni = null // Google no provee DNI, se puede completar después
                    };
                    _context.Personas.Add(persona);
                    _context.SaveChanges();

                    usuario = new Usuario
                    {
                        Nombre = name,
                        Contrasena = null, // Google no usa contraseña
                        Estado = "A",
                        PersonaId = persona.Id,
                        Persona = persona
                    };
                    _context.Usuarios.Add(usuario);
                    _context.SaveChanges();
                }
                else
                {
                    // Validar que el usuario esté activo
                    if (usuario.Estado != "A")
                        return (false, "Usuario inactivo", null);
                }

                var token = GenerarToken(usuario);
                return (true, "Login con Google exitoso", token);
            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}", null);
            }
        }

        /// <summary>
        /// VALIDA EL ID TOKEN DE GOOGLE usando la librería oficial
        /// </summary>
        private async Task<GoogleJsonWebSignature.Payload> ValidarGoogleToken(string idToken)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new[] { _configuration["Google:ClientId"] }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
                return payload;
            }
            catch
            {
                return null; // Token inválido
            }
        }

        /// <summary>
        /// Genera JWT con información completa del usuario
        /// </summary>
        private string GenerarToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            // Construir nombre completo
            string nombreCompleto = $"{usuario.Persona.PrimerNombre} {usuario.Persona.SegundoNombre} {usuario.Persona.PrimerApellido} {usuario.Persona.SegundoApellido}".Trim();
            nombreCompleto = System.Text.RegularExpressions.Regex.Replace(nombreCompleto, @"\s+", " ");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, usuario.Persona.CorreoElectronico),
                    new Claim(ClaimTypes.Name, nombreCompleto),
                    new Claim("UsuarioId", usuario.Id.ToString()),
                    new Claim("PersonaId", usuario.PersonaId.ToString()),
                    new Claim("PrimerNombre", usuario.Persona.PrimerNombre ?? ""),
                    new Claim("PrimerApellido", usuario.Persona.PrimerApellido ?? ""),
                    new Claim("TienePassword", (!string.IsNullOrEmpty(usuario.Contrasena)).ToString()),
                    new Claim("DNI", usuario.Persona.Dni?.ToString() ?? "")
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// VERIFICAR SI UN CORREO YA EXISTE
        /// </summary>
        public bool ExisteCorreo(string correo)
        {
            return _context.Usuarios
                .Include(u => u.Persona)
                .Any(u => u.Persona.CorreoElectronico == correo);
        }

        /// <summary>
        /// VERIFICAR SI UN DNI YA EXISTE
        /// </summary>
        public bool ExisteDNI(int dni)
        {
            return _context.Personas.Any(p => p.Dni == dni);
        }
    }
}