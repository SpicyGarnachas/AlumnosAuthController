using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SafeVaultExample.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SafeVaultExample.Controllers;
[Route("api/[controller]")]
public class AuthController(IConfiguration config) : ControllerBase
{
    [HttpPost("login")]
    public ActionResult Login([FromBody] UserLoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (request.Email != "admin@correo.com" || request.Password != "12345" || request.ClientId != "cliente-01")
            return Unauthorized("Credenciales incorrectas");

        var claims = new[]
        {
           new Claim(ClaimTypes.Name, request.Email),
           new Claim("client_id", request.ClientId),
           new Claim(ClaimTypes.Role, "User")
       };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiration = DateTime.UtcNow.AddMinutes(10);

        var token = new JwtSecurityToken(
            issuer: config["JwtSettings:Issuer"],
            audience: config["JwtSettings:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: creds);

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
