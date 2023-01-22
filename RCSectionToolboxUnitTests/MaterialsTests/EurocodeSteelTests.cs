using RCSectionToolbox.Materials;

namespace RCSectionToolboxUnitTests.MaterialsTests
{
    [TestFixture]
    public class EurocodeSteelTests
    {
        private EurocodeSteel steel;
        private double fy = 500000; //kPa

        [SetUp]
        public void setUpEurocodeConcrete()
        {
            steel = new EurocodeSteel(fy);
        }

        [Test]
        public void testSteelStrength()
        {
            Assert.That(steel.fy, Is.EqualTo(fy));
        }

        [Test]
        public void testSafetyFactor()
        {
            double safetyFactor = 1.15;
            Assert.That(steel.fyd, Is.EqualTo(fy / safetyFactor));
        }

        [Test]
        public void stressAtZeroStrainShouldBeZero()
        {
            Assert.That(steel.getStress(0), Is.EqualTo(0));
        }

        [Test]
        public void testStressBeforeYield()
        {
            double strain = 0.001;
            Assert.That(steel.getStress(strain), Is.EqualTo(steel.Es * strain));
        }

        [Test]
        public void testStressAfterYield()
        {
            double strain = 0.01;
            Assert.That(steel.getStress(strain), Is.EqualTo(steel.fyd));
        }
    }
}
