using MediatR;
using SafeVaultExample.Models;

namespace SafeVaultExample.Features.Queries;

public record GetAllAlumnosQuery() : IRequest<List<Alumno>>;