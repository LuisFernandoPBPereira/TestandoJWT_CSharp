
using System.ComponentModel.DataAnnotations;

namespace TestandoJWT.DTOs;

public class LoginRequestDto
{
    [Required]
    public string? NomeUsuario { get; set; }
    [Required]
    public string? Senha { get; set; }
}