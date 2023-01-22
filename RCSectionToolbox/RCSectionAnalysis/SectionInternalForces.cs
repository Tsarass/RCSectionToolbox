using RCSectionToolbox.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCSectionToolbox.RCSectionAnalysis
{
    public partial class SectionAnalysis
    {
        public class SectionInternalForces
        {
            private SectionAnalysis analysis;

            public double[,] concreteForces;
            public double As1force;
            public double As2force;
            public double concreteResultantForce;
            public double As1strain;
            public double As2strain;
            public double neutralAxisY;
            public double concreteStrain;
            public double curvature;

            /// <summary>
            /// Creates an instance of internal forces class.
            /// </summary>
            /// <param name="analysisInstance">The calling instance of analysis class, for referencing.</param>
            public SectionInternalForces(SectionAnalysis analysisInstance)
            {
                this.analysis = analysisInstance;
            }

            private void setInternalForces(double phi, double[,] concreteForces, double forceAs1, double forceAs2, double strainAs1, double strainAs2, double strainConcrete, double neutralAxisY)
            {
                this.curvature = phi;
                this.concreteForces = concreteForces;
                this.As1force = forceAs1;
                this.As2force = forceAs2;
                this.concreteResultantForce = calculateConcreteResultantForce();
                this.As1strain = strainAs1;
                this.As2strain = strainAs2;
                this.concreteStrain = strainConcrete;
                this.neutralAxisY = neutralAxisY;
            }

            public void calculateInternalForces(double curvature, double concreteStrain)
            {
                double neutralAxisY = concreteStrain / curvature;

                double[,] concreteForces = calculateConcreteForces(concreteStrain, neutralAxisY);

                double strainAs2 = calculateReinforcementStrain(analysis.ys2, neutralAxisY, concreteStrain);
                double forceAs2 = analysis.steel.getStress(strainAs2) * analysis.As2;

                double strainAs1 = calculateReinforcementStrain(analysis.ys1, neutralAxisY, concreteStrain);
                double forceAs1 = analysis.steel.getStress(strainAs1) * analysis.As1;

                setInternalForces(curvature, concreteForces, forceAs1, forceAs2, strainAs1, strainAs2, concreteStrain, neutralAxisY);
            }
            public double calculateInternalMoment()
            {
                double Mc = calculateConcreteMoment(concreteForces);
                double Ms1 = As1force * analysis.ys1;
                double Ms2 = As2force * analysis.ys2;
                return Mc + Ms1 + Ms2 - analysis.N * analysis.h / 2;
            }

            /// <summary>
            /// Calculate the internal force balance with a given axial force.
            /// </summary>
            /// <param name="axialForce">Positive for tension.</param>
            /// <returns></returns>
            public double getForcesBalance()
            {
                return -concreteResultantForce + As2force + As1force - analysis.N;
            }

            private double calculateReinforcementStrain(double reinforcementY, double neutralAxisY, double concreteStrain)
            {
                var distanceFromNeutralAxis = reinforcementY - neutralAxisY;
                return concreteStrain * distanceFromNeutralAxis / neutralAxisY;
            }

            private double[,] calculateConcreteForces(double e0, double neutralAxisY)
            {
                double[,] forces = new double[analysis.nFibreRows, analysis.nFibreColumns];

                for (int i = 0; i < analysis.nFibreRows; i++)
                {
                    double distanceFromNeutralAxis = analysis.elementy[i, 0] - neutralAxisY;
                    double eij = e0 * distanceFromNeutralAxis / neutralAxisY;
                    if (eij is double.NaN)
                        eij = 0;
                    if (eij >= 0)
                        continue;

                    for (int j = 0; j < analysis.nFibreColumns; j++)
                    {
                        var stress = analysis.concrete.getStress(-eij);
                        forces[i, j] = acc * stress * analysis.dA;
                    }
                }
                return forces;
            }

            private double calculateConcreteResultantForce()
            {
                double concforce = 0;
                for (int i = 0; i < analysis.nFibreRows; i++)
                {
                    for (int j = 0; j < analysis.nFibreColumns; j++)
                    {
                        concforce += concreteForces[i, j];
                    }
                }
                return concforce;
            }

            private double calculateConcreteMoment(double[,] forces)
            {
                double Mc = 0;
                for (int i = 0; i < analysis.nFibreRows; i++)
                {
                    for (int j = 0; j < analysis.nFibreColumns; j++)
                    {
                        var y = analysis.elementy[i, j];
                        Mc -= forces[i, j] * y;
                    }
                }

                return Mc;
            }
        }
    }
}
