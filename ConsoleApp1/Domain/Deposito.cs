using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Domain
{
    public class Deposito
    {
        public Residuo Residuo { get; }
        public double PuntosObtenidos { get; }
        public DateTime Fecha => Residuo.FechaDeposito;

        public Deposito(Residuo residuo, double puntosObtenidos)
        {
            Residuo = residuo ?? throw new ArgumentNullException(nameof(residuo));
            PuntosObtenidos = puntosObtenidos;
        }
    }
}
