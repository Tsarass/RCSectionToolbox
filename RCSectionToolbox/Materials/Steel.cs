using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCSectionToolbox.Materials
{
    public abstract class Steel
    {
        protected double _Es = 200000000;
        protected double _fy;
        protected double _esyd;
        protected double _esud;
        protected double _fyd;

        /// <summary>Young modulus of elasticity in kPa.</summary>
        public double Es { get { return _Es; } }

        /// <summary>Steel yield strength in kPa.</summary>
        public double fy { get { return _fy; } }

        /// <summary>Ultimate design steel strain.</summary>
        public double esud { get { return _esud; } }

        /// <summary>Steel yield strain.</summary>
        public double esyd { get { return _esyd; } }

        /// <summary>Steel design yield strength in kPa.</summary>
        public double fyd { get { return _fyd; } }

        public Steel(double fy)
        {
            _fy = fy;
            _fyd = fy;
            _esyd = _fyd / Es;
        }

        public abstract double getStress(double strain);
    }

    public class EurocodeSteel : Steel
    {
        private const double safetyFactor = 1.15;

        /// <summary>
        /// Creates an instance of Eurocode steel.
        /// </summary>
        /// <param name="fy">Yield strength in kPa.</param>
        public EurocodeSteel(double fy) : base(fy)
        {
            _fyd = fy / safetyFactor;
            _esud = 75d * 0.9 / 1000;
        }

        /// <summary>
        /// Get the stress of steel for a given strain in kPa.
        /// </summary>
        /// <param name="strain"></param>
        /// <returns></returns>
        public override double getStress(double strain)
        {
            double stress = strain * Es;
            return MathEx.Clamp(stress, -_fyd, _fyd);
        }
    }
}
