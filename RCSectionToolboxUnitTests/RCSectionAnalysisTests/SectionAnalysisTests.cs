using RCSectionToolbox.Materials;
using RCSectionToolbox;
using RCSectionToolbox.RCSectionAnalysis;
using static System.Collections.Specialized.BitVector32;

namespace RCSectionToolboxUnitTests.RCSectionAnalysisTests
{
    [TestFixture]
    public class SectionAnalysisTests
    {
        private const double TOLERANCE = 0.001;
        private const double acc = 0.85;
        private Concrete concrete;
        private Steel steel;
        RCSection section;

        [SetUp]
        public void setUp()
        {
            double As1 = 2 * 0.014 * 0.014 * Math.PI / 4;
            double As2 = As1;
            concrete = new EurocodeConcrete(20000);
            steel = new EurocodeSteel(500000);
            section = new RCSection(concrete, steel, 0.4, 0.3, 0, 0.05, As1, As2);
        }

        private double getTensionCapacity()
        {
            return (section.Asbot + section.Astop) * section.steel.fyd;
        }

        private double getCompressionCapacity()
        {
            return acc * section.concrete.fcd *
                (section.width * section.height - section.Asbot - section.Astop) +
                (section.Asbot + section.Astop) * section.steel.fyd;
        }

        [Test]
        public void whenAxialTensionExceeded_shouldThrow()
        {
            double axialForce = getTensionCapacity() * 2;   //force it to exceed capacity
            section.axialForce = axialForce;
            SectionAnalysis analysis = new SectionAnalysis(section);
            Assert.Throws<SectionAnalysis.TensionCapacityExceeded>(() => analysis.checkAxialForceCapacityAndThrowExceptionIfExceeded());
        }

        [Test]
        public void whenAxialTensionNotExceeded_shouldNotThrow()
        {
            double axialForce = getTensionCapacity() / 2;
            section.axialForce = axialForce;
            SectionAnalysis analysis = new SectionAnalysis(section);
            Assert.DoesNotThrow(() => analysis.checkAxialForceCapacityAndThrowExceptionIfExceeded());
        }

        [Test]
        public void whenAxialCompressionExceeded_shouldThrow()
        {
            double axialForce = getCompressionCapacity() * -2;   //force it to exceed capacity
            section.axialForce = axialForce;
            SectionAnalysis analysis = new SectionAnalysis(section);
            Assert.Throws<SectionAnalysis.CompressionCapacityExceeded>(() => analysis.checkAxialForceCapacityAndThrowExceptionIfExceeded());
        }

        [Test]
        public void whenAxialCompressionNotExceeded_shouldNotThrow()
        {
            double axialForce = getCompressionCapacity() / -2;
            section.axialForce = axialForce;
            SectionAnalysis analysis = new SectionAnalysis(section);
            Assert.DoesNotThrow(() => analysis.checkAxialForceCapacityAndThrowExceptionIfExceeded());
        }
    }
}
