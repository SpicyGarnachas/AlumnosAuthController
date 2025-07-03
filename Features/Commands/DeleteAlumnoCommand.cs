using MediatR;

namespace SafeVaultExample.Features.Commands;

public record DeleteAlumnoCommand(int Id) : IRequest<bool>;
