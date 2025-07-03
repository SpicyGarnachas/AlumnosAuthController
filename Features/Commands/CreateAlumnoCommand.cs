using MediatR;
using SafeVaultExample.Models;

namespace SafeVaultExample.Features.Commands;

public record CreateAlumnoCommand(string Nombre, int Edad) : IRequest<bool>;