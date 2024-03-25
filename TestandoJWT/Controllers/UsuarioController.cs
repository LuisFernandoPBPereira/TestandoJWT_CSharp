using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestandoJWT.DTOs;
using TestandoJWT.Services;

namespace TestandoJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        
        private readonly IAuthenticationService _authenticationService;

        public UsuarioController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var response = await _authenticationService.Login(request);

            return Ok($"Usuário logado! Token: {response}");
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var response = await _authenticationService.Register(request);

            return Ok(response);
        }
        
    }
}
