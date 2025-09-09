using ConsoleApp1.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Infrastructure
{
    // Implementación en memoria
    public class RepositorioEnMemoria : IRepositorioCiudadanos
    {
        private readonly Dictionary<string, Ciudadano> _ciudadanos = new();

        public void Agregar(Ciudadano ciudadano)
        {
            if (ciudadano == null) throw new ArgumentNullException(nameof(ciudadano));
            _ciudadanos[ciudadano.Id] = ciudadano;
        }

        public Ciudadano? ObtenerPorId(string id) =>
            _ciudadanos.TryGetValue(id, out var ciudadano) ? ciudadano : null;

        public IEnumerable<Ciudadano> Listar() => _ciudadanos.Values;

        public int Contar() => _ciudadanos.Count;
    }
}
