using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Domain
{
    public class Ciudadano
    {
        public string Id { get; }
        public string Nombre { get; private set; }
        public string Zona { get; private set; }

        // Encapsulamiento: Lista privada, acceso controlado
        private readonly List<Deposito> _depositos = new();
        public IReadOnlyCollection<Deposito> Depositos => _depositos.AsReadOnly();

        public Ciudadano(string id, string nombre, string zona)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("ID es requerido", nameof(id));
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("Nombre es requerido", nameof(nombre));
            if (string.IsNullOrWhiteSpace(zona))
                throw new ArgumentException("Zona es requerida", nameof(zona));

            Id = id;
            Nombre = nombre;
            Zona = zona;
        }

        // Encapsulamiento: Control de reglas al modificar estado
        public void RealizarDeposito(Residuo residuo, double puntosCalculados)
        {
            if (residuo == null)
                throw new ArgumentNullException(nameof(residuo));
            if (puntosCalculados < 0)
                throw new ArgumentException("Los puntos no pueden ser negativos");

            _depositos.Add(new Deposito(residuo, puntosCalculados));
        }

        public double CalcularPuntosTotales() => _depositos.Sum(d => d.PuntosObtenidos);

        public Dictionary<string, double> ObtenerEstadisticasPorTipo()
        {
            return _depositos
                .GroupBy(d => d.Residuo.TipoResiduo)
                .ToDictionary(g => g.Key, g => g.Sum(d => d.Residuo.Peso));
        }
    }
}
