using MediatR;
using SafeVaultExample.Data;
using SafeVaultExample.Features.Commands;

namespace SafeVaultExample.Features.Handlers;

public class DeleteAlumnoHandler : IRequestHandler<DeleteAlumnoCommand, bool>
{
    private readonly AppDbContext _context;

    public DeleteAlumnoHandler(AppDbContext context) => _context = context;

    public async Task<bool> Handle(DeleteAlumnoCommand request, CancellationToken cancellationToken)
    {
        var alumno = await _context.Alumnos.FindAsync(request.Id, cancellationToken);

        if (alumno == null) return false;

        _context.Alumnos.Remove(alumno);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}