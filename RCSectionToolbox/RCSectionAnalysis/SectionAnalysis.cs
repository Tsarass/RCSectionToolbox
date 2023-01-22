using System;
using RCSectionToolbox.Materials;

namespace RCSectionToolbox.RCSectionAnalysis
{
    public partial class SectionAnalysis
    {
        private const double forceBalanceTolerance = 0.005;      //tolerance of the equilibrium search
        private const double acc = 0.85;                         //reduction factor for long term effects on concrete strength
        private const double MAX_ALLOWED_ITERATIONS = 1E5;       //maximum iterations for the equilibrium algorithm

        public const double MIN_ALLOWED_CONC_STRAIN = 0.005 / 1000;

        //Fibre variables
        private int nFibreRows = 100;
        private int nFibreColumns = 2;
        private double dA;
        private double[,] elementx, elementy;

        protected double h;
        private double b;
        private double N;
        private double fcd;
        private double fyd;
        private double As1;
        private double As2;
        protected double ys1;
        protected double ys2;
        protected Concrete concrete;
        protected Steel steel;

        public SectionAnalysis(RCSection section)
        {
            extractSectionInMemberVariables(section);
            initializeSectionFibres();
        }

        /// <summary>
        /// Extract all section properties in member variables for readability.
        /// </summary>
        /// <param name="section"></param>
        private void extractSectionInMemberVariables(RCSection section)
        {
            concrete = section.concrete;
            steel = section.steel;
            h = section.height;
            b = section.width;
            N = section.axialForce;
            fcd = concrete.fcd;
            fyd = steel.fyd;
            As1 = section.Asbot;
            As2 = section.Astop;
            ys1 = section.yAsbot;
            ys2 = section.yAstop;
        }

        private void initializeSectionFibres()
        {
            //integrator parameters
            int ntot = nFibreRows * nFibreColumns;
            dA = b * h / ntot;
            elementx = new double[nFibreRows, nFibreColumns];
            elementy = new double[nFibreRows, nFibreColumns];

            for (int i = 0; i < nFibreRows; i++)
            {
                for (int j = 0; j < nFibreColumns; j++)
                {
                    elementx[i, j] = (j + 1) * b / nFibreColumns - b / nFibreColumns / 2;
                    elementy[i, j] = (i + 1) * h / nFibreRows - h / nFibreRows / 2;
                }
            }
        }

        /// <summary>
        /// Calculates the capacity of a section for a given curvature.
        /// </summary>
        /// <param name="curvature"></param>
        /// <param name="initialConcreteStrain">Start the search for equilibrium from an initial strain.</param>
        /// <returns></returns>
        public AnalysisResults getCapacityForConstantCurvature(double curvature, double initialConcreteStrain)
        {
            ConcreteStrainIncrementor csi = new ConcreteStrainIncrementor(curvature, initialConcreteStrain);
            return findInternalForceEquilibrium(csi);
        }

        /// <summary>
        /// Calculates the ultimate capacity of a section.
        /// </summary>
        /// <param name="curvatureLimit">The maximum curvature before search stops.</param>
        /// <returns></returns>
        public AnalysisResults getUltimateCapacity(double curvatureLimit)
        {
            CurvatureIncrementor ci = new CurvatureIncrementor(concrete.ecu, curvatureLimit);
            return findInternalForceEquilibrium(ci);
        }

        /// <summary>
        /// Calculates a RC section's moment capacity by incrementing a given quantity until internal forces balance out.<br/>
        /// If a material exceeds ultimate strain, the analysis stops and a FailureMode is assigned.
        /// </summary>
        /// <param name="analysisIcrementor">An iterator which knows which quantity to increment and which to keep constant.</param>
        /// <returns></returns>
        private AnalysisResults findInternalForceEquilibrium(AnalysisIncrementor analysisIcrementor)
        {
            AnalysisResults results = new AnalysisResults();

            //variables for equilibrium
            double forceBalance = double.PositiveInfinity;
            double oldbal = forceBalance;

            SectionInternalForces internalForces = new SectionInternalForces(this);

            int iterations = 0;
            while (forcesNotInBalance(forceBalance))
            {
                internalForces.calculateInternalForces(analysisIcrementor.curvature, analysisIcrementor.concreteStrain);
                forceBalance = internalForces.getForcesBalance();
                oldbal = getValueToUpdateBalanceAndAdaptIncrement(analysisIcrementor, forceBalance, oldbal);

                try
                {
                    analysisIcrementor.incrementQuantity();
                }
                catch (IncrementalValueExceededMaximum)
                {
                    results.setFailureMode(AnalysisResults.FailureMode.NoConvergence);
                    return results;
                }

                if (iterations++ > MAX_ALLOWED_ITERATIONS)
                {
                    results.setFailureMode(AnalysisResults.FailureMode.NoConvergence);
                    return results;
                }
            }

            //Check for material failure
            if (internalForces.concreteStrain > concrete.ecu)
            {
                results.setFailureMode(AnalysisResults.FailureMode.Concrete);
            }
            else if ((internalForces.As1strain > steel.esud) || (internalForces.As2strain > steel.esud))
            {
                results.setFailureMode(AnalysisResults.FailureMode.Steel);
            }

            double Mtot = internalForces.calculateInternalMoment();

            results.setResults(internalForces, Mtot);

            return results;
        }

        private static double getValueToUpdateBalanceAndAdaptIncrement(AnalysisIncrementor analysisIcrementor, double forceBalance, double oldbal)
        {
            if (forceBalance == oldbal)
            {
                //if the balance didn't change, the neutral axis is outside the section
                //and we may need to speed up the increment
                analysisIcrementor.scaleIncrement(2);
            }
            else if (forceBalanceChangedSign(forceBalance, oldbal))
            {
                analysisIcrementor.revertOneStepAndHalveIncrement();
            }
            else if (forceBalanceDivergedFromLastStep(forceBalance, oldbal))
            {
                analysisIcrementor.changeSignOfIncrement();
                oldbal = forceBalance;
            }
            else
            {
                if (!double.IsInfinity(oldbal))
                {
                    double incrementScaleFactor = Math.Min(Math.Abs(oldbal) / Math.Abs(oldbal - forceBalance) / 2, 10);
                    analysisIcrementor.scaleIncrement(incrementScaleFactor);
                }

                oldbal = forceBalance;
            }

            return oldbal;
        }

        private static bool forceBalanceDivergedFromLastStep(double forceBalance, double oldbal)
        {
            return Math.Abs(forceBalance) > Math.Abs(oldbal);
        }

        private static bool forceBalanceChangedSign(double forceBalance, double oldbal)
        {
            return (forceBalance * oldbal < 0) && (!double.IsInfinity(forceBalance * oldbal));
        }

        private bool forcesNotInBalance(double forceBalance)
        {
            return Math.Abs(forceBalance) > forceBalanceTolerance;
        }

        /// <summary>
        /// Checks if the capacity of the member is not exceeded both for tension and compression.
        /// Throws an exception if capacity is exceeded.
        /// </summary>
        /// <exception cref="CompressionCapacityExceeded"></exception>
        /// <exception cref="TensionCapacityExceeded"></exception>
        public void checkAxialForceCapacityAndThrowExceptionIfExceeded()
        {
            if (N < 0)  //compression
            {
                if (axialCompressionIsLargerThanCapacity())
                {
                    throw new CompressionCapacityExceeded(getCompressionCapacity());
                }
            }
            else
            {
                if (axialTensionIsLargerThanCapacity())
                {
                    throw new TensionCapacityExceeded(getTensionCapacity());
                }
            }
        }

        private bool axialCompressionIsLargerThanCapacity()
        {
            double maxCompressionCapacity = getCompressionCapacity();

            return Math.Abs(N) > maxCompressionCapacity;
        }

        private bool axialTensionIsLargerThanCapacity()
        {
            double maxTensionCapacity = getTensionCapacity();

            return Math.Abs(N) > maxTensionCapacity;
        }

        private double getTensionCapacity()
        {
            return (As1 + As2) * fyd;
        }

        private double getCompressionCapacity()
        {
            return acc * fcd * (b * h - As1 - As2) + (As1 + As2) * fyd;
        }

        public class CompressionCapacityExceeded : System.Exception
        {
            public CompressionCapacityExceeded(double maxCompressionCapacity) :
                base(String.Format("Maximum compression capacity {0:0.00}kN of section exceeded.", maxCompressionCapacity))
            { }
        }

        public class TensionCapacityExceeded : System.Exception
        {
            public TensionCapacityExceeded(double maxTensionCapacity) :
                base(String.Format("Maximum tension capacity {0:0.00}kN of section exceeded.", maxTensionCapacity))
            { }
        }
    }
}
