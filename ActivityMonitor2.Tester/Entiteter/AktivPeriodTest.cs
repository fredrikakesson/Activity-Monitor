using System;
using ActivityMonitor2.Doman.Entiteter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ActivityMonitor2.Tests.Entiteter
{
    [TestClass]
    public class GivetEnPeriod
    {
        private const string Namn = "person 1";
        private readonly AktivPeriod _period;

        public GivetEnPeriod()
        {
            var starttid = new DateTime(2012, 5, 22, 10, 50, 0);
            var tidsmängd = new TimeSpan(1, 10, 0);
            _period = new AktivPeriod {Användarnamn = Namn, Starttid = starttid, Tidsmängd = tidsmängd};
        }

        [TestMethod]
        public void SåBerättarToStringAnvändarnamnet()
        {
            Assert.IsTrue(_period.ToString().Contains(Namn));
        }
    }
}