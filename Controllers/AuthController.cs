using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SafeVaultExample.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SafeVaultExample.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IConfiguration config) : ControllerBase
{
    // Usuarios simulados
    private readonly List<(string Email, string Password, string ClientId, string Role)> _users =
    [
        ("admin@correo.com", "12345", "cliente-01", "Admin"),
        ("user@correo.com", "12345", "cliente-01", "User")
    ];

    [HttpPost("login")]
    public ActionResult Login([FromBody] UserLoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = _users.FirstOrDefault(u =>
            u.Email == request.Email &&
            u.Password == request.Password &&
            u.ClientId == request.ClientId);

        if (user == default)
            return Unauthorized("Credenciales incorrectas");

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, request.Email),
            new Claim(ClaimTypes.Role, user.Role),
        };


        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddMinutes(10);

        var token = new JwtSecurityToken(
            issuer: config["JwtSettings:Issuer"],
            audience: config["JwtSettings:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new
        {
            access_token = tokenString,
            expires_in = (int)(expiration - DateTime.UtcNow).TotalSeconds,
            token_type = "Bearer",
            refresh_token = "fake-refresh-token",
            id_token = tokenString
        });
    }
}
