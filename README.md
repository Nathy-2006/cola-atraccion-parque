# Cola de Atracción — Parque de Diversiones

## Descripción

Simula una línea de espera (cola FIFO) para la asignación de **30 asientos** en una atracción de un parque de diversiones, garantizando que cada persona suba en el mismo orden en que llegó a la fila.

## Estructura de datos utilizada

Se implementó una **Cola (Queue)** desde cero, mediante nodos enlazados, sin usar la clase `Queue<T>` nativa de C#, con el fin de demostrar la comprensión del funcionamiento interno de la estructura.

## Tecnologías

- Lenguaje: **C#**
- Paradigma: Programación Orientada a Objetos (POO)
- Framework: .NET

## Estructura del proyecto

```
├── Program.cs             # Código fuente completo (clases y lógica del sistema)
├── ColaAtraccion.csproj   # Archivo de configuración del proyecto
└── README.md
```

## Clases principales

| Clase | Responsabilidad |
|---|---|
| `Persona` | Representa a cada visitante (turno, nombre, cédula, asiento asignado) |
| `NodoCola<T>` | Nodo enlazado genérico, unidad básica de la cola |
| `ColaGenerica<T>` | Implementación propia del TAD Cola (Encolar, Desencolar, etc.) |
| `ColaAtraccion` | Lógica de negocio: registro de llegadas, asignación de asientos, reportería |
| `Program` | Punto de entrada; ejecuta la simulación completa |

## Cómo ejecutar

**Con Visual Studio:**
1. Abrir el archivo `.csproj` con Visual Studio.
2. Presionar `F5` o el botón ▶.

**Con .NET CLI:**
```bash
dotnet run
```

## Reportería incluida

- Listado de personas en cola de espera.
- Listado de personas ya atendidas (con asiento asignado).
- Consulta puntual de una persona por número de cédula.
- Análisis del tiempo de ejecución (medido con `Stopwatch`).

## Autor
Nathalia Teresa Barreiro Castro

## Asignatura

Estructura de Datos — UEA-L-UFB-032 — Período académico 2026-2026
