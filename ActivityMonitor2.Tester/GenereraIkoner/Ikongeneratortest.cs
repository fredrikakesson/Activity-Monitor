using System.Drawing;
using ActivityMonitor2.Doman.Ikongenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ActivityMonitor2.Tests.GenereraIkoner
{
    [TestClass]
    public class NärJagBerOmEnIkon
    {
        private readonly Icon _ico;

        public NärJagBerOmEnIkon()
        {
            var generator = new Ikongenerator();
            _ico = generator.SkapaTrayikon((float) 0.67, true);
        }

        [TestMethod]
        public void SåFårJagEnIkon()
        {
            Assert.IsNotNull(_ico);
        }
    }
}