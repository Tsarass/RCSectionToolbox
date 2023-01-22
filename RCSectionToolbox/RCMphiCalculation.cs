using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RCSectionToolbox.RCSectionAnalysis;

namespace RCSectionToolbox
{
    /// <summary>
    /// Calculate m-phi curve for a rectangular section with top and bottom reinforcement
    /// using the fibre method.
    /// </summary>
    public class RCMphiCalculation
    {
        /// <summary>
        /// Calculate m-phi curve results for the given input.
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        /// <exception cref="SectionAnalysis.CompressionCapacityExceeded"></exception>
        /// <exception cref="SectionAnalysis.TensionCapacityExceeded"></exception>
        public static RCMphiCalculationResults calculateMphiResults(RCSection section)
        {
            section.checkIfUltimateAxialCapacityExceeded();

            //create new results object
            RCMphiCalculationResults results = new RCMphiCalculationResults();

            //parameters for moment-curvature analysis
            double fMax = 0.5;
            double fStep = 0.0001;
            double fStart = 0 + fStep;

            //run ultimate capacity analysis
            var ultimateCapacityAnalysisResults = section.getUltimateCapacity(fMax);
            if (ultimateCapacityAnalysisResults.analysisFailed)
            {
                results.analysisFailed = true;
                return results;
            }
            var maxCurvature = Math.Min(ultimateCapacityAnalysisResults.internalForces.curvature, fMax);

            int maxSteps = (int)(maxCurvature / fStep);
            double[,] mPhiResults = new double[maxSteps, 2];
            double concreteStrain = SectionAnalysis.MIN_ALLOWED_CONC_STRAIN;

            int iterationIndex = 0;
            for (double f = fStart; f < maxCurvature; f += fStep)
            {
                var analysisResults = section.getCapacityForGivenCurvature(f, concreteStrain);
                if (analysisResults.analysisFailed)
                {
                    removeRowsFromResultsArrayAfterConcreteFailure(ref mPhiResults, iterationIndex);
                    break;
                }
                else
                {
                    //start the next iteration from the last valid concrete strain
                    concreteStrain = analysisResults.internalForces.concreteStrain;
                }

                mPhiResults[iterationIndex, 0] = analysisResults.sectionMoment;
                mPhiResults[iterationIndex, 1] = f;
                iterationIndex++;
            }

            results.Mphi = mPhiResults;
            return results;
        }

        private static void removeRowsFromResultsArrayAfterConcreteFailure(ref double[,] Mf, int numElements)
        {
            int rows = numElements;
            int cols = Mf.GetLength(1);
            double[,] result = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[i, j] = Mf[i, j];
                }
            }

            Mf = result;
        }

    }
}
