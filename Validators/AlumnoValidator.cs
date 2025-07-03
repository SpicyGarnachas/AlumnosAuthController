using SafeVaultExample.Models;

namespace SafeVaultExample.Validators;

public static class AlumnoValidator
{
    private static readonly char[] specialCharacters =
    [
        '!', '@', '#', '$', '%', '^', '&', '*', '(', ')',
        '-', '_', '=', '+', '[', ']', '{', '}', '|', '\\',
        ':', ';', '"', '\'', '<', '>', ',', '.', '?', '/'
    ];

    public static bool IsValid(this Alumno alumno)
    {
        if (string.IsNullOrWhiteSpace(alumno.Nombre))
            return false;

        if (alumno.Nombre.Any(c => specialCharacters.Contains(c)))
            return false;

        if (alumno.Edad > 30)
            return false;

        return true;
    }
}