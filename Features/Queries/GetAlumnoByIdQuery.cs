using MediatR;
using SafeVaultExample.Models;

namespace SafeVaultExample.Features.Queries;

public record GetAlumnoByIdQuery(int Id) : IRequest<Alumno?>;