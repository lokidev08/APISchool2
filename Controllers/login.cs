using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using APISchool.Data;

namespace APISchool.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ColegioDbContext _context;
        private readonly IConfiguration _configuration;

        public LoginController(ColegioDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (usuario == null)
            {
                return Unauthorized("usuario o contraseña incorrecta");
            }

            var passwordValida = BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash);
            if (!passwordValida)
            {
                return Unauthorized("usuario o contraseña incorrecta");
            }

            var token = GenerarToken(usuario);
            return Ok(new { Token = token });
        }

        [HttpPost("validate-token")]
        public IActionResult ValidateToken(TokenValidationRequest request)
        {
            var token = NormalizeBearerToken(request.Token);

            if (string.IsNullOrWhiteSpace(token))
            {
                return Unauthorized(new
                {
                    valid = false,
                    message = "Debes enviar un token."
                });
            }

            try
            {
                var jwt = _configuration.GetSection("Jwt");
                var tokenHandler = new JwtSecurityTokenHandler();

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwt["Issuer"],
                    ValidAudience = jwt["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"] ?? string.Empty)),
                    ClockSkew = TimeSpan.Zero
                }, out _);

                return Ok(new
                {
                    valid = true,
                    message = "Token valido. Ya estas autorizado."
                });
            }
            catch (SecurityTokenExpiredException)
            {
                return Unauthorized(new
                {
                    valid = false,
                    message = "Token expirado. Inicia sesion otra vez."
                });
            }
            catch (Exception)
            {
                return Unauthorized(new
                {
                    valid = false,
                    message = "Token invalido. No estas autorizado."
                });
            }
        }

        private string GenerarToken(Usuario usuario)
        {
            var jwt = _configuration.GetSection("Jwt");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Username),
                new Claim("Nombre", usuario.Nombre)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"] ?? string.Empty));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(jwt["ExpireMinutes"] ?? "60")),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string? NormalizeBearerToken(string? token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            token = token.Trim();
            const string bearerPrefix = "Bearer ";

            return token.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase)
                ? token[bearerPrefix.Length..].Trim()
                : token;
        }
    }
}
