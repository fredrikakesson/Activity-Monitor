using System;
using ActivityMonitor2.Doman.ExtensionMethods;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ActivityMonitor2.Tests.Diverse_kod
{
    [TestClass]
    public class TimeSpanExtension
    {
        [TestMethod]
        public void EnbartMinut()
        {
            var ts = new TimeSpan(0, 0, 1, 0);
            Assert.AreEqual("1m", ts.ToShortString());
        }

        [TestMethod]
        public void EnbartTimme()
        {
            var ts = new TimeSpan(0, 2, 0, 0);
            Assert.AreEqual("2h", ts.ToShortString());
        }

        [TestMethod]
        public void BådeTimmeOchMinut()
        {
            var ts = new TimeSpan(0, 2, 1, 0);
            Assert.AreEqual("2h 1m", ts.ToShortString());
        }

        [TestMethod]
        public void BaraSekunder()
        {
            var ts = new TimeSpan(0, 0, 0, 5);
            Assert.AreEqual("0m", ts.ToShortString());
        }
    }
}