using System;
using ActivityMonitor2.Doman;
using ActivityMonitor2.Doman.Entiteter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace ActivityMonitor2.Tests.Overvakning
{
    public class GivetEnPågåendeÖvervakare
    {
        protected string Användare = "användare";
        protected IAktivitetsdetektor Detektor;
        protected ILagring Lagring;
        protected Övervakare Övervakaren;

        public GivetEnPågåendeÖvervakare()
        {
            Lagring = Substitute.For<ILagring>();
            Detektor = Substitute.For<IAktivitetsdetektor>();
            Detektor.NuvarandeAnvändare.Returns(Användare);

            Övervakaren = new Övervakare(Detektor, Lagring);
        }
    }

    [TestClass]
    public class NärEnPågåendeAktivitetAvslutas
        : GivetEnPågåendeÖvervakare
    {
        private DateTime _starttid;
        private TimeSpan _tidsmängd;

        [TestInitialize]
        public void NärEnStartOchEttSlutSker()
        {
            // Registrerar användaren är aktiv
            _starttid = new DateTime(2001, 1, 1, 0, 0, 0);
            SystemTime.Now = () => _starttid;
            Detektor.AktivitetUpptäckt += Raise.Event();

            //Registrerar inaktivitet
            _tidsmängd = new TimeSpan(1, 0, 0);
            SystemTime.Now = () => _starttid.Add(_tidsmängd);
            Detektor.InaktivitetUpptäckt += Raise.Event();
        }

        [TestMethod]
        public void SåSpararÖvervakarenEnPeriod()
        {
            Lagring.Received(1).Spara(Arg.Any<AktivPeriod>());
        }

        [TestMethod]
        public void SåÄrStarttidenRätt()
        {
            Lagring.Received(1).Spara(Arg.Is<AktivPeriod>(o => o.Starttid.Equals(_starttid)));
        }

        [TestMethod]
        public void SåÄrTidsmängdenRätt()
        {
            Lagring.Received(1).Spara(Arg.Is<AktivPeriod>(o => o.Tidsmängd.Equals(_tidsmängd)));
        }

        [TestMethod]
        public void SåHarAnvändarenSattsKorrekt()
        {
            Lagring.Received(1).Spara(Arg.Is<AktivPeriod>(o => o.Användarnamn.Equals(Användare)));
        }

    }

    [TestClass]
    public class NärAktivitetPågårOchÖvervakarenDisposas
        : GivetEnPågåendeÖvervakare
    {

        [TestInitialize]
        public void StartaAktivitetOchDisposaÖvervakaren()
        {
            Detektor.AktivitetUpptäckt += Raise.Event();
            Övervakaren.Dispose();
        }

        [TestMethod]
        public void SåSpararÖvervakarenEnPeriod()
        {
            Lagring.Received(1).Spara(Arg.Any<AktivPeriod>());
        }
    }
}