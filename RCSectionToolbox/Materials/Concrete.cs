
namespace RCSectionToolbox.Materials
{
    public abstract class Concrete
    {
        protected double _fc;
        protected double _fcd;

        protected double _ecu = 0.0035;

        /// <summary>Concrete compressive strength in kPa.</summary>
        public double fc { get { return _fc; } }

        /// <summary>Concrete design compressive strength in kPa.</summary>
        public double fcd { get { return _fcd; } }

        /// <summary>Concrete ultimate strain.</summary>
        public double ecu { get { return _ecu; } }

        public Concrete(double fc)
        {
            _fc = fc;
        }

        public abstract double getStress(double strain);
    }

    public class EurocodeConcrete : Concrete
    {
        private const double safetyFactor = 1.5;

        
        protected double ecy = 0.002;

        /// <summary>
        /// Creates an instance of Eurocode concrete.
        /// </summary>
        /// <param name="fc">Concrete compressive strength in kPa.</param>
        public EurocodeConcrete(double fc) : base(fc)
        {
            _fcd = fc / safetyFactor;
        }

        /// <summary>
        /// Creates an instance of Eurocode concrete.
        /// </summary>
        /// <param name="fc">Concrete compressive strength in MPa.</param>
        /// <param name="ecu2">Ultimate concrete strain.</param>
        public EurocodeConcrete(double fc, double ecu2) : base(fc)
        {
            _fcd = fc / safetyFactor;
            _ecu = ecu2;
        }

        /// <summary>
        /// Get the stress of concrete for a given strain in kPa.
        /// </summary>
        /// <param name="strain"></param>
        /// <returns></returns>
        public override double getStress(double strain)
        {
            if (strain <= 0)
            {
                return 0;
            }
            else if (strain < ecy)
            {
                return fcd * (1 - MathEx.PowerOf2(1 - strain / ecy));
            }
            else if (strain <= ecu)
            {
                return fcd;
            }

            return 0;
        }
    }

}
