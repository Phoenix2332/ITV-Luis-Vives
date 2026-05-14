namespace ITV_Luis_Vives.Errors.Common;

/// <summary>
///     Record base para todos los errores de dominio
/// </summary>
/// <param name="Mensaje">Error de dominio</param>
public abstract record DomainErrors(string Mensaje);