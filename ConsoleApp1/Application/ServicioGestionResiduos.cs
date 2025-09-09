using ConsoleApp1.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Application
{
    public class ServicioGestionResiduos
    {
        private readonly IRepositorioCiudadanos _repositorio;
        private IEstrategiaCalculoPuntos _estrategiaActual;

        public ServicioGestionResiduos(IRepositorioCiudadanos repositorio)
        {
            _repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            _estrategiaActual = new EstrategiaEstandar(); // Estrategia por defecto
        }

        public IEstrategiaCalculoPuntos EstrategiaActual => _estrategiaActual;

        public void CambiarEstrategia(IEstrategiaCalculoPuntos nuevaEstrategia)
        {
            _estrategiaActual = nuevaEstrategia ?? throw new ArgumentNullException(nameof(nuevaEstrategia));
        }

        public void CrearCiudadano(string id, string nombre, string zona)
        {
            var ciudadanoExistente = _repositorio.ObtenerPorId(id);
            if (ciudadanoExistente != null)
                throw new InvalidOperationException($"Ya existe un ciudadano con ID: {id}");

            var ciudadano = new Ciudadano(id, nombre, zona);
            _repositorio.Agregar(ciudadano);
        }

        public void RegistrarDeposito(string ciudadanoId, Residuo residuo)
        {
            var ciudadano = _repositorio.ObtenerPorId(ciudadanoId);
            if (ciudadano == null)
                throw new InvalidOperationException($"Ciudadano con ID {ciudadanoId} no encontrado");

            // Polimorfismo: La estrategia actual calcula los puntos
            double puntos = _estrategiaActual.Calcular(residuo);
            ciudadano.RealizarDeposito(residuo, puntos);
        }

        public Ciudadano? ConsultarCiudadano(string id) => _repositorio.ObtenerPorId(id);

        public IEnumerable<Ciudadano> ListarCiudadanos() => _repositorio.Listar();

        // Reporte por zona
        public ReporteZona GenerarReportePorZona(string zona)
        {
            var ciudadanosZona = _repositorio.Listar()
                .Where(c => c.Zona.Equals(zona, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!ciudadanosZona.Any())
                throw new InvalidOperationException($"No hay ciudadanos registrados en la zona: {zona}");

            return new ReporteZona(zona, ciudadanosZona);
        }
    }

    // Clase para reportes
    public class ReporteZona
    {
        public string Zona { get; }
        public int CantidadCiudadanos { get; }
        public Dictionary<string, double> TotalesPorTipo { get; }
        public double PuntosTotalesZona { get; }

        public ReporteZona(string zona, List<Ciudadano> ciudadanos)
        {
            Zona = zona;
            CantidadCiudadanos = ciudadanos.Count;

            TotalesPorTipo = new Dictionary<string, double>();
            PuntosTotalesZona = 0;

            foreach (var ciudadano in ciudadanos)
            {
                PuntosTotalesZona += ciudadano.CalcularPuntosTotales();

                foreach (var deposito in ciudadano.Depositos)
                {
                    var tipo = deposito.Residuo.TipoResiduo;
                    if (!TotalesPorTipo.ContainsKey(tipo))
                        TotalesPorTipo[tipo] = 0;

                    TotalesPorTipo[tipo] += deposito.Residuo.Peso;
                }
            }
        }
    }
}
