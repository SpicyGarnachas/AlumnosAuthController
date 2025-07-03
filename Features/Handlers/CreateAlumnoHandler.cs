using MediatR;
using SafeVaultExample.Data;
using SafeVaultExample.Features.Commands;
using SafeVaultExample.Models;
using SafeVaultExample.Validators;

namespace SafeVaultExample.Features.Handlers;

public class CreateAlumnoHandler : IRequestHandler<CreateAlumnoCommand, bool>
{
    private readonly AppDbContext _context;

    public CreateAlumnoHandler(AppDbContext context) => _context = context;

    public async Task<bool> Handle(CreateAlumnoCommand request, 
                                     CancellationToken cancellationToken)
    {
        Alumno alumno = new Alumno()
        {
            Edad = request.Edad,
            Nombre = request.Nombre,
        };

        if (!alumno.IsValid())
            return false;

        _context.Alumnos.Add(alumno);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}