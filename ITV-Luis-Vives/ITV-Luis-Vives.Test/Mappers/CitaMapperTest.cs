using FluentAssertions;
using ITV_Luis_Vives.Config;
using ITV_Luis_Vives.Dto;
using ITV_Luis_Vives.Entities;
using ITV_Luis_Vives.Enums;
using ITV_Luis_Vives.Mappers;
using ITV_Luis_Vives.Models;

namespace ITV_Luis_Vives.test.Mappers;

[TestFixture]
public class CitaMapperTest {
    [TestFixture]
    public class CasosPositivos {
        [SetUp]
        public void SetUp() {
            _citasVehiculo = new CitasVehiculos {
                Id = 1,
                Matricula = "4919BVS",
                Marca = "Seat",
                Modelo = "León",
                Cilindrada = 1900,
                Motor = Motor.Diesel,
                DniDueno = "12345678Z",
                FechaMatriculacion = new DateOnly(2002, 04, 20, AppConfig.Cultura.Calendar),
                FechaUltimaInspeccion = new DateOnly(2025, 05, 20, AppConfig.Cultura.Calendar),
                FechaProximaInspeccion = new DateOnly(2026, 05, 20, AppConfig.Cultura.Calendar),
                CreateAt = new DateTime(2024, 1, 1, 10, 0, 0),
                UpdateAt = new DateTime(2024, 1, 2, 12, 0, 0),
                DeleteAt = null
            };

            _dtoCita = new CitaDto(
                1, "4919BVS", "Seat", "León",
                1900, "Diesel", "12345678Z",
                "20/4/2002", "20/5/2025", "20/5/2026", true,
                "2024-01-01T10:00:00", "2024-01-02T12:00:00", null, false
            );

            _entityCita = new CitaEntity {
                Id = 1,
                Matricula = "4919BVS",
                Marca = "Seat",
                Modelo = "León",
                Cilindrada = 1900,
                Motor = Motor.Diesel,
                DniDueno = "12345678Z",
                FechaMatriculacion = new DateOnly(2002, 04, 20, AppConfig.Cultura.Calendar),
                FechaUltimaInspeccion = new DateOnly(2025, 05, 20, AppConfig.Cultura.Calendar),
                FechaProximaInspeccion = new DateOnly(2026, 05, 20, AppConfig.Cultura.Calendar),
                CreateAt = new DateTime(2024, 1, 1, 10, 0, 0),
                UpdateAt = new DateTime(2024, 1, 2, 12, 0, 0),
                DeleteAt = null
            };
        }

        private CitasVehiculos _citasVehiculo = null!;
        private CitaDto _dtoCita = null!;
        private CitaEntity _entityCita = null!;

        [Test]
        public void ToDto_CitasVehiculos_DeberiaConvertirCorrectamente() {
            // Act
            var resultado = _citasVehiculo.ToDto();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Id.Should().Be(1);
            resultado.Matricula.Should().Be("4919BVS");
            resultado.Marca.Should().Be("Seat");
            resultado.Modelo.Should().Be("León");
            resultado.Cilindrada.Should().Be(1900);
            resultado.Motor.Should().Be("Diesel");
            resultado.DniDueno.Should().Be("12345678Z");
            resultado.FechaMatriculacion.Should().Be("20/4/2002");
            resultado.FechaUltimaInspeccion.Should().Be("20/5/2025");
            resultado.FechaProximaInspeccion.Should().Be("20/5/2026");
            resultado.IsInspeccionApta.Should().Be(true);
        }

        [Test]
        public void ToModel_DtoCita_DeberiaConvertirCorrectamente() {
            // Act
            var resultado = _dtoCita.ToModel();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Id.Should().Be(1);
            resultado.Matricula.Should().Be("4919BVS");
            resultado.Marca.Should().Be("Seat");
            resultado.Modelo.Should().Be("León");
            resultado.Cilindrada.Should().Be(1900);
            resultado.Motor.Should().Be(Motor.Diesel);
            resultado.DniDueno.Should().Be("12345678Z");
        }

        [Test]
        public void ToModel_EntityCita_DeberiaConvertirCorrectamente() {
            // Act
            var resultado = _entityCita.ToModel();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Id.Should().Be(1);
            resultado.Matricula.Should().Be("4919BVS");
            resultado.Marca.Should().Be("Seat");
            resultado.Modelo.Should().Be("León");
            resultado.Cilindrada.Should().Be(1900);
            resultado.Motor.Should().Be(Motor.Diesel);
            resultado.DniDueno.Should().Be("12345678Z");
        }

        [Test]
        public void ToEntity_CitasVehiculos_DeberiaConvertirCorrectamente() {
            // Act
            var resultado = _citasVehiculo.ToEntity();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Id.Should().Be(1);
            resultado.Matricula.Should().Be("4919BVS");
            resultado.Marca.Should().Be("Seat");
            resultado.Modelo.Should().Be("León");
            resultado.Cilindrada.Should().Be(1900);
            resultado.Motor.Should().Be(Motor.Diesel);
            resultado.DniDueno.Should().Be("12345678Z");
        }

        [Test]
        public void ToModel_ListaEntities_DeberiaConvertirTodos() {
            // Arrange
            var entities = new List<CitaEntity> { _entityCita };

            // Act
            var resultado = entities.ToModel();

            // Assert
            resultado.Should().HaveCount(1);
        }

        [Test]
        public void ToModel_EntityNulo_DeberiaRetornarNull() {
            // Act
            var resultado = ((CitaEntity?)null).ToModel();

            // Assert
            resultado.Should().BeNull();
        }
    }

    [TestFixture]
    public class CasosNegativos {
        [Test]
        public void ToModel_DtoCita_ConMotorInvalido_DeberiaUsarValoresPorDefecto() {
            // Arrange
            var dto = new CitaDto(
                1, "4919BVS", "Seat", "León",
                1900, "GAS_NATURAL", "12345678Z",
                "20/04/2002", "20/05/2025", "20/05/2026", true,
                "2024-01-01T10:00:00", "2024-01-02T12:00:00", null, false
            );

            // Act
            var resultado = dto.ToModel();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Motor.Should().Be(Motor.Desconocido);
        }
    }
}