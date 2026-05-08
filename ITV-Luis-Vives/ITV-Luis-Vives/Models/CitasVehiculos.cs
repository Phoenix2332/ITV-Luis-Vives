using ITV_Luis_Vives.Enums;

namespace ITV_Luis_Vives.Models;

/// <summary>
///     Representa una cita en el sistema.
/// </summary>
public record CitasVehiculos {
    public int Id { get; init; }
    public string Matricula { get; set; } = string.Empty;
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public int Cilindrada { get; set; }
    public Motor Motor { get; set; }
    public string DniDueño { get; set; } = string.Empty;
    public DateOnly FechaMatriculacion { get; set; }
    public DateOnly FechaUltimaInspeccion { get; set; }
    public DateOnly FechaProximaInspeccion { get; set; }
    public bool IsInspeccionApta => DateOnly.FromDateTime(DateTime.Today) <= FechaProximaInspeccion;
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeleteAt { get; set; }
    public bool IsDelete { get; set; }

    /// <summary>
    ///     Determina si dos citas son idénticas comparando las matrículas de los vehículos.
    /// </summary>
    /// <param name="other">Instancia de cita a comparar.</param>
    /// <returns>True si los DNIs coinciden.</returns>
    public virtual bool Equals(CitasVehiculos? other) {
        return other is not null && string.Equals(Matricula, other.Matricula, StringComparison.OrdinalIgnoreCase);
    }

    public string ItvApta() {
        return IsInspeccionApta ? "Apta" : "No Apta";
    }

    public override string ToString() {
        return $"Cita: [Id-{Id}, Matricula-{Matricula}, Marca-{Marca}, Modelo-{Modelo}, Cilindrada-{Cilindrada}, " +
               $"Motor-{Motor.ToString()}, Dni dueño-{DniDueño}, Fecha matriculacion-{FechaMatriculacion}, " +
               $"Fecha última inspeccion-{FechaUltimaInspeccion}, Fecha próxima inspección-{FechaProximaInspeccion}" +
               $"Estado de la inspección-{ItvApta()}, Fecha de creacion-{CreateAt}, " +
               $"Fecha de última modificacion-{UpdateAt}, Fecha de eliminación-{DeleteAt}, " +
               $"Ha sido eliminado-{IsDelete}]";
    }

    /// <summary>
    ///     Calcula el código hash basado exclusivamente en la matrícula para mantener coherencia con la igualdad.
    /// </summary>
    /// <returns>Código hash entero.</returns>
    public override int GetHashCode() {
        return HashCode.Combine(Matricula.ToLowerInvariant());
    }
}