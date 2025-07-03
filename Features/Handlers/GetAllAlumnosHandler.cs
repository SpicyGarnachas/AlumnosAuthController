using MediatR;
using Microsoft.EntityFrameworkCore;
using SafeVaultExample.Data;
using SafeVaultExample.Features.Queries;
using SafeVaultExample.Models;

namespace SafeVaultExample.Features.Handlers;

public class GetAllAlumnosHandler : IRequestHandler<GetAllAlumnosQuery, List<Alumno>>
{
    private readonly AppDbContext _context;

    public GetAllAlumnosHandler(AppDbContext context) => _context = context;

    public async Task<List<Alumno>> Handle(GetAllAlumnosQuery request, CancellationToken cancellationToken)
        => await _context.Alumnos.ToListAsync(cancellationToken);
}
