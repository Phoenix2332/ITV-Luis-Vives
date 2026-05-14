using ITV_Luis_Vives.Config;
using ITV_Luis_Vives.Dto;
using ITV_Luis_Vives.Entities;
using ITV_Luis_Vives.Enums;
using ITV_Luis_Vives.Models;

namespace ITV_Luis_Vives.Mappers;

public static class CitaMapper {
    private const string _dateFormat = "d";
    private const string _dateTimeFormat = "s";

    public static CitasVehiculos? ToModel(this CitaEntity? entity) {
        if (entity == null) return null;

        return new CitasVehiculos {
            Id = entity.Id,
            Matricula = entity.Matricula,
            Marca = entity.Marca,
            Modelo = entity.Modelo,
            Cilindrada = entity.Cilindrada,
            Motor = entity.Motor,
            DniDueno = entity.DniDueno,
            FechaMatriculacion = entity.FechaMatriculacion,
            FechaUltimaInspeccion = entity.FechaUltimaInspeccion,
            FechaProximaInspeccion = entity.FechaProximaInspeccion,
            CreateAt = entity.CreateAt,
            UpdateAt = entity.UpdateAt,
            DeleteAt = entity.DeleteAt
        };
    }

    public static IEnumerable<CitasVehiculos> ToModel(this IEnumerable<CitaEntity> entities) {
        return entities.Select(ToModel).OfType<CitasVehiculos>();
    }

    public static CitaEntity ToEntity(this CitasVehiculos model) {
        return new CitaEntity {
            Id = model.Id,
            Matricula = model.Matricula,
            Marca = model.Marca,
            Modelo = model.Modelo,
            Cilindrada = model.Cilindrada,
            Motor = model.Motor,
            DniDueno = model.DniDueno,
            FechaMatriculacion = model.FechaMatriculacion,
            FechaUltimaInspeccion = model.FechaUltimaInspeccion,
            FechaProximaInspeccion = model.FechaProximaInspeccion,
            CreateAt = model.CreateAt,
            UpdateAt = model.UpdateAt,
            DeleteAt = model.DeleteAt
        };
    }

    public static CitaDto ToDto(this CitasVehiculos dto) {
        return new CitaDto(
            dto.Id,
            dto.Matricula,
            dto.Marca,
            dto.Modelo,
            dto.Cilindrada,
            dto.Motor.ToString(),
            dto.DniDueno,
            dto.FechaMatriculacion.ToString(_dateFormat, AppConfig.Cultura),
            dto.FechaUltimaInspeccion.ToString(_dateFormat, AppConfig.Cultura),
            dto.FechaProximaInspeccion.ToString(_dateFormat, AppConfig.Cultura),
            dto.IsInspeccionApta,
            dto.CreateAt.ToString(_dateTimeFormat, AppConfig.Cultura),
            dto.UpdateAt.ToString(_dateTimeFormat, AppConfig.Cultura),
            dto.DeleteAt?.ToString(_dateTimeFormat, AppConfig.Cultura),
            dto.IsDelete
        );
    }

    public static CitasVehiculos ToModel(this CitaDto dto) {
        return new CitasVehiculos {
            Id = dto.Id,
            Matricula = dto.Matricula,
            Marca = dto.Marca,
            Modelo = dto.Modelo,
            Cilindrada = dto.Cilindrada,
            Motor = Enum.TryParse(dto.Motor, out Motor motor) ? motor : Motor.Desconocido,
            DniDueno = dto.DniDueno,
            FechaMatriculacion = DateOnly.Parse(dto.FechaMatriculacion, AppConfig.Cultura),
            FechaUltimaInspeccion = DateOnly.Parse(dto.FechaUltimaInspeccion, AppConfig.Cultura),
            FechaProximaInspeccion = DateOnly.Parse(dto.FechaProximaInspeccion, AppConfig.Cultura),
            CreateAt = DateTime.Parse(dto.CreateAt, AppConfig.Cultura),
            UpdateAt = DateTime.Parse(dto.UpdateAt, AppConfig.Cultura),
            DeleteAt = string.IsNullOrEmpty(dto.DeleteAt)
                ? null
                : DateTime.Parse(dto.DeleteAt, AppConfig.Cultura)
        };
    }
}