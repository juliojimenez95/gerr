# 🌱 Sistema de Gestión de Residuos y Reciclaje

**ODS 11: Ciudades y Comunidades Sostenibles**  
Aplicación de consola en C# .NET 8 que implementa los 4 pilares de POO

---

## 📋 Descripción del Proyecto

Sistema que incentiva el reciclaje ciudadano mediante un esquema de puntos, genera datos para la gestión municipal de residuos, y contribuye al Objetivo de Desarrollo Sostenible 11: Ciudades y Comunidades Sostenibles.

### 🎯 Funcionalidades Principales
- ✅ Registro de ciudadanos por zona geográfica
- ♻️ Gestión de depósitos de residuos (Plástico, Papel, Vidrio)
- 🎯 Sistema de puntos con estrategias intercambiables
- 📊 Reportes por zona para optimización municipal
- 📈 Estadísticas de impacto ambiental

---

## 🛠️ Requisitos del Sistema

- **.NET 8 SDK** o superior
- **Sistema Operativo:** Windows, Linux, macOS
- **IDE recomendado:** Visual Studio 2022, Visual Studio Code, o Rider

---

## 🚀 Instalación y Ejecución

### Opción 1: Clonar repositorio
```bash
git clone https://github.com/usuario/gestion-residuos.git
cd gestion-residuos/src/GestionResiduos.ConsoleApp
dotnet run
```

### Opción 2: Crear proyecto desde cero
```bash
# Crear proyecto de consola
dotnet new console -n GestionResiduos
cd GestionResiduos

# Copiar el código del archivo Program.cs proporcionado
# Ejecutar aplicación
dotnet run
```

### Opción 3: Visual Studio
1. Abrir Visual Studio 2022
2. Crear nuevo proyecto → Console App (.NET 8)
3. Copiar el código fuente
4. Ejecutar con F5 o Ctrl+F5

---

## 📖 Manual de Usuario

### Menú Principal
```
🌱 Estrategia actual: Estrategia Estándar

=== MENÚ PRINCIPAL ===
1) Crear ciudadano
2) Registrar depósito de residuo
3) Consultar ciudadano
4) Listar todos los ciudadanos
5) Generar reporte por zona
6) Cambiar estrategia de puntos
7) Mostrar estadísticas generales
0) Salir
```

### 👤 1. Crear Ciudadano
Registra un nuevo ciudadano en el sistema.
- **ID/Cédula:** Identificación única
- **Nombre:** Nombre completo del ciudadano
- **Zona:** Norte, Sur, Este, Oeste, Centro

**Ejemplo:**
```
ID/Cédula: 12345678
Nombre completo: María González
Zona: Norte
✅ Ciudadano María González creado exitosamente
```

### ♻️ 2. Registrar Depósito
Permite registrar residuos depositados por un ciudadano.

**Tipos de residuo y puntuación:**
- **Plástico (P):** 2 puntos por kg
- **Papel (A):** 1.5 puntos por kg  
- **Vidrio (V):** 3 puntos por kg

**Ejemplo:**
```
ID del ciudadano: 12345678
Seleccione el tipo: P
Peso en kilogramos: 2.5
✅ Depósito registrado: 2.5 kg de Plástico
🎯 Puntos obtenidos: 5.00
```

### 🔍 3. Consultar Ciudadano
Muestra información detallada de un ciudadano específico.

**Información mostrada:**
- Datos personales y zona
- Puntos totales acumulados
- Estadísticas por tipo de residuo
- Últimos 5 depósitos realizados

### 📊 5. Reporte por Zona
Genera estadísticas de reciclaje por zona geográfica.

**Ejemplo de salida:**
```
🌍 REPORTE ZONA: NORTE
👥 Ciudadanos participantes: 3
🎯 Puntos totales zona: 25.50

=== RESIDUOS RECOLECTADOS ===
Plástico: 8.50 kg (60.7%)
Papel: 3.20 kg (22.9%)
Vidrio: 2.30 kg (16.4%)

📦 Total recolectado: 14.00 kg
```

### ⚙️ 6. Cambiar Estrategia de Puntos
Permite modificar el sistema de cálculo de puntos:
- **Estándar:** Puntos normales según tipo de residuo
- **Promocional:** Multiplica puntos por 1.5 (campañas especiales)

---

## 🏗️ Arquitectura del Sistema

### Estructura de Carpetas
```
/src/
  /GestionResiduos.ConsoleApp/     # Aplicación principal
  /GestionResiduos.Domain/         # Entidades y contratos
  /GestionResiduos.Application/    # Servicios y lógica
  /GestionResiduos.Infrastructure/ # Persistencia
/docs/
  /analisis.pdf                    # Documento de análisis
  /capturas/                       # Screenshots del sistema
/tests/                            # Pruebas unitarias (bonus)
```

### 🧩 Aplicación de los 4 Pilares de POO

#### 1. 🎭 Abstracción
- **Clase abstracta `Residuo`:** Define estructura base para todos los residuos
- **Interface `IEstrategiaCalculoPuntos`:** Contrato para algoritmos de cálculo
- **Interface `IRepositorioCiudadanos`:** Abstrae la persistencia de datos

```csharp
public abstract class Residuo
{
    public abstract double CalcularPuntos();
    public abstract string TipoResiduo { get; }
}

public interface IEstrategiaCalculoPuntos
{
    double Calcular(Residuo residuo);
}
```

#### 2. 🔒 Encapsulamiento
- **Lista privada de depósitos:** `Ciudadano._depositos`
- **Acceso controlado:** Métodos públicos validan antes de modificar estado
- **Propiedades protegidas:** Setters privados para mantener integridad

```csharp
public class Ciudadano
{
    private readonly List<Deposito> _depositos = new();
    public IReadOnlyCollection<Deposito> Depositos => _depositos.AsReadOnly();
    
    public void RealizarDeposito(Residuo residuo, double puntos)
    {
        // Validaciones antes de modificar estado
        _depositos.Add(new Deposito(residuo, puntos));
    }
}
```

#### 3. 🧬 Herencia
- **Jerarquía de residuos:** `Residuo` ← `Plastico`, `Papel`, `Vidrio`
- **Especialización:** Cada tipo implementa su cálculo específico

```csharp
public sealed class Plastico : Residuo
{
    public const double PUNTOS_POR_KG = 2.0;
    public override double CalcularPuntos() => Peso * PUNTOS_POR_KG;
}
```

#### 4. 🎪 Polimorfismo
- **Estrategias intercambiables:** Sistema puede usar diferentes algoritmos de cálculo
- **Comportamiento dinámico:** `residuo.CalcularPuntos()` actúa según el tipo real

```csharp
// El sistema calcula puntos sin conocer el tipo específico
double puntos = _estrategiaActual.Calcular(residuo);
```

---

## 🧪 Casos de Prueba

### Casos Manuales Básicos

| Caso | Acción | Entrada | Resultado Esperado |
|------|---------|---------|-------------------|
| CP-01 | Crear ciudadano | ID: 12345, Nombre: Juan, Zona: Norte | ✅ Ciudadano creado |
| CP-02 | Ciudadano duplicado | ID: 12345 (existente) | ❌ Error: "Ya existe" |
| CP-03 | Depósito plástico | 2.5 kg plástico | ✅ 5 puntos (2.5 × 2) |
| CP-04 | Peso negativo | -1 kg papel | ❌ Error: "Peso debe ser positivo" |
| CP-05 | Cambio estrategia | Promocional | ✅ Puntos × 1.5 en nuevos depósitos |

### Escenarios de Demostración

#### 🎬 Escenario 1: Flujo Completo Básico
1. Crear ciudadano: `12345 - María González - Norte`
2. Depositar: `2 kg de plástico → 4 puntos`
3. Depositar: `1.5 kg de papel → 2.25 puntos`
4. Consultar ciudadano → Mostrar 6.25 puntos totales
5. Generar reporte zona Norte

#### 🎬 Escenario 2: Estrategias de Puntos
1. Estrategia estándar: `1 kg vidrio → 3 puntos`
2. Cambiar a promocional: `1 kg vidrio → 4.5 puntos`
3. Demostrar que ciudadanos anteriores mantienen sus puntos

#### 🎬 Escenario 3: Manejo de Errores
1. Intentar crear ciudadano con ID duplicado
2. Intentar depositar peso negativo
3. Consultar ciudadano inexistente
4. Verificar mensajes de error claros

---

## 📈 Ejemplos de Salida

### Consulta de Ciudadano
```
📋 INFORMACIÓN DE MARÍA GONZÁLEZ
ID: 12345678
Zona: Norte
🎯 Puntos totales: 12.75
📦 Total depósitos: 3

=== ESTADÍSTICAS POR TIPO ===
Plástico: 3.50 kg
Papel: 2.10 kg
Vidrio: 1.80 kg

=== ÚLTIMOS 5 DEPÓSITOS ===
08/09/2025 14:30 - Vidrio: 1.80 kg (5.40 pts)
08/09/2025 10:15 - Papel: 2.10 kg (3.15 pts)
07/09/2025 16:45 - Plástico: 2.20 kg (4.40 pts)
```

### Estadísticas Generales
```
=== ESTADÍSTICAS GENERALES DEL SISTEMA ===
👥 Total ciudadanos: 8
📦 Total depósitos: 23
🎯 Total puntos: 89.75
📊 Promedio depósitos/ciudadano: 2.9
🌟 Promedio puntos/ciudadano: 11.22

=== TOP 3 CIUDADANOS (POR PUNTOS) ===
1. María González (Norte) - 15.75 puntos
2. Carlos Rodríguez (Sur) - 12.50 puntos
3. Ana Martínez (Norte) - 9.25 puntos

🌍 IMPACTO AMBIENTAL: 47.30 kg de residuos reciclados ♻️
```

---

## 🔧 Decisiones de Diseño

### ¿Por qué clase abstracta para `Residuo`?
- Permite compartir código común (peso, fecha) entre tipos
- Fuerza implementación específica de `CalcularPuntos()`
- Más flexible que interface cuando hay estado compartido

### ¿Por qué interfaces para estrategias?
- Facilita el cambio de algoritmos en tiempo de ejecución
- Sigue el principio Abierto/Cerrado (SOLID)
- Permite testing fácil con mocks

### ¿Por qué repositorio en memoria?
- Simplifica la demostración sin dependencias externas
- Facilita el testing y desarrollo
- Extensible a base de datos implementando la misma interfaz

---

## 🚀 Extensibilidad

### Agregar Nuevo Tipo de Residuo
```csharp
public sealed class Metal : Residuo
{
    public const double PUNTOS_POR_KG = 2.5;
    public override string TipoResiduo => "Metal";
    public override double CalcularPuntos() => Peso * PUNTOS_POR_KG;
}
```

### Nueva Estrategia de Cálculo
```csharp
public class EstrategiaFindeSemana : IEstrategiaCalculoPuntos
{
    public string Descripcion => "Bonificación Fin de Semana (x2)";
    public double Calcular(Residuo residuo) => 
        DateTime.Now.DayOfWeek >= DayOfWeek.Saturday ? 
        residuo.CalcularPuntos() * 2 : residuo.CalcularPuntos();
}
```

### Persistencia en JSON (Bonus)
```csharp
public class RepositorioJson : IRepositorioCiudadanos
{
    private const string ARCHIVO = "ciudadanos.json";
    
    public void Guardar()
    {
        var json = JsonSerializer.Serialize(_ciudadanos.Values);
        File.WriteAllText(ARCHIVO, json);
    }
}
```

---

## 🐛 Problemas Conocidos y Soluciones

### Problema: Formato decimal
- **Error:** "Peso inválido" al usar coma decimal
- **Solución:** Usar punto decimal (2.5 en lugar de 2,5)

### Problema: Zona inexistente en reporte
- **Error:** "No hay ciudadanos en la zona"
- **Solución:** Verificar ortografía exacta de la zona

---

## 👥 Contribuir al Proyecto

### Estructura para Pull Requests
1. Fork del repositorio
2. Crear branch: `feature/nueva-funcionalidad`
3. Implementar con tests
4. Documentar cambios
5. Pull request con descripción clara

### Estándares de Código
- Nombres en español para entidades de dominio
- Métodos en inglés para convenciones técnicas
- Documentación XML en métodos públicos
- Principios SOLID

---

## 🏆 Créditos

**Desarrollado como proyecto educativo para:**
- Aplicación práctica de los 4 pilares de POO
- Contribución a los Objetivos de Desarrollo Sostenible
- Demostración de arquitectura limpia en .NET

**Tecnologías utilizadas:**
- C# .NET 8
- Principios SOLID
- Patrones de diseño (Strategy, Repository)

---

## 📞 Contacto y Soporte

Para preguntas sobre implementación, arquitectura o extensiones del proyecto:

- **Email:** juliojimenez212617@correo.itm.edu.co,juanamaya305782@correo.itm.edu.co

*"Cada kilogramo reciclado es un paso hacia ciudades más sostenibles" - ODS 11* 🌱
