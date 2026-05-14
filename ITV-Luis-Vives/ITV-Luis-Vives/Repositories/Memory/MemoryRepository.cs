using CSharpFunctionalExtensions;
using ITV_Luis_Vives.Config;
using ITV_Luis_Vives.Entities;
using ITV_Luis_Vives.Errors.Citas;
using ITV_Luis_Vives.Errors.Common;
using ITV_Luis_Vives.Factory;
using ITV_Luis_Vives.Mappers;
using ITV_Luis_Vives.Models;
using ITV_Luis_Vives.Repositories.Common;
using Serilog;

namespace ITV_Luis_Vives.Repositories.Memory;

/// <summary>
///     Repositorio en memoria.
///     Utiliza diccionarios para almacenamiento rápido.
/// </summary>
public class MemoryRepository : IRepository {
    private readonly ILogger _logger = Log.ForContext<MemoryRepository>();
    private readonly Dictionary<string, int> _matriculaIndex = [];
    private readonly Dictionary<int, CitaEntity> _porId = [];
    private int _idCounter;

    /// <summary>
    ///     Constructor delegado que usa la configuración de la aplicación.
    /// </summary>
    public MemoryRepository() : this(AppConfig.DropData, AppConfig.SeedData) {
    }

    /// <summary>
    ///     Constructor principal que contiene la lógica de inicialización necesaria.
    /// </summary>
    public MemoryRepository(bool dropData, bool seedData) {
        if (dropData) {
            _logger.Warning("Borrando datos en memoria...");
            DeleteAll();
        }

        if (seedData) {
            _logger.Information("Cargando datos de semilla...");
            foreach (var persona in CitasFactory.Seed()) Create(persona);
            _logger.Information("SeedData completado.");
        }
    }

    /// <inheritdoc />
    public IEnumerable<CitasVehiculos> GetAll(int page = 1, int pageSize = 10, bool includeDeleted = true) {
        _logger.Debug(
            "Obteniendo citas: página {Page}, tamaño {PageSize}, incluir borrados: {IncludeDeleted}",
            page, pageSize, includeDeleted
        );

        var query = includeDeleted
            ? _porId.Values.AsEnumerable()
            : _porId.Values.Where(e => !e.IsDelete);

        return query
            .OrderBy(e => e.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToModel();
    }

    /// <inheritdoc />
    public CitasVehiculos? GetById(int id) {
        _logger.Debug("Obteniendo cita con ID {Id}", id);
        return _porId.GetValueOrDefault(id).ToModel();
    }

    /// <inheritdoc />
    public CitasVehiculos? GetByMatricula(string matricula) {
        _logger.Debug("Obteniendo persona con Matricula {Matricula}", matricula);
        return _matriculaIndex.TryGetValue(matricula, out var id) && _porId.TryGetValue(id, out var entity)
            ? entity.ToModel()
            : null;
    }

    /// <inheritdoc />
    public bool ExisteMatricula(string matricula) {
        return _matriculaIndex.ContainsKey(matricula);
    }

    /// <inheritdoc />
    public Result<CitasVehiculos, DomainErrors> Create(CitasVehiculos citasVehiculos) {
        _logger.Debug("Creando nueva cita {Matricula}", citasVehiculos.Matricula);

        if (ExisteMatricula(citasVehiculos.Matricula)) {
            _logger.Warning("No se puede crear: Matricula {Matricula} ya existe", citasVehiculos.Matricula);
            return Result.Failure<CitasVehiculos, DomainErrors>(CitaError.MatriculaAlreadyExists(citasVehiculos.Matricula));
        }

        citasVehiculos = citasVehiculos with {
            Id = ++_idCounter,
            CreateAt = DateTime.UtcNow,
            UpdateAt = DateTime.UtcNow,
            DeleteAt = null
        };

        var entity = citasVehiculos.ToEntity();
        _porId[entity.Id] = entity;
        _matriculaIndex[entity.Matricula] = entity.Id;

        _logger.Information("Cita creada con ID {Id}", entity.Id);
        return Result.Success<CitasVehiculos, DomainErrors>(entity.ToModel()!);
    }

    /// <inheritdoc />
    public Result<CitasVehiculos, DomainErrors> Update(int id, CitasVehiculos citasVehiculos) {
        _logger.Debug("Actualizando cita con ID {Id}", id);

        if (!_porId.TryGetValue(id, out var actual)) {
            _logger.Warning("No se puede actualizar: cita con ID {Id} no encontrada", id);
            return Result.Failure<CitasVehiculos, DomainErrors>(CitaError.NotFound(id.ToString()));
        }

        if (citasVehiculos.Matricula != actual.Matricula && _matriculaIndex.TryGetValue(citasVehiculos.Matricula, out var otroId)
                                                         && otroId != id) {
            _logger.Warning("No se puede actualizar: Matricula {Matricula} ya está en uso", citasVehiculos.Matricula);
            return Result.Failure<CitasVehiculos, DomainErrors>(CitaError.MatriculaAlreadyExists(citasVehiculos.Matricula));
        }

        citasVehiculos = citasVehiculos with {
            Id = id,
            CreateAt = actual.CreateAt,
            UpdateAt = DateTime.UtcNow,
            DeleteAt = actual.DeleteAt
        };

        var entity = citasVehiculos.ToEntity();
        _porId[id] = entity;

        if (actual.Matricula != entity.Matricula) {
            _matriculaIndex.Remove(actual.Matricula);
            _matriculaIndex[entity.Matricula] = id;
        }

        _logger.Information("Cita con ID {Id} actualizada correctamente", id);
        return Result.Success<CitasVehiculos, DomainErrors>(entity.ToModel()!);
    }

    /// <inheritdoc />
    public CitasVehiculos? Delete(int id, bool isLogical = true) {
        _logger.Debug("Eliminando cita con ID {Id} (borrado lógico: {IsLogical})", id, isLogical);

        if (!_porId.TryGetValue(id, out var entity)) {
            _logger.Warning("No se puede eliminar: cita con ID {Id} no encontrada", id);
            return null;
        }

        if (isLogical) {
            entity.DeleteAt = DateTime.UtcNow;
            entity.UpdateAt = DateTime.UtcNow;
            _logger.Information("Borrado lógico de cita con ID {Id}", id);
            return entity.ToModel();
        }

        _porId.Remove(id);
        _matriculaIndex.Remove(entity.Matricula);
        _logger.Information("Borrado físico de cita con ID {Id}", id);
        return entity.ToModel();
    }

    /// <inheritdoc />
    public bool DeleteAll() {
        _logger.Warning("Eliminando permanentemente todas las citas");
        _porId.Clear();
        _matriculaIndex.Clear();
        _idCounter = 0;
        return true;
    }

    /// <inheritdoc />
    public int CountCitas(bool includeDeleted = false) {
        var query = includeDeleted
            ? _porId.Values.AsEnumerable()
            : _porId.Values.Where(e => !e.IsDelete);
        return query.Count();
    }

    /// <inheritdoc />
    public IEnumerable<CitasVehiculos> GetCitasOrderBy(string orden, int page = 1, int pageSize = 10,
        bool includeDeleted = true) {
        var query = includeDeleted
            ? _porId.Values.AsEnumerable()
            : _porId.Values.Where(e => !e.IsDelete);

        query = orden.ToLower() switch {
            "id" => query.OrderBy(e => e.Id),
            "matricula" => query.OrderBy(e => e.Matricula),
            "dniDueno" => query.OrderBy(e => e.DniDueno),
            "marca" => query.OrderBy(e => e.Marca),
            "modelo" => query.OrderByDescending(e => e.Modelo),
            "motor" => query.OrderBy(e => e.Motor),
            "fechaMatriculacion" => query.OrderBy(e => e.FechaMatriculacion),
            "fechaUltimaInspeccion" => query.OrderBy(e => e.FechaUltimaInspeccion),
            "fechaProximaInspeccion" => query.OrderBy(e => e.FechaProximaInspeccion),
            _ => query.OrderBy(e => e.Matricula)
        };

        return query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToModel();
    }

    /// <inheritdoc />
    public Result<CitasVehiculos, DomainErrors> Restore(int id) {
        if (!_porId.TryGetValue(id, out var entity)) {
            _logger.Warning("No se puede restaurar: cita con ID {Id} no encontrada", id);
            return Result.Failure<CitasVehiculos, DomainErrors>(CitaError.NotFound(id.ToString()));
        }

        var restored = new CitaEntity {
            Id = entity.Id,
            Matricula = entity.Matricula,
            Marca = entity.Marca,
            Modelo = entity.Modelo,
            Cilindrada = entity.Cilindrada,
            Motor = entity.Motor,
            DniDueno = entity.DniDueno,
            FechaMatriculacion = entity.FechaMatriculacion,
            FechaUltimaInspeccion = entity.FechaUltimaInspeccion,
            FechaProximaInspeccion = entity.FechaProximaInspeccion
        };
        _porId[id] = restored;

        _matriculaIndex[restored.Matricula] = id;

        _logger.Information("Cita con ID {Id} restaurada correctamente", id);
        return Result.Success<CitasVehiculos, DomainErrors>(restored.ToModel()!);
    }
}