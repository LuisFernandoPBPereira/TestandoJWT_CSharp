using TestandoJWT.DTOs;

namespace TestandoJWT.Services
{
    public interface IAuthenticationService
    {
        Task<string> Register(RegisterRequestDto request);
        Task<string> Login(LoginRequestDto request);
    }
}
