using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ITV_Luis_Vives.Enums;
using Microsoft.EntityFrameworkCore;

namespace ITV_Luis_Vives.Entities;

/// <summary>
///     Entidad de base de datos.
/// </summary>
[Table("CitasVehiculos")]
[Index(nameof(Matricula), IsUnique = true)]
[Index(nameof(Motor))]
[Index(nameof(DniDueno))]
[Index(nameof(IsDelete))]
public class CitaEntity {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required] [MaxLength(8)] public string Matricula { get; set; } = string.Empty;

    [Required] [MaxLength(50)] public string Marca { get; set; } = string.Empty;

    [Required] [MaxLength(100)] public string Modelo { get; set; } = string.Empty;

    public int Cilindrada { get; set; }

    [Required] public Motor Motor { get; set; }

    [Required] [MaxLength(9)] public string DniDueno { get; set; } = string.Empty;

    [Column(TypeName = "datetime")] public DateOnly FechaMatriculacion { get; set; }

    [Column(TypeName = "datetime")] public DateOnly FechaUltimaInspeccion { get; set; }

    [Column(TypeName = "datetime")] public DateOnly FechaProximaInspeccion { get; set; }

    public bool IsInspeccionApta => DateOnly.FromDateTime(DateTime.Today) <= FechaProximaInspeccion;

    [Column(TypeName = "datetime")] public DateTime CreateAt { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "datetime")] public DateTime UpdateAt { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "datetime")] public DateTime? DeleteAt { get; set; }

    public bool IsDelete => DeleteAt != null;
}