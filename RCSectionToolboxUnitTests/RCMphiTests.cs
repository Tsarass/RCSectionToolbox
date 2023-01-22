using RCSectionToolbox;
using RCSectionToolbox.Materials;

namespace RCSectionToolboxUnitTests
{

    [TestFixture]
    public class RCMphiTests
    {
        const double PHI_TOLERANCE = 0.0001;
        const double MOMENT_TOLERANCE = 0.01;

        [Test]
        public void testPrototypeCase()
        {
            //2D14 
            double As1 = 2 * 0.014 * 0.014 * Math.PI / 4;
            double As2 = As1;
            Concrete concrete = new EurocodeConcrete(20000);
            Steel steel = new EurocodeSteel(500000);
            RCSection section = new RCSection(concrete, steel, 0.4, 0.3, -100, 0.05, As1, As2);
            var results = RCMphiCalculationOriginal.calculateMphiResults(section);
            var results2 = RCMphiCalculation.calculateMphiResults(section);
            Assert.That(results2.Mphi.GetLength(0), Is.EqualTo(results.Mphi.GetLength(0)));
            Assert.That(results2.Mphi.GetLength(1), Is.EqualTo(results.Mphi.GetLength(1)));
            for (int i = 0; i < results.Mphi.GetLength(0); i++)
            {
                Assert.That(results2.Mphi[i, 0], Is.EqualTo(results.Mphi[i, 0]).Within(MOMENT_TOLERANCE), String.Format("iteration: i={0}", i));
                Assert.That(results2.Mphi[i, 1], Is.EqualTo(results.Mphi[i, 1]).Within(PHI_TOLERANCE), String.Format("iteration: i={0}", i));
            }
        }
    }
}
