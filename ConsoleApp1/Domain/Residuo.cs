using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Domain
{
    public abstract class Residuo
    {
        public double Peso { get; }
        public DateTime FechaDeposito { get; }

        protected Residuo(double peso)
        {
            if (peso <= 0)
                throw new ArgumentException("El peso debe ser mayor a cero", nameof(peso));

            Peso = peso;
            FechaDeposito = DateTime.Now;
        }

        // Método abstracto - cada tipo de residuo calcula puntos diferente
        public abstract double CalcularPuntos();
        public abstract string TipoResiduo { get; }
    }

    // Herencia: Clases específicas de residuo
    public sealed class Plastico : Residuo
    {
        public const double PUNTOS_POR_KG = 2.0;
        public override string TipoResiduo => "Plástico";

        public Plastico(double peso) : base(peso) { }

        public override double CalcularPuntos() => Peso * PUNTOS_POR_KG;
    }

    public sealed class Papel : Residuo
    {
        public const double PUNTOS_POR_KG = 1.5;
        public override string TipoResiduo => "Papel";

        public Papel(double peso) : base(peso) { }

        public override double CalcularPuntos() => Peso * PUNTOS_POR_KG;
    }

    public sealed class Vidrio : Residuo
    {
        public const double PUNTOS_POR_KG = 3.0;
        public override string TipoResiduo => "Vidrio";

        public Vidrio(double peso) : base(peso) { }

        public override double CalcularPuntos() => Peso * PUNTOS_POR_KG;
    }

}
