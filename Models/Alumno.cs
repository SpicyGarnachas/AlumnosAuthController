using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SafeVaultExample.Models;

public class Alumno
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Edad { get; set; }
}