using FluentAssertions;
using ITV_Luis_Vives.Config;
using ITV_Luis_Vives.Enums;
using ITV_Luis_Vives.Errors.Citas;
using ITV_Luis_Vives.Models;
using ITV_Luis_Vives.Repositories.Memory;

namespace ITV_Luis_Vives.test.Repositories.Memory;

[TestFixture]
public class MemoryRepositoryTest {
    [TestFixture]
    public class CasosPositivos {
        [SetUp]
        public void SetUp() {
            _repository = new MemoryRepository(true, false);
            _citasVehiculos1 = new CitasVehiculos {
                Matricula = "4919BVS",
                Marca = "Seat",
                Modelo = "León",
                Cilindrada = 1900,
                Motor = Motor.Diesel,
                DniDueno = "12345678Z",
                FechaMatriculacion = new DateOnly(2002, 04, 20, AppConfig.Cultura.Calendar),
                FechaUltimaInspeccion = new DateOnly(2025, 05, 20, AppConfig.Cultura.Calendar),
                FechaProximaInspeccion = new DateOnly(2026, 05, 20, AppConfig.Cultura.Calendar)
            };
            _citasVehiculos2 = new CitasVehiculos {
                Matricula = "1531JHG",
                Marca = "Citroën",
                Modelo = "C4 Picasso",
                Cilindrada = 1600,
                Motor = Motor.Diesel,
                DniDueno = "68579893B",
                FechaMatriculacion = new DateOnly(2015, 07, 31, AppConfig.Cultura.Calendar),
                FechaUltimaInspeccion = new DateOnly(2025, 07, 26, AppConfig.Cultura.Calendar),
                FechaProximaInspeccion = new DateOnly(2026, 07, 31, AppConfig.Cultura.Calendar)
            };
        }

        private MemoryRepository _repository = null!;
        private CitasVehiculos _citasVehiculos1 = null!;
        private CitasVehiculos _citasVehiculos2 = null!;

        [Test]
        public void Create_CitaVehiculoValida_DeberiaCrearCorrectamente() {
            // Act
            var resultado = _repository.Create(_citasVehiculos1);

            // Assert
            resultado.IsSuccess.Should().BeTrue();
            resultado.Value.Id.Should().Be(1);
            resultado.Value.Matricula.Should().Be("4919BVS");
        }

        [Test]
        public void GetById_CuandoExiste_DeberiaRetornarCita() {
            // Arrange
            _repository.Create(_citasVehiculos1);

            // Act
            var resultado = _repository.GetById(1);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Id.Should().Be(1);
            resultado.Matricula.Should().Be("4919BVS");
        }

        [Test]
        public void GetByMatricula_CuandoExiste_DeberiaRetornarCita() {
            // Arrange
            _repository.Create(_citasVehiculos1);

            // Act
            var resultado = _repository.GetByMatricula("4919BVS");

            // Assert
            resultado.Should().NotBeNull();
            resultado.Matricula.Should().Be("4919BVS");
        }

        [Test]
        public void ExisteMatricula_CuandoExiste_DeberiaRetornarTrue() {
            // Arrange
            _repository.Create(_citasVehiculos1);

            // Act
            var resultado = _repository.ExisteMatricula("4919BVS");

            // Assert
            resultado.Should().BeTrue();
        }

        [Test]
        public void GetAll_SinParametros_DeberiaRetornarTodos() {
            // Arrange
            _repository.Create(_citasVehiculos1);
            _repository.Create(_citasVehiculos2);

            // Act
            var resultado = _repository.GetAll();

            // Assert
            resultado.Should().HaveCount(2);
        }

        [Test]
        public void GetAll_ConPaginacion_DeberiaRetornarPagina() {
            // Arrange
            for (var i = 1; i <= 5; i++)
                _repository.Create(new CitasVehiculos {
                    Matricula = $"{i:D4}HSK", Marca = $"Marca{i}", Modelo = "Modelo", Cilindrada = i * 1000,
                    Motor = Motor.Desconocido, DniDueno = $"{i:D8}K",
                    FechaMatriculacion = new DateOnly(2015, 07, 31, AppConfig.Cultura.Calendar),
                    FechaUltimaInspeccion = new DateOnly(2025, 07, 26, AppConfig.Cultura.Calendar),
                    FechaProximaInspeccion = new DateOnly(2026, 07, 31, AppConfig.Cultura.Calendar)
                });

            // Act
            var resultado = _repository.GetAll(1, 3);

            // Assert
            resultado.Should().HaveCount(3);
        }

        [Test]
        public void GetAll_SinIncluirBorrados_DeberiaRetornarSoloActivos() {
            // Arrange
            _repository.Create(_citasVehiculos1);
            var cita2 = _repository.Create(_citasVehiculos2).Value;
            _repository.Delete(cita2.Id);
            _repository.Create(new CitasVehiculos {
                Matricula = "6880GKN",
                Marca = "Ford",
                Modelo = "Fusión",
                Cilindrada = 1400,
                Motor = Motor.Gasolina,
                DniDueno = "07050667V",
                FechaMatriculacion = new DateOnly(2009, 01, 20, AppConfig.Cultura.Calendar),
                FechaUltimaInspeccion = new DateOnly(2026, 01, 17, AppConfig.Cultura.Calendar),
                FechaProximaInspeccion = new DateOnly(2027, 01, 20, AppConfig.Cultura.Calendar)
            });

            // Act
            var resultado = _repository.GetAll(includeDeleted: false).ToList();

            // Assert
            resultado.Should().HaveCount(2);
        }

        [Test]
        public void GetAll_IncluirBorrados_DeberiaRetornarTodos() {
            // Arrange
            _repository.Create(_citasVehiculos1);
            var cita2 = _repository.Create(_citasVehiculos2).Value;
            _repository.Delete(cita2.Id);
            _repository.Create(new CitasVehiculos {
                Matricula = "6880GKN",
                Marca = "Ford",
                Modelo = "Fusión",
                Cilindrada = 1400,
                Motor = Motor.Gasolina,
                DniDueno = "07050667V",
                FechaMatriculacion = new DateOnly(2009, 01, 20, AppConfig.Cultura.Calendar),
                FechaUltimaInspeccion = new DateOnly(2026, 01, 17, AppConfig.Cultura.Calendar),
                FechaProximaInspeccion = new DateOnly(2027, 01, 20, AppConfig.Cultura.Calendar)
            });

            // Act
            var resultado = _repository.GetAll(includeDeleted: true).ToList();

            // Assert
            resultado.Should().HaveCount(3);
        }
        
        [Test]
        public void GetCitasOrderBy_OrdenPorMatricula_DeberiaOrdenarDescendente() {
            // Arrange
            _repository.Create(_citasVehiculos1);
            _repository.Create(_citasVehiculos2);
            _repository.Create(new CitasVehiculos {
                Matricula = "6880GKN",
                Marca = "Ford",
                Modelo = "Fusión",
                Cilindrada = 1400,
                Motor = Motor.Gasolina,
                DniDueno = "07050667V",
                FechaMatriculacion = new DateOnly(2009, 01, 20, AppConfig.Cultura.Calendar),
                FechaUltimaInspeccion = new DateOnly(2026, 01, 17, AppConfig.Cultura.Calendar),
                FechaProximaInspeccion = new DateOnly(2027, 01, 20, AppConfig.Cultura.Calendar)
            });

            // Act
            var resultado = _repository.GetCitasOrderBy("matricula").ToList();

            // Assert
            resultado.Should().HaveCount(3);
            resultado[0].Matricula.Should().Be("1531JHG");
            resultado[1].Matricula.Should().Be("4919BVS");
            resultado[2].Matricula.Should().Be("6880GKN");
        }

        [Test]
        public void Update_ConDatosValidos_DeberiaActualizar() {
            // Arrange
            _repository.Create(_citasVehiculos1);

            var actualizado = new CitasVehiculos {
                Matricula = "4919BVS",
                Marca = "Ford",
                Modelo = "Focus C-Max",
                Cilindrada = 1600,
                Motor = Motor.Diesel,
                DniDueno = "12345678Z",
                FechaMatriculacion = new DateOnly(2002, 04, 20, AppConfig.Cultura.Calendar),
                FechaUltimaInspeccion = new DateOnly(2025, 05, 20, AppConfig.Cultura.Calendar),
                FechaProximaInspeccion = new DateOnly(2026, 05, 20, AppConfig.Cultura.Calendar)
            };

            // Act
            var resultado = _repository.Update(1, actualizado);

            // Assert
            resultado.IsSuccess.Should().BeTrue();
            resultado.Value.Marca.Should().Be("Ford");
            resultado.Value.Modelo.Should().Be("Focus C-Max");
            resultado.Value.Cilindrada.Should().Be(1600);
        }

        [Test]
        public void Delete_Logico_CuandoExiste_DeberiaRetornarCita() {
            // Arrange
            _repository.Create(_citasVehiculos1);

            // Act
            var resultado = _repository.Delete(1);

            // Assert
            resultado.Should().NotBeNull();
            resultado.IsDelete.Should().BeTrue();
        }

        [Test]
        public void Delete_Fisico_CuandoExiste_DeberiaRetornarCita() {
            // Arrange
            _repository.Create(_citasVehiculos1);

            // Act
            var resultado = _repository.Delete(1, false);

            // Assert
            resultado.Should().NotBeNull();
            _repository.GetById(1).Should().BeNull();
        }

        [Test]
        public void DeleteAll_CuandoHayDatos_DeberiaEliminarTodos() {
            // Arrange
            _repository.Create(_citasVehiculos1);
            _repository.Create(_citasVehiculos2);

            // Act
            var resultado = _repository.DeleteAll();

            // Assert
            resultado.Should().BeTrue();
            _repository.GetAll().Should().BeEmpty();
        }

        [Test]
        public void Restore_CuandoEliminadoLogicamente_DeberiaRestaurar() {
            // Arrange
            var citaCreada = _repository.Create(_citasVehiculos1).Value;
            _repository.Delete(citaCreada.Id);

            // Act
            var resultado = _repository.Restore(citaCreada.Id);

            // Assert
            resultado.IsSuccess.Should().BeTrue();
            resultado.Value.IsDelete.Should().BeFalse();
            resultado.Value.DeleteAt.Should().BeNull();
        }

        [Test]
        public void CountCitasVehiculos_SinEliminados_DeberiaContarSoloActivos() {
            // Arrange
            _repository.Create(_citasVehiculos1);
            var cita2 = _repository.Create(_citasVehiculos2).Value;
            _repository.Delete(cita2.Id);

            // Act
            var resultado = _repository.CountCitas();

            // Assert
            resultado.Should().Be(1);
        }

        [Test]
        public void CountCitasVehiculos_IncluyendoEliminados_DeberiaContarTodos() {
            // Arrange
            _repository.Create(_citasVehiculos1);
            var cita2 = _repository.Create(_citasVehiculos2).Value;
            _repository.Delete(cita2.Id);

            // Act
            var resultado = _repository.CountCitas(true);

            // Assert
            resultado.Should().Be(2);
        }
    }

    [TestFixture]
    public class CasosNegativos {
        [SetUp]
        public void SetUp() {
            _repository = new MemoryRepository(true, false);
            _citasVehiculos1 = new CitasVehiculos {
                Matricula = "4919BVS",
                Marca = "Seat",
                Modelo = "León",
                Cilindrada = 1900,
                Motor = Motor.Diesel,
                DniDueno = "12345678Z",
                FechaMatriculacion = new DateOnly(2002, 04, 20, AppConfig.Cultura.Calendar),
                FechaUltimaInspeccion = new DateOnly(2025, 05, 20, AppConfig.Cultura.Calendar),
                FechaProximaInspeccion = new DateOnly(2026, 05, 20, AppConfig.Cultura.Calendar)
            };
        }

        private MemoryRepository _repository = null!;
        private CitasVehiculos _citasVehiculos1 = null!;

        [Test]
        public void Create_ConDniExistente_DeberiaRetornarFailure() {
            // Arrange
            var citasVehiculos1 = _citasVehiculos1;
            var citasVehiculos2 = new CitasVehiculos {
                Matricula = "4919BVS",
                Marca = "Citroën",
                Modelo = "C4 Picasso",
                Cilindrada = 1600,
                Motor = Motor.Diesel,
                DniDueno = "68579893B",
                FechaMatriculacion = new DateOnly(2015, 07, 31, AppConfig.Cultura.Calendar),
                FechaUltimaInspeccion = new DateOnly(2025, 07, 26, AppConfig.Cultura.Calendar),
                FechaProximaInspeccion = new DateOnly(2026, 07, 31, AppConfig.Cultura.Calendar)
            };
            _repository.Create(citasVehiculos1);

            // Act
            var resultado = _repository.Create(citasVehiculos2);

            // Assert
            resultado.IsFailure.Should().BeTrue();
            resultado.Error.Should().BeOfType<CitaErrors.MatriculaAlreadyExists>();
            (resultado.Error as CitaErrors.MatriculaAlreadyExists)?.Matricula.Should().Be("4919BVS");
        }

        [Test]
        public void GetById_CuandoNoExiste_DeberiaRetornarNull() {
            // Act
            var resultado = _repository.GetById(999);

            // Assert
            resultado.Should().BeNull();
        }

        [Test]
        public void GetByDni_CuandoNoExiste_DeberiaRetornarNull() {
            // Act
            var resultado = _repository.GetByMatricula("1531JHG");

            // Assert
            resultado.Should().BeNull();
        }

        [Test]
        public void ExisteDni_CuandoNoExiste_DeberiaRetornarFalse() {
            // Act
            var resultado = _repository.ExisteMatricula("1531JHG");

            // Assert
            resultado.Should().BeFalse();
        }

        [Test]
        public void Update_CuandoNoExiste_DeberiaRetornarFailure() {
            // Act
            var resultado = _repository.Update(999, _citasVehiculos1);

            // Assert
            resultado.IsFailure.Should().BeTrue();
            resultado.Error.Should().BeOfType<CitaErrors.NotFound>();
            (resultado.Error as CitaErrors.NotFound)?.Id.Should().Be("999");
        }

        [Test]
        public void Update_ConDniExistenteEnOtro_DeberiaRetornarFailure() {
            // Arrange
            var citasVehiculos2 = new CitasVehiculos {
                Matricula = "1531JHG",
                Marca = "Citroën",
                Modelo = "C4 Picasso",
                Cilindrada = 1600,
                Motor = Motor.Diesel,
                DniDueno = "68579893B",
                FechaMatriculacion = new DateOnly(2015, 07, 31, AppConfig.Cultura.Calendar),
                FechaUltimaInspeccion = new DateOnly(2025, 07, 26, AppConfig.Cultura.Calendar),
                FechaProximaInspeccion = new DateOnly(2026, 07, 31, AppConfig.Cultura.Calendar)
            };
            _repository.Create(_citasVehiculos1);
            _repository.Create(citasVehiculos2);

            // Act
            var resultado = _repository.Update(2, _citasVehiculos1);

            // Assert
            resultado.IsFailure.Should().BeTrue();
            resultado.Error.Should().BeOfType<CitaErrors.MatriculaAlreadyExists>();
            (resultado.Error as CitaErrors.MatriculaAlreadyExists)?.Matricula.Should().Be("4919BVS");
        }

        [Test]
        public void Delete_CuandoNoExiste_DeberiaRetornarNull() {
            // Act
            var resultado = _repository.Delete(999);

            // Assert
            resultado.Should().BeNull();
        }

        [Test]
        public void Restore_CuandoNoExiste_DeberiaRetornarFailure() {
            // Act
            var resultado = _repository.Restore(999);

            // Assert
            resultado.IsFailure.Should().BeTrue();
            resultado.Error.Should().BeOfType<CitaErrors.NotFound>();
        }
    }
}