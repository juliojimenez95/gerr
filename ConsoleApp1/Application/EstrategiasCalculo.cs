using ConsoleApp1.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Application
{
    // Implementaciones de estrategias (Polimorfismo)
    public class EstrategiaEstandar : IEstrategiaCalculoPuntos
    {
        public string Descripcion => "Estrategia Estándar";
        public double Calcular(Residuo residuo) => residuo.CalcularPuntos();
    }

    public class EstrategiaPromocional : IEstrategiaCalculoPuntos
    {
        public string Descripcion => "Estrategia Promocional (x1.5)";
        public double Calcular(Residuo residuo) => residuo.CalcularPuntos() * 1.5;
    }
}
