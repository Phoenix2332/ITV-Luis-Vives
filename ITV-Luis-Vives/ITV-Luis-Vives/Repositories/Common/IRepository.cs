using CSharpFunctionalExtensions;
using ITV_Luis_Vives.Errors.Common;
using ITV_Luis_Vives.Models;

namespace ITV_Luis_Vives.Repositories.Common;

/// <summary>
///     Define las operaciones de búsqueda, persistencia y validación de identidad.
/// </summary>
public interface IRepository {
    /// <summary>
    ///     Obtiene todas las citas de forma paginada.
    /// </summary>
    IEnumerable<CitasVehiculos> GetAll(int page = 1, int pageSize = 10, bool includeDeleted = true);

    /// <summary>
    ///     Obtiene una cita por su ID.
    /// </summary>
    CitasVehiculos? GetById(int id);

    /// <summary>
    ///     Crea una nueva cita en el sistema.
    /// </summary>
    /// <returns>Result con la cita creada o error de dominio.</returns>
    Result<CitasVehiculos, DomainErrors> Create(CitasVehiculos citasVehiculos);

    /// <summary>
    ///     Actualiza una cita existente.
    /// </summary>
    /// <returns>Result con la cita actualizada o error de dominio.</returns>
    Result<CitasVehiculos, DomainErrors> Update(int id, CitasVehiculos citasVehiculos);

    /// <summary>
    ///     Elimina una cita.
    /// </summary>
    CitasVehiculos? Delete(int id, bool isLogical = true);

    /// <summary>
    ///     Realiza una búsqueda por la matrícula del vehículo.
    /// </summary>
    CitasVehiculos? GetByMatricula(string matricula);

    /// <summary>
    ///     Verifica si una matrícula ya se encuentra registrada.
    /// </summary>
    bool ExisteMatricula(string matricula);

    /// <summary>
    ///     Elimina todas las citas del sistema.
    /// </summary>
    bool DeleteAll();

    /// <summary>
    ///     Obtiene el número total de citas.
    /// </summary>
    int CountCitas(bool includeDeleted = false);

    /// <summary>
    ///     Obtiene citas ordenadas y paginadas (paginación real en BD).
    /// </summary>
    /// <param name="orden">Criterio de ordenación.</param>
    /// <param name="page">Número de página.</param>
    /// <param name="pageSize">Tamaño de página.</param>
    /// <param name="includeDeleted">Incluir eliminados.</param>
    /// <returns>Enumerable de citas.</returns>
    IEnumerable<CitasVehiculos> GetCitasOrderBy(string orden, int page = 1, int pageSize = 10,
        bool includeDeleted = true);

    /// <summary>
    ///     Restaura una cita eliminada lógicamente (IsDeleted = false, DeletedAt = null).
    /// </summary>
    Result<CitasVehiculos, DomainErrors> Restore(int id);
}