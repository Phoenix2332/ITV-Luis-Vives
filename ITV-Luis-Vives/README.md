# Sistema de Gestión de Citas ITV "Luis Vives"
## 1. Introducción
Se propone el desarrollo de una aplicación de escritorio profesional destinada a la gestión integral de inspecciones técnicas de vehículos (ITV).

La aplicación deberá ser implementada utilizando WPF sobre .NET 10 con C# 14, garantizando una arquitectura robusta, escalable y mantenible, basada en principios de Clean Architecture y SOLID.

---

## 2. Objetivo del Proyecto
El objetivo es diseñar e implementar un sistema que permita registrar, gestionar, consultar y administrar citas de inspección técnica de vehículos, cumpliendo con las restricciones legales y de negocio establecidas.

---

## 3. Requisitos Funcionales
### 3.1 Gestión de Citas y Vehículos
Cada cita deberá incluir obligatoriamente la siguiente información:

- Matrícula: Debe cumplir el formato español actual (4 números y 3 letras, ej.: 1234ABC).
    - https://es.wikipedia.org/wiki/Matr%C3%ADculas_automovil%C3%ADsticas_de_Espa%C3%B1a
- DNI del propietario: Validación real conforme al algoritmo oficial.
    - https://www.interior.gob.es/opencms/en/servicios-al-ciudadano/tramites-y-gestiones/dni/calculo-del-digito-de-control-del-nif-nie/
- Datos técnicos del vehículo:
    - Marca
    - Modelo
    - Tipo de motor (Gasolina, Diésel, Eléctrico o Híbrido)
    - Fecha de matriculación: No puede ser posterior a la fecha actual.
    - Fecha de inspección:
        - No puede ser anterior a la fecha actual.
        - No puede superar los 30 días naturales desde la fecha actual.

### 3.2 Reglas de Negocio
Unicidad de cita:
- Un mismo vehículo no puede tener más de una inspección programada en el mismo día.

Cupo por propietario:
- Un mismo DNI no puede tener más de tres vehículos registrados para inspección en una misma fecha.

### 3.3 Sistema de Búsqueda
El sistema deberá permitir:
- Filtrado por cualquier campo.
- Búsquedas combinadas (multi-parámetro).
- Soporte para rangos de fechas.
- Paginación obligatoria (ej.: 10 resultados por página).

Ejemplos de uso:
- Buscar vehículos de marca Toyota, motor gasolina, con fecha anterior a una determinada.
- Buscar citas asociadas a un DNI en un rango de fechas concreto.

---

## 4. Requisitos Técnicos
### 4.1 Persistencia de Datos
La aplicación deberá permitir cambiar el motor de persistencia mediante configuración (JSON), sin recompilar:
- ADO.NET
- Dapper
- Entity Framework Core

### 4.2 Gestión de Datos y Archivos
Importación y exportación en:
- JSON
- XML
- CSV

Generación de documentos:
- PDF
- HTML

Sistema de logging:
- Consola y fichero
- Eliminación o rotación automática de logs con antigüedad superior a 5 días

Sistema de borrado configurable:
- Borrado físico (DELETE)
- Borrado lógico (flag)

### 4.3 Interfaz de Usuario
La interfaz, desarrollada en WPF, deberá incluir:
- Menú principal de navegación
- DataGrid con ordenación por columnas
- Validación visual de datos en formularios
- Sección "Acerca de" con:
    - Datos del autor
    - Enlace al repositorio

---

## 5. Calidad del Software
### 5.1 Control de Versiones
Uso obligatorio de Git con GitFlow

Historial de commits evaluable

### 5.2 Documentación (UML)
El proyecto deberá incluir:
- Descripción del problema
- Requisitos funcionales y no funcionales
- Requisitos de información
- Diagramas:
    - Casos de uso
    - Arquitectura
    - Base de datos
    - Secuencia (operaciones CRUD)

### 5.3 Testing
Tests unitarios obligatorios para:
- Lógica de negocio
- Acceso a datos
- Exclusión de la capa de interfaz (UI)

### 5.4 Análisis Económico
Estimación de costes de desarrollo

Posibles escenarios de ampliación

---

## 6. Entregables
Aplicación completamente funcional

Código documentado

Vídeo demostrativo (YouTube)