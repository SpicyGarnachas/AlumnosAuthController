using MediatR;
using SafeVaultExample.Data;
using SafeVaultExample.Features.Commands;
using SafeVaultExample.Validators;

namespace SafeVaultExample.Features.Handlers;

public class UpdateAlumnoHandler : IRequestHandler<UpdateAlumnoCommand, bool>
{
    private readonly AppDbContext _context;

    public UpdateAlumnoHandler(AppDbContext context) => _context = context;

    public async Task<bool> Handle(UpdateAlumnoCommand request, CancellationToken cancellationToken)
    {
        var alumno = await _context.Alumnos.FindAsync(request.Id, cancellationToken);

        if (alumno == null) return false;

        alumno.Nombre = request.Nombre;
        alumno.Edad = request.Edad;

        if (!alumno.IsValid())
            return false;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}