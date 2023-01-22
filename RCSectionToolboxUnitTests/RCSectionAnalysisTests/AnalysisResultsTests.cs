using RCSectionToolbox.RCSectionAnalysis;
using static RCSectionToolbox.RCSectionAnalysis.SectionAnalysis.AnalysisResults;

namespace RCSectionToolboxUnitTests.RCSectionAnalysisTests
{
    [TestFixture]
    public class AnalysisResultsTests
    {
        SectionAnalysis.AnalysisResults results;

        [SetUp]
        public void setUp()
        {
            results = new SectionAnalysis.AnalysisResults();
        }

        [Test]
        public void resultsEmptyBeforeSetting()
        {
            Assert.IsNull(results.internalForces);
            Assert.That(results.sectionMoment, Is.EqualTo(0));
        }

        [Test]
        public void whenFailureSet_flagShouldBeTrue()
        {
            var failureMode = FailureMode.Concrete;
            results.setFailureMode(failureMode);
            Assert.IsTrue(results.analysisFailed);
            Assert.That(results.failureMode, Is.EqualTo(failureMode));
        }

        [Test]
        public void whenFailureSetToNone_flagShouldBeFalse()
        {
            var failureMode = FailureMode.None;
            results.setFailureMode(failureMode);
            Assert.IsFalse(results.analysisFailed);
            Assert.That(results.failureMode, Is.EqualTo(failureMode));
        }
    }
}
