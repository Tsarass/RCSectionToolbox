using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCSectionToolbox.Materials;
using RCSectionToolbox.RCSectionAnalysis;
using static RCSectionToolbox.RCSectionAnalysis.SectionAnalysis;

namespace RCSectionToolbox
{
    public class RCSection
    {
        Concrete _concrete;
        Steel _steel;

        private double _h;
        private double _b;
        private double _N;
        private double _c;
        private double _As1;
        private double _As2;
        private double _ys1;
        private double _ys2;

        public double height { get { return _h; } }
        public double width { get { return _b; } }
        public double axialForce { get { return _N; } set { _N = value; } }
        public double cover { get { return _c; } }
        public double Asbot { get { return _As1; } }
        public double Astop { get { return _As2; } }
        public double yAsbot { get { return _ys1; } }
        public double yAstop { get { return _ys2; } }
        public Concrete concrete { get { return _concrete; } }
        public Steel steel { get { return _steel; } }

        public RCSection(Concrete concrete, Steel steel, double h, double b, double N, double c, double as1, double as2)
        {
            _concrete = concrete;
            _steel = steel;
            _h = h;
            _b = b;
            _N = N;
            _c = c;
            _As1 = as1;
            _As2 = as2;
            _ys1 = h - c;
            _ys2 = c;
        }

        /// <summary>
        /// Runs the analysis and tries to find internal equilibrium for the section for the given curvature.
        /// </summary>
        /// <param name="curvature">Curvature for which to find equilibrium of internal forces.</param>
        /// <param name="initialConcreteStrain">An initial concrete strain. If no value given, the iterations will start from 0.</param>
        /// <returns>A results object which contains all the material information or a failure mode.</returns>
        public AnalysisResults getCapacityForGivenCurvature(double curvature, double initialConcreteStrain = 0)
        {
            SectionAnalysis analysis = new SectionAnalysis(this);
            var analysisResults = analysis.getCapacityForConstantCurvature(curvature, initialConcreteStrain);
            return analysisResults;
        }

        /// <summary>
        /// Calculates the ultimate capacity of a section.
        /// </summary>
        /// <param name="curvatureLimit">The maximum curvature before search stops.</param>
        /// <returns></returns>
        public AnalysisResults getUltimateCapacity(double curvatureLimit)
        {
            SectionAnalysis analysis = new SectionAnalysis(this);
            var analysisResults = analysis.getUltimateCapacity(curvatureLimit);
            return analysisResults;
        }

        /// <summary>
        /// Checks the maximum axial force capacity against the applied axial force.
        /// </summary>
        /// <exception cref="CompressionCapacityExceeded"></exception>
        /// <exception cref="TensionCapacityExceeded"></exception>
        public void checkIfUltimateAxialCapacityExceeded()
        {
            SectionAnalysis analysis = new SectionAnalysis(this);
            analysis.checkAxialForceCapacityAndThrowExceptionIfExceeded();
        }

        public class AnalysisNotRun : System.Exception
        {
            public AnalysisNotRun() : base("Analysis was not run yet.") { }
        }

    }
}
