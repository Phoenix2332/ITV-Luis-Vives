using ITV_Luis_Vives.Config;
using ITV_Luis_Vives.Enums;
using ITV_Luis_Vives.Models;

namespace ITV_Luis_Vives.Factory;

/// <summary>
///     Factoría con datos semilla fijos.
/// </summary>
public static class CitasFactory {
    /// <summary>
    ///     Genera la semilla de datos inicial.
    /// </summary>
    /// <returns>Enumerable con datos de demostración</returns>
    public static IEnumerable<CitasVehiculos> Seed() {
        var lista = new List<CitasVehiculos> {
            new() {
                Id = 1,
                Matricula = "4919BVS",
                Marca = "Seat",
                Modelo = "León",
                Cilindrada = 1900,
                Motor = Motor.Diesel,
                DniDueño = "12345678Z",
                FechaMatriculacion = new DateOnly(2002, 04, 19, AppConfig.Cultura.Calendar),
                FechaUltimaInspeccion = new DateOnly(2025, 04, 16, AppConfig.Cultura.Calendar),
                FechaProximaInspeccion = new DateOnly(2026, 04, 19, AppConfig.Cultura.Calendar),
                DeleteAt = DateTime.UtcNow
            },
            new() {
                Id = 2,
                Matricula = "1531JHG",
                Marca = "Citroën",
                Modelo = "C4 Picasso",
                Cilindrada = 1600,
                Motor = Motor.Diesel,
                DniDueño = "68579893B",
                FechaMatriculacion = new DateOnly(2015, 07, 31, AppConfig.Cultura.Calendar),
                FechaUltimaInspeccion = new DateOnly(2025, 07, 26, AppConfig.Cultura.Calendar),
                FechaProximaInspeccion = new DateOnly(2026, 07, 31, AppConfig.Cultura.Calendar)
            },
            new() {
                Id = 3,
                Matricula = "6880GKN",
                Marca = "Ford",
                Modelo = "Fusión",
                Cilindrada = 1400,
                Motor = Motor.Gasolina,
                DniDueño = "07050667V",
                FechaMatriculacion = new DateOnly(2009, 01, 20, AppConfig.Cultura.Calendar),
                FechaUltimaInspeccion = new DateOnly(2026, 01, 17, AppConfig.Cultura.Calendar),
                FechaProximaInspeccion = new DateOnly(2027, 01, 20, AppConfig.Cultura.Calendar)
            },
            new() {
                Id = 4,
                Matricula = "9195DDB",
                Marca = "Peugeot",
                Modelo = "206",
                Cilindrada = 1400,
                Motor = Motor.Gasolina,
                DniDueño = "72898549A",
                FechaMatriculacion = new DateOnly(2004, 07, 24, AppConfig.Cultura.Calendar),
                FechaUltimaInspeccion = new DateOnly(2025, 07, 14, AppConfig.Cultura.Calendar),
                FechaProximaInspeccion = new DateOnly(2026, 07, 24, AppConfig.Cultura.Calendar)
            },
            new() {
                Id = 5,
                Matricula = "6532MLN",
                Marca = "Renault",
                Modelo = "5",
                Cilindrada = 1400,
                Motor = Motor.Gasolina,
                DniDueño = "07508636X",
                FechaMatriculacion = new DateOnly(1983, 09, 12, AppConfig.Cultura.Calendar),
                FechaUltimaInspeccion = new DateOnly(2025, 09, 10, AppConfig.Cultura.Calendar),
                FechaProximaInspeccion = new DateOnly(2026, 09, 12, AppConfig.Cultura.Calendar)
            },
            new() {
                Id = 6,
                Matricula = "4572CWZ",
                Marca = "Ford",
                Modelo = "Focus C-Max",
                Cilindrada = 1600,
                Motor = Motor.Diesel,
                DniDueño = "53280457Z",
                FechaMatriculacion = new DateOnly(2003, 05, 28, AppConfig.Cultura.Calendar),
                FechaUltimaInspeccion = new DateOnly(2014, 05, 26, AppConfig.Cultura.Calendar),
                FechaProximaInspeccion = new DateOnly(2015, 05, 28, AppConfig.Cultura.Calendar),
                DeleteAt = DateTime.UtcNow
            },
            new() {
                Id = 7,
                Matricula = "6697MSG",
                Marca = "Citröen",
                Modelo = "ZX",
                Cilindrada = 1400,
                Motor = Motor.Gasolina,
                DniDueño = "81137877H",
                FechaMatriculacion = new DateOnly(1995, 11, 16, AppConfig.Cultura.Calendar),
                FechaUltimaInspeccion = new DateOnly(2008, 11, 14, AppConfig.Cultura.Calendar),
                FechaProximaInspeccion = new DateOnly(2009, 11, 16, AppConfig.Cultura.Calendar),
                DeleteAt = DateTime.UtcNow
            }
        };

        return lista;
    }
}