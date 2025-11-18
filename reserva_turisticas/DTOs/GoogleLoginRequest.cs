using System.ComponentModel.DataAnnotations;

namespace reserva_turisticas.DTOs
{
    public class GoogleLoginRequest
    {
        [Required(ErrorMessage = "El token de Google es obligatorio")]
        public string IdToken { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Name { get; set; }

        // Campos opcionales que Google puede proveer
        public string GivenName { get; set; }  
        public string FamilyName { get; set; } 
    }
}
