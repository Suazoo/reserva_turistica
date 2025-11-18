using System.ComponentModel.DataAnnotations;

namespace reserva_turisticas.DTOs
{
    public class RegistroDto
    {
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        [MaxLength(45, ErrorMessage = "El correo no puede exceder 45 caracteres")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Password { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio")]
        [Range(10000000, 99999999, ErrorMessage = "DNI debe ser un número válido de 8 dígitos")]
        public int DNI { get; set; }

        [Required(ErrorMessage = "El primer nombre es obligatorio")]
        [MaxLength(45, ErrorMessage = "El primer nombre no puede exceder 45 caracteres")]
        public string PrimerNombre { get; set; }

        [MaxLength(45, ErrorMessage = "El segundo nombre no puede exceder 45 caracteres")]
        public string SegundoNombre { get; set; }

        [Required(ErrorMessage = "El primer apellido es obligatorio")]
        [MaxLength(45, ErrorMessage = "El primer apellido no puede exceder 45 caracteres")]
        public string PrimerApellido { get; set; }

        [MaxLength(45, ErrorMessage = "El segundo apellido no puede exceder 45 caracteres")]
        public string SegundoApellido { get; set; }

        [MaxLength(45, ErrorMessage = "La dirección no puede exceder 45 caracteres")]
        public string Direccion { get; set; }
    }
}
