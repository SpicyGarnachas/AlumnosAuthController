using MediatR;
using SafeVaultExample.Data;
using SafeVaultExample.Features.Queries;
using SafeVaultExample.Models;

namespace SafeVaultExample.Features.Handlers;

public class GetAlumnoByIdHandler : IRequestHandler<GetAlumnoByIdQuery, Alumno?>
{
    private readonly AppDbContext _context;

    public GetAlumnoByIdHandler(AppDbContext context) => _context = context;

    public async Task<Alumno?> Handle(GetAlumnoByIdQuery request, CancellationToken cancellationToken)
        => await _context.Alumnos.FindAsync(request.Id, cancellationToken);
}
