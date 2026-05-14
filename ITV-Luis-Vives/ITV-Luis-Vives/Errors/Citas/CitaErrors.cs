using ITV_Luis_Vives.Errors.Common;

namespace ITV_Luis_Vives.Errors.Citas;

/// <summary>
///     Contenedor de errores específicos para el dominio.
/// </summary>
public abstract record CitaErrors(string Message) : DomainErrors(Message) {
    public sealed record NotFound(string Id)
        : CitaErrors($"No se ha encontrado ninguna cita con el ID: {Id}");

    public sealed record Validation(IEnumerable<string> Errors)
        : CitaErrors(
            $"Se han detectado errores de validación en la entidad:{Environment.NewLine}• {string.Join($"{Environment.NewLine}• ", Errors)}");

    public sealed record MatriculaAlreadyExists(string Matricula)
        : CitaErrors($"Conflicto de integridad: La Matrícula {Matricula} ya está registrada en el sistema.");

    public sealed record DatabaseError(string Details)
        : CitaErrors($"Error de base de datos: {Details}");

    public sealed record StorageError(string Details)
        : CitaErrors($"Error de almacenamiento: {Details}");
}

/// <summary>
///     Factory para crear errores de dominio.
/// </summary>
public static class CitaError {
    public static DomainErrors NotFound(string id) {
        return new CitaErrors.NotFound(id);
    }

    public static DomainErrors Validation(IEnumerable<string> errors) {
        return new CitaErrors.Validation(errors);
    }

    public static DomainErrors MatriculaAlreadyExists(string dni) {
        return new CitaErrors.MatriculaAlreadyExists(dni);
    }

    public static DomainErrors DatabaseError(string details) {
        return new CitaErrors.DatabaseError(details);
    }

    public static DomainErrors StorageError(string details) {
        return new CitaErrors.StorageError(details);
    }
}