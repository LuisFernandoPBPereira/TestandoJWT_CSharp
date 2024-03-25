using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestandoJWT.Entities;
using TestandoJWT.DTOs;

namespace TestandoJWT.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly IConfiguration _configuration;

        public AuthenticationService(UserManager<Usuario> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<string> Register(RegisterRequestDto request)
        {
            var userByEmail = await _userManager.FindByEmailAsync(request.Email);
            var userByUsername = await _userManager.FindByNameAsync(request.NomeUsuario);
            if (userByEmail is not null || userByUsername is not null)
            {
                throw new ArgumentException($"User with email {request.Email} or username {request.NomeUsuario} already exists.");
            }

            Usuario user = new()
            {
                Email = request.Email,
                UserName = request.NomeUsuario,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, request.Senha);

            if (!result.Succeeded)
            {
                throw new ArgumentException($"Unable to register user {request.NomeUsuario} errors: {GetErrorsText(result.Errors)}");
            }

            return await Login(new LoginRequestDto { NomeUsuario = request.Email, Senha = request.Senha });
        }

        public async Task<string> Login(LoginRequestDto request)
        {
            var user = await _userManager.FindByNameAsync(request.NomeUsuario);

            if (user is null)
            {
                user = await _userManager.FindByEmailAsync(request.NomeUsuario);
            }

            if (user is null || !await _userManager.CheckPasswordAsync(user, request.Senha))
            {
                throw new ArgumentException($"Unable to authenticate user {request.NomeUsuario}");
            }

            var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

            var token = GetToken(authClaims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private JwtSecurityToken GetToken(IEnumerable<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            return token;
        }

        private string GetErrorsText(IEnumerable<IdentityError> errors)
        {
            return string.Join(", ", errors.Select(error => error.Description).ToArray());
        }
    }
}
