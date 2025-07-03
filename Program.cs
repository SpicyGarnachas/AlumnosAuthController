using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SafeVaultExample.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ───────────────────────────────────────────────────────────
// 1) Limpiamos los mapeos por defecto para que Name y Role
//    lleguen exactamente como los emitimos en el token.   🔥
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

// ───────────────────────────────────────────────────────────
var config = builder.Configuration;
var jwtSection = config.GetSection("JwtSettings");
var jwtKey = jwtSection["Key"]!;
var jwtIssuer = jwtSection["Issuer"]!;
var jwtAudience = jwtSection["Audience"]!;

// ───────────────────────────────────────────────────────────
// 2) Servicios
// ───────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("AlumnosDb"));
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining(typeof(Program)));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ───────────────────────────────────────────────────────────
// 3) Autenticación JWT
// ───────────────────────────────────────────────────────────
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)),

            RoleClaimType = ClaimTypes.Role,   // <- mapeo de rol
            NameClaimType = ClaimTypes.Name    // <- mapeo de name
        };

        // 4) Log detallado si la validación falla        🔥
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = ctx =>
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"JWT FAIL: {ctx.Exception.Message}");
                Console.ResetColor();
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

// ───────────────────────────────────────────────────────────
// 5) Pipeline HTTP
// ───────────────────────────────────────────────────────────
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();   // <- siempre antes de Authorization
app.UseAuthorization();

app.MapControllers();
app.Run();
