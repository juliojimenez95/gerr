using System;
using System.Globalization;
using System.Linq;
using ConsoleApp1.Application;
using ConsoleApp1.Domain;
using ConsoleApp1.Infrastructure;

namespace ConsoleApp1
{
    public class Program
    {
        private static readonly ServicioGestionResiduos _servicio;

        static Program()
        {
            var repositorio = new RepositorioEnMemoria();
            _servicio = new ServicioGestionResiduos(repositorio);
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("=== SISTEMA DE GESTIÓN DE RESIDUOS Y RECICLAJE ===");
            Console.WriteLine("ODS 11: Ciudades y Comunidades Sostenibles\n");

            // Datos de ejemplo
            InicializarDatosEjemplo();

            while (true)
            {
                MostrarMenu();
                var opcion = Console.ReadLine();

                try
                {
                    switch (opcion)
                    {
                        case "1": CrearCiudadano(); break;
                        case "2": RegistrarDeposito(); break;
                        case "3": ConsultarCiudadano(); break;
                        case "4": ListarCiudadanos(); break;
                        case "5": GenerarReporteZona(); break;
                        case "6": CambiarEstrategia(); break;
                        case "7": MostrarEstadisticas(); break;
                        case "0":
                            Console.WriteLine("¡Gracias por contribuir al reciclaje!");
                            return;
                        default:
                            Console.WriteLine("Opción inválida. Intente nuevamente.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error: {ex.Message}");
                }

                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void MostrarMenu()
        {
            Console.WriteLine($"\n🌱 Estrategia actual: {_servicio.EstrategiaActual.Descripcion}");
            Console.WriteLine("\n=== MENÚ PRINCIPAL ===");
            Console.WriteLine("1) Crear ciudadano");
            Console.WriteLine("2) Registrar depósito de residuo");
            Console.WriteLine("3) Consultar ciudadano");
            Console.WriteLine("4) Listar todos los ciudadanos");
            Console.WriteLine("5) Generar reporte por zona");
            Console.WriteLine("6) Cambiar estrategia de puntos");
            Console.WriteLine("7) Mostrar estadísticas generales");
            Console.WriteLine("0) Salir");
            Console.Write("\nSeleccione una opción: ");
        }

        static void CrearCiudadano()
        {
            Console.WriteLine("\n=== CREAR NUEVO CIUDADANO ===");

            Console.Write("ID/Cédula: ");
            var id = Console.ReadLine()!;

            Console.Write("Nombre completo: ");
            var nombre = Console.ReadLine()!;

            Console.Write("Zona (Norte/Sur/Este/Oeste/Centro): ");
            var zona = Console.ReadLine()!;

            _servicio.CrearCiudadano(id, nombre, zona);

            Console.WriteLine($"✅ Ciudadano {nombre} creado exitosamente en zona {zona}");
        }

        static void RegistrarDeposito()
        {
            Console.WriteLine("\n=== REGISTRAR DEPÓSITO DE RESIDUO ===");

            Console.Write("ID del ciudadano: ");
            var ciudadanoId = Console.ReadLine()!;

            Console.WriteLine("\nTipos de residuo disponibles:");
            Console.WriteLine("P) Plástico (2 puntos/kg)");
            Console.WriteLine("A) Papel (1.5 puntos/kg)");
            Console.WriteLine("V) Vidrio (3 puntos/kg)");
            Console.Write("Seleccione el tipo: ");
            var tipo = Console.ReadLine()!.ToUpperInvariant();

            Console.Write("Peso en kilogramos: ");
            if (!double.TryParse(Console.ReadLine(), NumberStyles.Float, CultureInfo.InvariantCulture, out var peso))
            {
                throw new ArgumentException("Peso inválido. Use formato decimal con punto (ej: 2.5)");
            }

            Residuo residuo = tipo switch
            {
                "P" => new Plastico(peso),
                "A" => new Papel(peso),
                "V" => new Vidrio(peso),
                _ => throw new ArgumentException("Tipo de residuo inválido")
            };

            _servicio.RegistrarDeposito(ciudadanoId, residuo);

            var puntosCalculados = _servicio.EstrategiaActual.Calcular(residuo);
            Console.WriteLine($"✅ Depósito registrado: {peso} kg de {residuo.TipoResiduo}");
            Console.WriteLine($"🎯 Puntos obtenidos: {puntosCalculados:F2}");
        }

        static void ConsultarCiudadano()
        {
            Console.WriteLine("\n=== CONSULTAR CIUDADANO ===");

            Console.Write("ID del ciudadano: ");
            var id = Console.ReadLine()!;

            var ciudadano = _servicio.ConsultarCiudadano(id);
            if (ciudadano == null)
            {
                Console.WriteLine("❌ Ciudadano no encontrado");
                return;
            }

            Console.WriteLine($"\n📋 INFORMACIÓN DE {ciudadano.Nombre.ToUpper()}");
            Console.WriteLine($"ID: {ciudadano.Id}");
            Console.WriteLine($"Zona: {ciudadano.Zona}");
            Console.WriteLine($"🎯 Puntos totales: {ciudadano.CalcularPuntosTotales():F2}");
            Console.WriteLine($"📦 Total depósitos: {ciudadano.Depositos.Count}");

            if (ciudadano.Depositos.Any())
            {
                Console.WriteLine("\n=== ESTADÍSTICAS POR TIPO ===");
                var estadisticas = ciudadano.ObtenerEstadisticasPorTipo();
                foreach (var stat in estadisticas)
                {
                    Console.WriteLine($"{stat.Key}: {stat.Value:F2} kg");
                }

                Console.WriteLine("\n=== ÚLTIMOS 5 DEPÓSITOS ===");
                var ultimosDepositos = ciudadano.Depositos
                    .OrderByDescending(d => d.Fecha)
                    .Take(5);

                foreach (var deposito in ultimosDepositos)
                {
                    Console.WriteLine($"{deposito.Fecha:dd/MM/yyyy HH:mm} - " +
                                    $"{deposito.Residuo.TipoResiduo}: {deposito.Residuo.Peso:F2} kg " +
                                    $"({deposito.PuntosObtenidos:F2} pts)");
                }
            }
        }

        static void ListarCiudadanos()
        {
            Console.WriteLine("\n=== LISTADO DE CIUDADANOS REGISTRADOS ===");

            var ciudadanos = _servicio.ListarCiudadanos().OrderBy(c => c.Zona).ThenBy(c => c.Nombre);

            if (!ciudadanos.Any())
            {
                Console.WriteLine("❌ No hay ciudadanos registrados");
                return;
            }

            Console.WriteLine($"{"ID",-10} {"Nombre",-25} {"Zona",-10} {"Depósitos",-10} {"Puntos",-10}");
            Console.WriteLine(new string('=', 70));

            foreach (var ciudadano in ciudadanos)
            {
                Console.WriteLine($"{ciudadano.Id,-10} {ciudadano.Nombre,-25} {ciudadano.Zona,-10} " +
                                $"{ciudadano.Depositos.Count,-10} {ciudadano.CalcularPuntosTotales(),-10:F2}");
            }

            var totalCiudadanos = ciudadanos.Count();
            var totalPuntos = ciudadanos.Sum(c => c.CalcularPuntosTotales());
            var totalDepositos = ciudadanos.Sum(c => c.Depositos.Count);

            Console.WriteLine(new string('=', 70));
            Console.WriteLine($"📊 TOTALES: {totalCiudadanos} ciudadanos | {totalDepositos} depósitos | {totalPuntos:F2} puntos");
        }

        static void GenerarReporteZona()
        {
            Console.WriteLine("\n=== REPORTE POR ZONA ===");

            Console.Write("Zona a consultar: ");
            var zona = Console.ReadLine()!;

            var reporte = _servicio.GenerarReportePorZona(zona);

            Console.WriteLine($"\n🌍 REPORTE ZONA: {reporte.Zona.ToUpper()}");
            Console.WriteLine($"👥 Ciudadanos participantes: {reporte.CantidadCiudadanos}");
            Console.WriteLine($"🎯 Puntos totales zona: {reporte.PuntosTotalesZona:F2}");

            Console.WriteLine("\n=== RESIDUOS RECOLECTADOS ===");
            if (reporte.TotalesPorTipo.Any())
            {
                var totalKg = reporte.TotalesPorTipo.Values.Sum();
                foreach (var tipo in reporte.TotalesPorTipo)
                {
                    var porcentaje = (tipo.Value / totalKg) * 100;
                    Console.WriteLine($"{tipo.Key}: {tipo.Value:F2} kg ({porcentaje:F1}%)");
                }
                Console.WriteLine($"\n📦 Total recolectado: {totalKg:F2} kg");
            }
            else
            {
                Console.WriteLine("❌ No hay residuos registrados en esta zona");
            }
        }

        static void CambiarEstrategia()
        {
            Console.WriteLine("\n=== CAMBIAR ESTRATEGIA DE CÁLCULO ===");
            Console.WriteLine("Estrategia actual: " + _servicio.EstrategiaActual.Descripcion);

            Console.WriteLine("\nEstrategias disponibles:");
            Console.WriteLine("1) Estándar (puntos normales)");
            Console.WriteLine("2) Promocional (puntos x1.5)");
            Console.Write("Seleccione: ");

            var opcion = Console.ReadLine();

            IEstrategiaCalculoPuntos nuevaEstrategia = opcion switch
            {
                "1" => new EstrategiaEstandar(),
                "2" => new EstrategiaPromocional(),
                _ => throw new ArgumentException("Opción inválida")
            };

            _servicio.CambiarEstrategia(nuevaEstrategia);
            Console.WriteLine($"✅ Estrategia cambiada a: {nuevaEstrategia.Descripcion}");
            Console.WriteLine("⚠️  Los nuevos depósitos usarán la nueva estrategia");
        }

        static void MostrarEstadisticas()
        {
            Console.WriteLine("\n=== ESTADÍSTICAS GENERALES DEL SISTEMA ===");

            var ciudadanos = _servicio.ListarCiudadanos().ToList();

            if (!ciudadanos.Any())
            {
                Console.WriteLine("❌ No hay datos para mostrar");
                return;
            }

            // Estadísticas generales
            var totalCiudadanos = ciudadanos.Count;
            var totalDepositos = ciudadanos.Sum(c => c.Depositos.Count);
            var totalPuntos = ciudadanos.Sum(c => c.CalcularPuntosTotales());

            Console.WriteLine($"👥 Total ciudadanos: {totalCiudadanos}");
            Console.WriteLine($"📦 Total depósitos: {totalDepositos}");
            Console.WriteLine($"🎯 Total puntos: {totalPuntos:F2}");

            if (totalDepositos > 0)
            {
                Console.WriteLine($"📊 Promedio depósitos/ciudadano: {(double)totalDepositos / totalCiudadanos:F1}");
                Console.WriteLine($"🌟 Promedio puntos/ciudadano: {totalPuntos / totalCiudadanos:F2}");
            }

            // Por zona
            Console.WriteLine("\n=== DISTRIBUCIÓN POR ZONA ===");
            var porZona = ciudadanos.GroupBy(c => c.Zona);
            foreach (var grupo in porZona.OrderBy(g => g.Key))
            {
                var puntosZona = grupo.Sum(c => c.CalcularPuntosTotales());
                var depositosZona = grupo.Sum(c => c.Depositos.Count);
                Console.WriteLine($"{grupo.Key}: {grupo.Count()} ciudadanos, " +
                                $"{depositosZona} depósitos, {puntosZona:F2} puntos");
            }

            // Top 3 ciudadanos
            Console.WriteLine("\n=== TOP 3 CIUDADANOS (POR PUNTOS) ===");
            var topCiudadanos = ciudadanos
                .OrderByDescending(c => c.CalcularPuntosTotales())
                .Take(3);

            int posicion = 1;
            foreach (var ciudadano in topCiudadanos)
            {
                Console.WriteLine($"{posicion}. {ciudadano.Nombre} ({ciudadano.Zona}) - " +
                                $"{ciudadano.CalcularPuntosTotales():F2} puntos");
                posicion++;
            }

            // Residuos por tipo
            Console.WriteLine("\n=== RESIDUOS POR TIPO (SISTEMA COMPLETO) ===");
            var residuosPorTipo = new Dictionary<string, double>();

            foreach (var ciudadano in ciudadanos)
            {
                foreach (var deposito in ciudadano.Depositos)
                {
                    var tipo = deposito.Residuo.TipoResiduo;
                    if (!residuosPorTipo.ContainsKey(tipo))
                        residuosPorTipo[tipo] = 0;

                    residuosPorTipo[tipo] += deposito.Residuo.Peso;
                }
            }

            if (residuosPorTipo.Any())
            {
                var totalKg = residuosPorTipo.Values.Sum();
                foreach (var tipo in residuosPorTipo.OrderByDescending(x => x.Value))
                {
                    var porcentaje = (tipo.Value / totalKg) * 100;
                    Console.WriteLine($"{tipo.Key}: {tipo.Value:F2} kg ({porcentaje:F1}%)");
                }
                Console.WriteLine($"\n🌍 IMPACTO AMBIENTAL: {totalKg:F2} kg de residuos reciclados ♻️");
            }
        }

        static void InicializarDatosEjemplo()
        {
            try
            {
                // Crear algunos ciudadanos de ejemplo
                _servicio.CrearCiudadano("12345678", "María González", "Norte");
                _servicio.CrearCiudadano("87654321", "Carlos Rodríguez", "Sur");
                _servicio.CrearCiudadano("11223344", "Ana Martínez", "Norte");
                _servicio.CrearCiudadano("44332211", "Pedro López", "Centro");

                // Algunos depósitos de ejemplo
                _servicio.RegistrarDeposito("12345678", new Plastico(2.5));
                _servicio.RegistrarDeposito("12345678", new Papel(1.8));
                _servicio.RegistrarDeposito("87654321", new Vidrio(3.2));
                _servicio.RegistrarDeposito("11223344", new Plastico(1.5));
                _servicio.RegistrarDeposito("44332211", new Papel(2.1));

                Console.WriteLine("✅ Datos de ejemplo inicializados correctamente");
                Console.WriteLine("💡 Tip: Ya hay ciudadanos registrados para que puedas probar el sistema");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error al inicializar datos de ejemplo: {ex.Message}");
            }
        }
    }
}

/*
 * DEMOSTRACIÓN DE LOS 4 PILARES DE POO:
 * 
 * 1. ABSTRACCIÓN:
 *    - Clase abstracta Residuo define la estructura base
 *    - Interfaces IEstrategiaCalculoPuntos e IRepositorioCiudadanos
 *    
 * 2. ENCAPSULAMIENTO:
 *    - Ciudadano._depositos es privada, acceso via RealizarDeposito()
 *    - Validaciones en constructores protegen integridad de datos
 *    
 * 3. HERENCIA:
 *    - Plastico, Papel, Vidrio heredan de Residuo
 *    - Cada clase implementa su propio CalcularPuntos()
 *    
 * 4. POLIMORFISMO:
 *    - IEstrategiaCalculoPuntos permite cambiar algoritmos en tiempo real
 *    - residuo.CalcularPuntos() se comporta diferente según el tipo real
 */