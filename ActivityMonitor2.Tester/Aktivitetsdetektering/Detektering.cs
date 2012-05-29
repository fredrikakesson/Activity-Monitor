using System;
using System.Threading;
using ActivityMonitor2.Doman;
using ActivityMonitor2.Doman.Aktivitetsdetektor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace ActivityMonitor2.Tests.Aktivitetsdetektering
{
    public class GivetEnDetektor
    {
        protected readonly IAnvändaraktivitet Användaraktivitet;
        protected readonly IStrömspararkontroll Strömspararkontroll;
        protected readonly IAktivitetsdetektor Detektor;
        protected ITimer Timer;

        public GivetEnDetektor()
        {
            Timer = Substitute.For<ITimer>();
            Strömspararkontroll = Substitute.For<IStrömspararkontroll>();
            Användaraktivitet = Substitute.For<IAnvändaraktivitet>();
            Detektor = new Aktivitetsdetektor(Timer, Användaraktivitet, Strömspararkontroll);
        }
    }

    [TestClass]
    public class GivetAttAnvändarenÄrAktiv : GivetEnDetektor
    {
        private bool _aktivitetUpptäckt;

        public GivetAttAnvändarenÄrAktiv()
        {
            Användaraktivitet.ÄrAktiv().Returns(true);
            Detektor.AktivitetUpptäckt += (s, a) => _aktivitetUpptäckt = true;
            Timer.Tick += Raise.Event();
        }

        [TestMethod]
        public void SåKastarDetektornRättEvent()
        {
            Assert.IsTrue(_aktivitetUpptäckt);
        }

        [TestMethod]
        public void SåHarDetektornAnvändarnamnet()
        {
            Assert.AreEqual(Environment.UserName, Detektor.NuvarandeAnvändare);
        }
    }

    [TestClass]
    public class GivetAttAnvändarenÄrInaktiv : GivetEnDetektor
    {
        private bool _inaktivitetUpptäckt;

        public GivetAttAnvändarenÄrInaktiv()
        {
            Användaraktivitet.ÄrAktiv().Returns(false);
            Detektor.InaktivitetUpptäckt += (s, a) => _inaktivitetUpptäckt = true;
            Timer.Tick += Raise.Event();
        }

        [TestMethod]
        public void SåKastarDetektornRättEvent()
        {
            Assert.IsTrue(_inaktivitetUpptäckt);
        }
    }

    [TestClass]
    public class NärAnvändaraktivitetKörs
    {
        private readonly Användaraktivitet _aktivitet;

        public NärAnvändaraktivitetKörs()
        {
            _aktivitet = new Användaraktivitet(600);
            // Förutsätter att någon varit aktiv senaste 10 minuterna. Funkar ju inte på en byggserver...
        }

        [TestMethod]
        public void SåHarNågonVaritAktiv()
        {
            Assert.IsTrue(_aktivitet.ÄrAktiv());
        }
    }

    [TestClass]
    public class NärTimernKörs
    {
        private int _tickräknare;

        public NärTimernKörs()
        {
            var timer = new SystemTimer(1);
            timer.Tick += (s, a) => _tickräknare++;
        }

        [TestMethod, Ignore]
        public void SåTickarDet()
        {
            Thread.Sleep(2500);
            Assert.AreEqual(2, _tickräknare);
        }
    }

    [TestClass]
    public class NärDatornGårNerIStrömsparläge : GivetEnDetektor
    {
        private bool _inaktivitetUpptäckt;

        public NärDatornGårNerIStrömsparläge()
        {
            Detektor.InaktivitetUpptäckt += (s, a) => _inaktivitetUpptäckt = true;
            Strömspararkontroll.GårNerIStrömsparläge += Raise.Event();
        }

        [TestMethod]
        public void RegistrerasEnInaktivitet()
        {
            Assert.IsTrue(_inaktivitetUpptäckt);
        }
    }
}