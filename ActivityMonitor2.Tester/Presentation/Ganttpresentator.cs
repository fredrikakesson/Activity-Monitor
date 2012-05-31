using System;
using System.Collections.Generic;
using ActivityMonitor2.Doman;
using ActivityMonitor2.Doman.Entiteter;
using ActivityMonitor2.GUI;
using ActivityMonitor2.GUI.Formular.Vyer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace ActivityMonitor2.Tests.Presentation.Ganttpresentatörstester
{
    public class GivetEnGanttPresentatör
    {
        protected readonly ILagring Lagring;
        protected readonly GanttPresentatör Presentatör;
        protected readonly IGanttVy Vy;

        public GivetEnGanttPresentatör()
        {
            Vy = Substitute.For<IGanttVy>();
            Lagring = Substitute.For<ILagring>();

            Presentatör = new GanttPresentatör(Vy, Lagring);
        }


    }

    public class GivetAttDetFinnsData : GivetEnGanttPresentatör
    {
        public GivetAttDetFinnsData()
        {
            Lagring.HämtaAllaPerioder().Returns(FejkadPeriodlista());
        }

        private static IList<AktivPeriod> FejkadPeriodlista()
        {
            return new List<AktivPeriod>
                       {
                           new AktivPeriod
                               {
                                   Användarnamn = "Person A",
                                   Starttid = new DateTime(2001, 1, 1, 1, 1, 1),
                                   Tidsmängd = new TimeSpan(1, 0, 0)
                               },
                           new AktivPeriod
                               {
                                   Användarnamn = "Person B",
                                   Starttid = new DateTime(2001, 1, 1, 2, 1, 1),
                                   Tidsmängd = new TimeSpan(2, 0, 0)
                               },
                           new AktivPeriod
                               {
                                   Användarnamn = "Person C",
                                   Starttid = new DateTime(2001, 1, 1, 3, 1, 1),
                                   Tidsmängd = new TimeSpan(3, 0, 0)
                               }
                       };
        }
    }

    [TestClass]
    public class NärViVillVisaGränssnittet : GivetAttDetFinnsData
    {
        public NärViVillVisaGränssnittet()
        {
            Presentatör.VisaGränssnitt();
        }

        [TestMethod]
        public void SåVisasGränssnittet()
        {
            Vy.Received(1).VisaGränssnitt();
        }

        [TestMethod]
        public void SåFårGränssnittetRättData()
        {
            Vy.Received(1).VisaData(Arg.Any<IList<AktivPeriod>>());
        }
    }

    [TestClass]
    public class NärViVisarGränssnittetOchDetInteFinnsData : GivetEnGanttPresentatör
    {
        public NärViVisarGränssnittetOchDetInteFinnsData()
        {
            Presentatör.VisaGränssnitt();
        }

        [TestMethod]
        public void SåVisasGränssnittet()
        {
            Vy.Received(1).VisaGränssnitt();
        }

        [TestMethod]
        public void SåFårVisasEnVarning()
        {
            Vy.Received(1).VisaDataSaknas();
        }
    }
}