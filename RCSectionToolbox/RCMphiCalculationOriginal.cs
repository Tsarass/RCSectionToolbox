using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RCSectionToolbox
{
    /// <summary>
    /// Calculate m-phi curve for a rectangular section with top and bottom reinforcement
    /// using the fibre method.
    /// </summary>
    public class RCMphiCalculationOriginal
    {
        private static double h;
        private static double b;
        private static double N;
        private static double fc;
        private static double fy;
        private static double c;
        private static double As1;
        private static double As2;

        private const double Ey = 200000000;    //Young modulus of steel
        private const double acc = 0.85;        //reduction factor for long term effects on concrete strength
        private const double forceEquilibriumTolerance = 0.001; //tolerance of the equilibrium search

        //Fibre variables
        private static int nFibreRows = 100;
        private static int nFibreColumns = 2;
        private static double dA;
        private static double[,] elementx, elementy;

        private static void parseInputObject(RCSection input)
        {
            h = input.height;
            b = input.width;
            N = input.axialForce;
            fc = input.concrete.fc;
            fy = input.steel.fy;
            c = input.cover;
            As1 = input.Asbot;
            As2 = input.Astop;
        }

        private static void initializeSectionFibres()
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

        public static RCMphiCalculationResults calculateMphiResults(RCSection input)
        {
            parseInputObject(input);

            //create new results object
            RCMphiCalculationResults results = new RCMphiCalculationResults();

            double ecy = 2d / 1000;
            double ecu = 3.5d / 1000;

            //EC2 material properties
            var ecu2 = ecu;

            checkAxialForceCapacityAndThrowExceptionIfExceeded();

            initializeSectionFibres();

            //start moment-curvature analysis
            double fend = 0.1;
            double fstart = 0;
            double fstep = 0.0001;
            int maxIterations = (int)((fend - fstart) / fstep);
            double[,] Mf = new double[maxIterations, 2];
            bool concreteFailed = false;

            double f;

            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                f = fstart + fstep * (iteration + 1);

                double e0 = 0;
                double forceBalance = double.PositiveInfinity;
                double oldbal = forceBalance;
                double e0step = 0.005 / 1000;

                //init variables
                double[,] concreteForces = new double[nFibreRows, nFibreColumns];
                double forces1 = 0;
                double forces2 = 0;
                double ys1 = h - c;
                double ys2 = c;

                while (Math.Abs(forceBalance) > forceEquilibriumTolerance)
                {
                    e0 += e0step;
                    double neutralAxisY = e0 / f;
                    concreteForces = calculateConcreteForces(ecy, ecu, e0, neutralAxisY);
                    double concforce = integrateConcreteForces(concreteForces);

                    //reinforcement forces
                    var distanceOfAs2 = ys2 - neutralAxisY;
                    var es2 = e0 * distanceOfAs2 / neutralAxisY;
                    var stress2 = steelStress(fy, Ey, es2);
                    forces2 = stress2 * As2;

                    var distanceOfAs1 = ys1 - neutralAxisY;
                    var es1 = e0 * distanceOfAs1 / neutralAxisY;
                    var stress1 = steelStress(fy, Ey, es1);
                    forces1 = stress1 * As1;

                    //write to results object
                    //results._es1 = es1;
                    //results._es2 = es2;
                    //results._Fs1 = forces1;
                    //results._Fs2 = forces2;
                    //results._Fc = concforce;
                    //results.ec = e0;

                    forceBalance = -concforce + forces2 + forces1 - N;

                    if (forceBalance * oldbal < 0)
                    {
                        //halve the step and go 1 step back
                        e0 -= e0step;
                        e0step /= 2;
                    }
                    else
                    {
                        oldbal = forceBalance;
                    }

                    if (e0 > ecu2)
                    {
                        concreteFailed = true;
                        break;
                    }
                }

                if (concreteFailed)
                {
                    removeRowsFromResultsArrayAfterConcreteFailure(ref Mf, iteration);
                    break;
                }

                double Mc = calculateConcreteMoment(concreteForces);

                //steel moments
                double Ms1 = forces1 * ys1;
                double Ms2 = forces2 * ys2;

                double Mtot = Mc + Ms1 + Ms2 - N * h / 2;
                Mf[iteration, 0] = Mtot;
                Mf[iteration, 1] = f;
            }

            results.Mphi = Mf;
            return results;
            
        }

        private static double[,] calculateConcreteForces(double ecy, double ecu, double e0, double neutralAxisY)
        {
            double[,] stresses = new double[nFibreRows, nFibreColumns];
            double[,] forces = new double[nFibreRows, nFibreColumns];

            for (int i = 0; i < nFibreRows; i++)
            {
                double distanceFromNeutralAxis = elementy[i, 0] - neutralAxisY;
                double eij = e0 * distanceFromNeutralAxis / neutralAxisY;
                for (int j = 0; j < nFibreColumns; j++)
                {
                    stresses[i, j] = concreteStress(fc, -eij, ecy, ecu);
                    forces[i, j] = acc * stresses[i, j] * dA;
                }
            }
            return forces;
        }

        /// <summary>
        /// Checks if the capacity of the member is not exceeded both for tension and compression.
        /// Throws an exception if capacity is exceeded.
        /// </summary>
        /// <exception cref="CompressionCapacityExceeded"></exception>
        /// <exception cref="TensionCapacityExceeded"></exception>
        private static void checkAxialForceCapacityAndThrowExceptionIfExceeded()
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

        private static bool axialCompressionIsLargerThanCapacity()
        {
            double maxCompressionCapacity = getCompressionCapacity();

            return Math.Abs(N) > maxCompressionCapacity;
        }

        private static bool axialTensionIsLargerThanCapacity()
        {
            double maxTensionCapacity = getTensionCapacity();

            return Math.Abs(N) > maxTensionCapacity;
        }

        private static double getTensionCapacity()
        {
            return (As1 + As2) * fy;
        }

        private static double getCompressionCapacity()
        {
            return acc * fc * (b * h - As1 - As2) + (As1 + As2) * fy;
        }

        private static double calculateConcreteMoment(double[,] forces)
        {
            double Mc = 0;
            for (int i = 0; i < nFibreRows; i++)
            {
                for (int j = 0; j < nFibreColumns; j++)
                {
                    var y = elementy[i, j];
                    Mc -= forces[i, j] * y;
                }
            }

            return Mc;
        }

        private static double integrateConcreteForces(double[,] forces)
        {
            double concforce = 0;
            for (int i = 0; i < nFibreRows; i++)
            {
                for (int j = 0; j < nFibreColumns; j++)
                {
                    concforce += forces[i, j];
                }
            }
            return concforce;
        }

        private static double concreteStress(double fc, double e0, double ec2, double ecu2)
        {

            if ((e0 > 0) && (e0 < ec2))
            {
                return fc * (1 - powerOf2(1 - e0 / ec2));
            }
            else if ((e0 >= ec2) && (e0 <= ecu2))
            {
                return fc;
            }

            return 0;
        }

        private static double steelStress(double fy, double Ey, double e0)
        {
            double ey = fy / Ey;
            double e0_ = e0;

            if (Math.Abs(e0) >= ey)
            {
                if (e0 > 0)
                {
                    e0_ = ey;
                }
                else
                {
                    e0_ = -ey;
                }
            }

            return e0_ * Ey;
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

        private static double powerOf2(double num)
        {
            return num * num;
        }

        private class CompressionCapacityExceeded : System.Exception
        {
            public CompressionCapacityExceeded(double maxCompressionCapacity) :
                base(String.Format("Maximum compression capacity {0:0.00}kN of section exceeded.", maxCompressionCapacity))
            { }
        }

        private class TensionCapacityExceeded : System.Exception
        {
            public TensionCapacityExceeded(double maxTensionCapacity) :
                base(String.Format("Maximum tension capacity {0:0.00}kN of section exceeded.", maxTensionCapacity))
            { }
        }
    }
}
