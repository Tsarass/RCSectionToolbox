using RCSectionToolbox.Materials;

namespace RCSectionToolboxUnitTests.MaterialsTests
{
    [TestFixture]
    public class EurocodeConcreteTests
    {
        private EurocodeConcrete concreteDefault;
        private EurocodeConcrete concreteCustomEcu2;
        private double fc = 20000; //kPa

        [SetUp]
        public void setUpEurocodeConcrete()
        {
            concreteDefault = new EurocodeConcrete(fc);
            concreteCustomEcu2 = new EurocodeConcrete(fc, 0.005);
        }

        [Test]
        public void testConcreteStrength()
        {
            Assert.That(concreteDefault.fc, Is.EqualTo(fc));
        }

        [Test]
        public void testSafetyFactor()
        {
            double safetyFactor = 1.5;
            Assert.That(concreteDefault.fcd, Is.EqualTo(fc/safetyFactor));
        }

        [Test]
        public void stressAtZeroStrainShouldBeZero()
        {
            Assert.That(concreteDefault.getStress(0), Is.EqualTo(0));
        }

        [Test]
        public void stressAtNegativeStrainShouldBeZero()
        {
            Assert.That(concreteDefault.getStress(-0.1), Is.EqualTo(0));
        }

        [Test]
        public void testStressBeforeYield()
        {
            Assert.That(concreteDefault.getStress(0.001), Is.EqualTo(10000));
        }

        [Test]
        public void testStressAfterYield()
        {
            Assert.That(concreteDefault.getStress(0.003), Is.EqualTo(concreteDefault.fcd));
        }
    }
}
