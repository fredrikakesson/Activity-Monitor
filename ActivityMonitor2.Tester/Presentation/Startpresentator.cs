using System;
using System.Collections.Generic;
using ActivityMonitor2.Doman;
using ActivityMonitor2.Doman.Entiteter;
using ActivityMonitor2.GUI;
using ActivityMonitor2.GUI.Formular.Vyer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace ActivityMonitor2.Tests.Presentation
{
    public class GivetEnPresentatör
    {
        protected readonly IAktivitetsdetektor Detektor;
        protected readonly Presentatör Presentatör;
        protected readonly ITimer Timer;
        protected readonly IStartvy Vy;
        protected readonly ILagring Lagring;

        public GivetEnPresentatör()
        {
            Vy = Substitute.For<IStartvy>();
            Detektor = Substitute.For<IAktivitetsdetektor>();
            Timer = Substitute.For<ITimer>();

            Lagring = Substitute.For<ILagring>();

            Presentatör = new Presentatör(Vy, Detektor, Lagring, Timer);
        }
    }

    public class GivetAttDetFinnsLagradData : GivetEnPresentatör
    {
        public GivetAttDetFinnsLagradData()
        {
            Lagring.HämtaAllaFörEnVissDag(Arg.Any<DateTime>()).Returns(Aktivitetslista());
        }

        private static IList<AktivPeriod> Aktivitetslista()
        {
            var lista = new List<AktivPeriod>
                            {
                                new AktivPeriod
                                    {
                                        Starttid = new DateTime(2001, 1, 1, 1, 0, 0),
                                        Tidsmängd = new TimeSpan(1, 0, 0),
                                        Användarnamn = "Test A"
                                    },
                                new AktivPeriod
                                    {
                                        Starttid = new DateTime(2001, 1, 1, 3, 0, 0),
                                        Tidsmängd = new TimeSpan(0, 30, 0),
                                        Användarnamn = "Test A"
                                    }
                            };
            return lista;
        }
    }

    [TestClass]
    public class NärPresentatörenStartar : GivetEnPresentatör
    {
        [TestMethod]
        public void SåVisasInteAktivDel()
        {
            // Aktiv del ska inte visas förrän aktivitet upptäcks
            Vy.Received(0).VisaAktivDel(Arg.Any<double>(), false);
        }
    }


    // TODO: Vad ska hända då?
    [TestClass]
    public class NärAktivitetUpptäcks : GivetEnPresentatör
    {
        public NärAktivitetUpptäcks()
        {
            Detektor.AktivitetUpptäckt += Raise.Event();
        }


        [TestMethod]
        public void SåVisasDetFörAnvändaren()
        {
        //    Vy.Received(1).VisaSomAktiv();
        }
    }

    // TODO: Vad ska hända då?
    [TestClass]
    public class NärInaktivitetUpptäcks : GivetEnPresentatör
    {
        public NärInaktivitetUpptäcks()
        {
            SystemTime.Now = () => new DateTime(2001, 1, 1, 4, 0, 0);
            Detektor.InaktivitetUpptäckt += Raise.Event();
        }

        [TestMethod]
        public void SåVisasDetFörAnvändaren()
        {
  //          Vy.Received(1).VisaSomInaktiv();
        }
    }

    [TestClass]
    public class NärAktivDelTimernTickar : GivetAttDetFinnsLagradData
    {
        public NärAktivDelTimernTickar()
        {
            SystemTime.Now = () => new DateTime(2001, 1, 1, 4, 0, 0);
            Detektor.AktivitetUpptäckt += Raise.Event();
            Timer.Tick += Raise.Event();
        }

        [TestMethod]
        public void SåVisasRättAndelAktiv()
        {
            Vy.Received(1).VisaAktivDel(0.5, true);
        }
    }


    [TestClass]
    public class NärAktivDelTimernTickarUnderPågåendeAktivitet : GivetAttDetFinnsLagradData
    {
        public NärAktivDelTimernTickarUnderPågåendeAktivitet()
        {
            // Användaren har varit aktiv en halvtimme
            SystemTime.Now = () => new DateTime(2001, 1, 1, 3, 30, 0);
            Detektor.AktivitetUpptäckt += Raise.Event();

            SystemTime.Now = () => new DateTime(2001, 1, 1, 4, 0, 0);
            Timer.Tick += Raise.Event();
        }

        [TestMethod]
        public void SåVisasRättAndelAktiv()
        {
            Vy.Received(1).VisaAktivDel((double)120 / 180, true);
        }
    }

    [TestClass]
    public class NärAktivDelTimernTickarUnderFörstaAktiviteten : GivetEnPresentatör
    {
        public NärAktivDelTimernTickarUnderFörstaAktiviteten()
        {
            // Användaren har varit aktiv en halvtimme
            SystemTime.Now = () => new DateTime(2001, 1, 1, 3, 30, 0);
            Detektor.AktivitetUpptäckt += Raise.Event();

            SystemTime.Now = () => new DateTime(2001, 1, 1, 4, 0, 0);
            Timer.Tick += Raise.Event();
        }

        [TestMethod]
        public void SåVisasRättAndelAktiv()
        {
            Vy.Received(1).VisaAktivDel(1, true);
        }
    }


    [TestClass]
    public class NärAnvändarenVillVisaGanttSchemat : GivetEnPresentatör
    {
        private bool _presentatörsevent;

        public NärAnvändarenVillVisaGanttSchemat()
        {
            Presentatör.VisaGanttschema += (s, a) => _presentatörsevent = true;
            Vy.VisaDagsöversikt += Raise.Event();
        }

        [TestMethod]
        public void SåKastasEttEventOmDet()
        {
            Assert.IsTrue(_presentatörsevent);
        }
    }

    [TestClass]
    public class NärAnvändarenVillVisaVeckoöversikten : GivetEnPresentatör
    {
        private bool _presentatörsevent;

        public NärAnvändarenVillVisaVeckoöversikten()
        {
            Presentatör.VisaVeckoöversikt += (s, a) => _presentatörsevent = true;
            Vy.VisaVeckoöversikt += Raise.Event();
        }

        [TestMethod]
        public void SåKastasEttEventOmDet()
        {
            Assert.IsTrue(_presentatörsevent);
        }
    }
}