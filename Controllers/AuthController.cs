using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DSW_3P_PR02.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DSW_3P_PR02.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(
    IOptions<JwtOptions> jwtOptions,
    IOptions<DemoUserOptions> demoUserOptions) : ControllerBase
{
    private readonly JwtOptions _jwt = jwtOptions.Value;
    private readonly DemoUserOptions _demoUser = demoUserOptions.Value;

    /// <summary>Valida el usuario de demostración y entrega un JWT.</summary>
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<LoginResponse> Login(LoginRequest request)
    {
        var passwordMatches = CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(request.Contrasena),
            Encoding.UTF8.GetBytes(_demoUser.Password));
        if (!string.Equals(request.Usuario, _demoUser.Username, StringComparison.Ordinal)
            || !passwordMatches)
        {
            return Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: "Credenciales inválidas",
                detail: "El usuario o la contraseña no son correctos.");
        }

        var now = DateTimeOffset.UtcNow;
        var expiresAt = now.AddMinutes(_jwt.ExpirationMinutes);
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SigningKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, _demoUser.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, _demoUser.Username)
        };
        var token = new JwtSecurityToken(
            _jwt.Issuer,
            _jwt.Audience,
            claims,
            now.UtcDateTime,
            expiresAt.UtcDateTime,
            credentials);

        return Ok(new LoginResponse(
            new JwtSecurityTokenHandler().WriteToken(token),
            "Bearer",
            expiresAt));
    }
}
