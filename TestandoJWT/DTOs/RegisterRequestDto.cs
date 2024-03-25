using System.ComponentModel.DataAnnotations;

namespace TestandoJWT.DTOs
{
    public class RegisterRequestDto
    {

        [Required]
        public string? NomeUsuario { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Senha { get; set; }
    }
}
