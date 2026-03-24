# Simulador de la Liga BetPlay — Documentación del proyecto

## Descripción del Proyecto

Aplicación de **consola en C#** que simula de forma básica la gestión de la **Liga BetPlay**: permite registrar equipos en memoria, simular partidos (marcador manual o aleatorio), actualizar automáticamente las estadísticas de cada club y consultar la tabla de posiciones y diversas métricas del torneo mediante **Programación Orientada a Objetos (POO)**, **colecciones en memoria** y **consultas LINQ**.

El sistema evita la administración manual repetitiva de puntos, goles y clasificación, y sirve como ejercicio práctico para reforzar conceptos de C#, modelado de datos y consultas sobre objetos.

**Queda fuera de alcance** (según el documento de especificación): bases de datos, APIs externas, interfaz gráfica, persistencia en archivos, goleadores individuales y calendario completo de jornadas.

---

## Características Destacadas

- **Registro y listado de equipos** en una lista en memoria, sin duplicados por nombre (ignorando mayúsculas/minúsculas).
- **Simulación de partidos** entre dos equipos distintos, con goles ingresados por el usuario o generados al azar.
- **Actualización automática** de PJ, PG, PE, PP, GF, GC y TP para ambos equipos tras cada partido.
- **Regla de puntos**: victoria 3, empate 1, derrota 0.
- **Tabla de posiciones** ordenada por puntos, diferencia de gol, goles a favor y nombre.
- **Submenú de consultas con LINQ**: líder, máximos/mínimos en estadísticas, invictos, top 3, búsqueda por nombre, promedios, totales, proyección personalizada, ranking agrupado por puntos, entre otras.
- **Interfaz por menús** con limpieza de consola entre pantallas para facilitar la lectura.

---

## Objetivo

**Objetivo general:** desarrollar una aplicación de consola en C# que permita simular la Liga BetPlay, gestionando equipos, partidos y tabla de posiciones de manera automatizada, aplicando POO, estructuras de datos en memoria y LINQ.

**Objetivos que cubre la implementación:**

- Registrar equipos participantes en una lista en memoria.
- Almacenar y actualizar la información competitiva de cada equipo.
- Simular partidos y calcular estadísticas y puntos automáticamente.
- Generar y mostrar la clasificación con criterios de desempate definidos.
- Consultar y analizar el estado del torneo mediante LINQ.

---

## Tecnologías Utilizadas

| Área | Tecnología |
|------|------------|
| Lenguaje | C# |
| Tipo de aplicación | Consola (`OutputType` ejecutable) |
| Plataforma | .NET (`TargetFramework` según `Simulador_Liga_Bet_Play.csproj`, p. ej. `net10.0`) |
| Paradigma | Programación Orientada a Objetos |
| Datos | Listas y objetos en memoria (sin base de datos ni archivos) |
| Consultas | LINQ (`System.Linq`) |
| Entorno de desarrollo sugerido (documento guía) | Visual Studio / Visual Studio Code |

---

## Estructura del Sistema

```
Simulador_Liga_Bet_Play/
├── Program.cs                 # Punto de entrada y capa de presentación (menús, consola)
├── EQUIPO/
│   └── Equipo.cs              # Modelo: datos y lógica de estadísticas por equipo
├── TORNEOS/
│   └── Torneo.cs              # Gestor del torneo: lista de equipos, partidos, consultas LINQ
├── Simulador_Liga_Bet_Play.csproj
├── Simulador_Liga_Bet_Play.sln
└── Documentacion_Proyecto.md  # Este archivo
```

Los directorios `obj/` y `bin/` son generados por la compilación y no forman parte del diseño lógico del sistema.

**Capas conceptuales:**

1. **Modelo:** `Equipo` (estado y actualización tras un resultado).
2. **Lógica / servicio:** `Torneo` (reglas del campeonato, simulación, consultas).
3. **Presentación:** `Program` (entrada/salida por consola y navegación por menús).

---

## Qué Hace Cada Archivo

### `Program.cs`

Contiene el método `Main`, el menú principal (listar, registrar, simular, tabla, consultas, salir) y el submenú de estadísticas. Orquesta la interacción con el usuario: lectura de opciones, validaciones básicas, llamadas a `Torneo` y visualización de resultados. Incluye utilidades como limpieza de consola y pausas entre pantallas.

### `EQUIPO/Equipo.cs`

Define la entidad **Equipo** con propiedades: nombre, partidos jugados (PJ), ganados (PG), empatados (PE), perdidos (PP), goles a favor (GF), goles en contra (GC), total de puntos (TP) y diferencia de gol. El método `AplicarResultado` actualiza todas las estadísticas y los puntos según el marcador del partido visto desde ese equipo.

### `TORNEOS/Torneo.cs`

Clase **Torneo**: mantiene la colección de equipos, permite registrar y buscar por nombre, simular partidos entre dos instancias de `Equipo` y expone la tabla de posiciones con el orden establecido en el documento del proyecto. Centraliza las **consultas LINQ** (líder, rankings, filtros, promedios, sumas, agrupaciones, proyecciones y textos de resumen) que consume la interfaz de consola.

### `Simulador_Liga_Bet_Play.csproj`

Archivo de proyecto MSBuild/SDK de .NET: define el ensamblado como ejecutable de consola, la versión de framework objetivo y opciones como `ImplicitUsings` y `Nullable`.

### `Simulador_Liga_Bet_Play.sln`

Solución de Visual Studio que agrupa el proyecto para abrirlo y compilarlo desde el IDE.

### `Documentacion_Proyecto.md`

Este documento: descripción general del trabajo, características, objetivo, tecnologías, estructura y rol de cada archivo fuente relevante.

---

## Autor

**Nombre:** *Valentina Mancilla*  

