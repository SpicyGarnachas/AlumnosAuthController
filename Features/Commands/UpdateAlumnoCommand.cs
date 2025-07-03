using MediatR;
using SafeVaultExample.Models;

namespace SafeVaultExample.Features.Commands;

public record UpdateAlumnoCommand(int Id, string Nombre, int Edad) : IRequest<bool>;