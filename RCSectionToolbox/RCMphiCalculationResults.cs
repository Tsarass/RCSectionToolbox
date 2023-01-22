using RCSectionToolbox.RCSectionAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCSectionToolbox
{
    public class RCMphiCalculationResults
    {
        public double[,] Mphi;
        public RCSectionAnalysis.SectionAnalysis.AnalysisResults[] analysisResults;

        public bool analysisFailed = false;
    }
}
