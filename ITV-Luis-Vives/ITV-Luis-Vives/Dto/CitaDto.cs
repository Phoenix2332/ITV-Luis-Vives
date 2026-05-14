namespace ITV_Luis_Vives.Dto;

/// <summary>
///     Objeto de Transferencia de Datos.
/// </summary>
public record CitaDto(
    int Id,
    string Matricula,
    string Marca,
    string Modelo,
    int Cilindrada,
    string Motor,
    string DniDueno,
    string FechaMatriculacion,
    string FechaUltimaInspeccion,
    string FechaProximaInspeccion,
    bool IsInspeccionApta,
    string CreateAt,
    string UpdateAt,
    string? DeleteAt,
    bool IsDelete
) {
    public CitaDto() : this(0, "", "", "", 0, "", "",
        "", "", "", false, "",
        "", null, false) {
    }
}