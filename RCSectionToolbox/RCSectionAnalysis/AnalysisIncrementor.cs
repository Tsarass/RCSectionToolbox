using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCSectionToolbox.RCSectionAnalysis
{
    public partial class SectionAnalysis
    {
        /// <summary>
        /// Represents an analysis incrementor object that has a constant and an incremental value.
        /// </summary>
        private abstract class AnalysisIncrementor
        {
            protected double? _maxIncrementalValue;
            protected double _incrementStep;
            protected double _phi;
            protected double _concreteStrain;

            public double curvature { get { return _phi; } }
            public double concreteStrain { get { return _concreteStrain; } }

            public AnalysisIncrementor(double maxIncrementalValue = double.PositiveInfinity)
            {
                if (double.IsInfinity(maxIncrementalValue))
                    _maxIncrementalValue = null;
                else
                    _maxIncrementalValue = maxIncrementalValue;
            }

            public abstract void incrementQuantity();
            public abstract void revertOneStepAndHalveIncrement();

            public void changeSignOfIncrement()
            {
                scaleIncrement(-1);
            }

            public void scaleIncrement(double scaleFactor)
            {
                _incrementStep *= scaleFactor;
            }

            protected void throwExceptionWhenMaxIncrementalValue(double incrementalValue)
            {
                if (_maxIncrementalValue.HasValue && (incrementalValue > _maxIncrementalValue))
                {
                    throw new IncrementalValueExceededMaximum();
                }
            }
        }

        /// <summary>
        /// Increment the concrete strain for a constant value of curvature.
        /// </summary>
        private class ConcreteStrainIncrementor : AnalysisIncrementor
        {
            public ConcreteStrainIncrementor(double curvature, double initialConcreteStrain, double maxConcreteStrain = double.PositiveInfinity) :
                base(maxConcreteStrain)
            {
                this._phi = curvature;
                this._concreteStrain = initialConcreteStrain;
                this._incrementStep = MIN_ALLOWED_CONC_STRAIN;
            }

            public override void incrementQuantity()
            {
                var newConcreteStrain = _concreteStrain + _incrementStep;
                throwExceptionWhenMaxIncrementalValue(newConcreteStrain);

                _concreteStrain = newConcreteStrain;
            }

            public override void revertOneStepAndHalveIncrement()
            {
                _concreteStrain -= _incrementStep;
                _incrementStep /= 2;
            }
        }

        /// <summary>
        /// Increment the curvature for a constant value of concrete strain.
        /// </summary>
        private class CurvatureIncrementor : AnalysisIncrementor
        {
            private const double phiStep = 0.0005;

            public CurvatureIncrementor(double concreteStrain, double maxCurvature = double.PositiveInfinity) :
                base(maxCurvature)
            {
                this._concreteStrain = concreteStrain;
                this._phi = phiStep;
                this._incrementStep = phiStep;
            }

            public override void incrementQuantity()
            {
                var newPhi = _phi + _incrementStep;
                throwExceptionWhenMaxIncrementalValue(newPhi);

                _phi = newPhi;
            }

            public override void revertOneStepAndHalveIncrement()
            {
                _phi -= _incrementStep;
                _incrementStep /= 2;
            }
        }

        private class IncrementalValueExceededMaximum : System.Exception
        {

        }
    }
}
