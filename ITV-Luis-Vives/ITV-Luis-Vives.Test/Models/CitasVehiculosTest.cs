using FluentAssertions;
using ITV_Luis_Vives.Config;
using ITV_Luis_Vives.Enums;
using ITV_Luis_Vives.Models;

namespace ITV_Luis_Vives.test.Models;

[TestFixture]
public class CitasVehiculosTest {
    [TestFixture]
    public class CasosPositivos {
        [Test]
        public void ToString_DeberiaRetornarFormatoCorrecto() {
            // Arrange
            var citasVehiculos = new CitasVehiculos {
                Id = 1,
                Matricula = "4919BVS",
                Marca = "Seat",
                Modelo = "León",
                Cilindrada = 1900,
                Motor = Motor.Diesel,
                DniDueño = "12345678Z",
                FechaMatriculacion = new DateOnly(2002, 04, 20, AppConfig.Cultura.Calendar),
                FechaInspeccion = new DateOnly(2026, 05, 20, AppConfig.Cultura.Calendar)
            };

            // Act
            var resultado = citasVehiculos.ToString();

            // Assert
            resultado.Should().Contain("4919BVS");
            resultado.Should().Contain("Seat");
            resultado.Should().Contain("León");
            resultado.Should().Contain("1900");
            resultado.Should().Contain("Diesel");
            resultado.Should().Contain("12345678Z");
            resultado.Should().Contain("20/04/2002");
            resultado.Should().Contain("20/05/2026");
        }

        [Test]
        public void Equals_MismaMatricula_DeberiaSerIgual() {
            // Arrange
            var citasVehiculos1 = new CitasVehiculos { Matricula = "4919BVS" };
            var citasVehiculos2 = new CitasVehiculos { Matricula = "4919BVS" };

            // Act
            var resultado = citasVehiculos1.Equals(citasVehiculos2);

            // Assert
            resultado.Should().BeTrue();
        }

        [Test]
        public void Equals_MatriculaDiferente_DeberiaSerDistinto() {
            // Arrange
            var citasVehiculos1 = new CitasVehiculos { Matricula = "4919BVS" };
            var citasVehiculos2 = new CitasVehiculos { Matricula = "1531JHG" };

            // Act
            var resultado = citasVehiculos1.Equals(citasVehiculos2);

            // Assert
            resultado.Should().BeFalse();
        }

        [Test]
        public void GetHashCode_MismaMatricula_MismoHashCode() {
            // Arrange
            var citasVehiculos1 = new CitasVehiculos { Matricula = "4919BVS" };
            var citasVehiculos2 = new CitasVehiculos { Matricula = "4919bvs" };

            // Act
            var hash1 = citasVehiculos1.GetHashCode();
            var hash2 = citasVehiculos2.GetHashCode();

            // Assert
            hash1.Should().Be(hash2);
        }
    }

    [TestFixture]
    public class CasosNegativos {
        [Test]
        public void Equals_Nulo_DeberiaRetornarFalse() {
            // Arrange
            var citasVehiculos1 = new CitasVehiculos { Matricula = "4919BVS" };

            // Act
            var resultado = citasVehiculos1.Equals(null);

            // Assert
            resultado.Should().BeFalse();
        }
    }
}