using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Domain
{
    // Polimorfismo: Estrategias de cálculo de puntos
    public interface IEstrategiaCalculoPuntos
    {
        double Calcular(Residuo residuo);
        string Descripcion { get; }
    }

    // Abstracción: Contrato de persistencia
    public interface IRepositorioCiudadanos
    {
        void Agregar(Ciudadano ciudadano);
        Ciudadano? ObtenerPorId(string id);
        IEnumerable<Ciudadano> Listar();
        int Contar();
    }
}
