using RCSectionToolbox.RCSectionAnalysis;
using RCSectionToolbox;
using RCSectionToolbox.Materials;

namespace RCSectionToolboxUnitTests.RCSectionAnalysisTests
{
    public class SectionInternalForcesTests
    {
        private const double TOLERANCE = 0.001;
        private const double N = -100;
        private RCSection section;
        private SectionAnalysis analysis;
        private SectionAnalysis.SectionInternalForces internalForces;

        [SetUp]
        public void setUp()
        {
            double As1 = 2 * 0.014 * 0.014 * Math.PI / 4;
            double As2 = As1;
            Concrete concrete = new EurocodeConcrete(20000);
            Steel steel = new EurocodeSteel(500000);
            section = new RCSection(concrete, steel, 0.4, 0.3, N, 0.05, As1, As2);
            analysis = new SectionAnalysis(section);
            internalForces = new SectionAnalysis.SectionInternalForces(analysis);
        }

        [Test]
        public void testSimpleCase()
        {
            internalForces.calculateInternalForces(0.01, 0.0015);
            Assert.That(internalForces.concreteResultantForce, Is.EqualTo(286.824).Within(TOLERANCE));
            Assert.That(internalForces.As1force, Is.EqualTo(123.15).Within(TOLERANCE));
            Assert.That(internalForces.As2force, Is.EqualTo(-61.575).Within(TOLERANCE));
            Assert.That(internalForces.calculateInternalMoment(), Is.EqualTo(44.489).Within(TOLERANCE));
            Assert.That(internalForces.getForcesBalance(), Is.EqualTo(-125.249).Within(TOLERANCE));
        }
    }
}
