namespace RCSectionToolbox.RCSectionAnalysis
{
    public partial class SectionAnalysis
    {
        public class AnalysisResults
        {
            /// <summary>
            /// Determines by which material the failure is controlled.
            /// </summary>
            public enum FailureMode
            {
                None,
                Concrete,
                Steel,
                NoConvergence
            }

            private SectionInternalForces _internalForces;
            private double _Meq;
            private FailureMode _failureMode = FailureMode.None;
            private bool _analysisFailed = false;

            /// <summary>
            /// Flag showing if the analysis could find solution for the given curvature.
            /// When false, a material exceeded ultimate strain.
            /// failureMode will show which material failed.
            /// </summary>
            public bool analysisFailed { get { return _analysisFailed; } }

            /// <summary>
            /// Indicates which material was responsible for the analysis failure.
            /// </summary>
            public FailureMode failureMode { get { return _failureMode; } }

            public SectionInternalForces internalForces { get { return _internalForces; } }
            public double sectionMoment { get { return _Meq; } }

            public void setResults(SectionInternalForces internalForces, double Meq)
            {
                _internalForces = internalForces;
                _Meq = Meq;
            }

            public void setFailureMode(FailureMode mode)
            {
                if (mode != FailureMode.None)
                {
                    _analysisFailed = true;
                    _failureMode = mode;
                }
            }
        }
    }
}