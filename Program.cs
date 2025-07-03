using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SafeVaultExample.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

try
{
    var config = builder.Configuration;
    var jwtSettings = config.GetSection("JwtSettings");

    builder.Services.AddControllers();
    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseInMemoryDatabase("AlumnosDb"));
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(Program)));
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!))
        };
    });

    builder.Services.AddAuthorization();

    var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapControllers();
    await app.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
    Environment.Exit(1);
}