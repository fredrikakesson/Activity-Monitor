using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using ActivityMonitor2.Doman;
using ActivityMonitor2.Doman.Entiteter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ActivityMonitor2.Tests.Lagring
{
    public class GivetEnDatabaskoppling
    {
        protected string Connectionstring;
        protected ILagring Lagring;

        public void SkapaFörutsättningar()
        {
            Connectionstring = @"Data Source=:memory:;Pooling=true;FailIfMissing=false;Version=3";
            Lagring = new SqliteLagring(Connectionstring);
        }
    }

    [TestClass]
    public class NärLagringsfunktionenStartas : GivetEnDatabaskoppling
    {
        private const string Tabellnamn = "AktivaPerioder";

        [TestInitialize]
        public new void SkapaFörutsättningar()
        {
            base.SkapaFörutsättningar();
        }

        [TestMethod]
        public void SåSkapasTabellen()
        {
            var c = new SQLiteConnection(Connectionstring);
            c.Open();
            SQLiteCommand cmd = c.CreateCommand();
            cmd.CommandText = string.Format("SELECT name FROM sqlite_master WHERE name='{0}'", Tabellnamn);
            bool tabellenFinns = cmd.ExecuteReader().HasRows;
            c.Close();

            Assert.IsTrue(tabellenFinns);
        }
    }

    [TestClass]
    public class NärEnPeriodSparas : GivetEnDatabaskoppling
    {
        private const string Tabellnamn = "AktivaPerioder";
        private AktivPeriod _period;
        private SQLiteDataReader _rader;

        [TestInitialize]
        public new void SkapaFörutsättningar()
        {
            base.SkapaFörutsättningar();

            _period = new AktivPeriod
                          {
                              Starttid = new DateTime(2011, 11, 11),
                              Tidsmängd = new TimeSpan(11, 11, 11),
                              Användarnamn = "Test 11"
                          };
            Lagring.Spara(_period);

            LäsInDatat();
        }

        private void LäsInDatat()
        {
            var c = new SQLiteConnection(Connectionstring);
            c.Open();
            SQLiteCommand cmd = c.CreateCommand();
            cmd.CommandText = string.Format("SELECT * from {0} ORDER BY Id DESC", Tabellnamn);
            _rader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        [TestMethod]
        public void SåFinnsDataIDatabasen()
        {
            Assert.IsTrue(_rader.HasRows);
        }

        [TestMethod]
        public void SåMotsvararRadenPerioden()
        {
            Assert.AreEqual(_period.Starttid, DateTime.Parse(_rader["start"].ToString()));
            Assert.AreEqual(_period.Tidsmängd, TimeSpan.Parse(_rader["mangd"].ToString()));
            Assert.AreEqual(_period.Användarnamn, _rader["anvandarnamn"]);
        }

        [TestMethod, Timeout(1500)]
        public void SåHarEttIdGenererats()
        {
            Assert.IsTrue((Int64)_rader["id"] > 0);
        }
    }

    public class GivetAttDetFinnsData : GivetEnDatabaskoppling
    {
        public new void SkapaFörutsättningar()
        {
            base.SkapaFörutsättningar();

            Lagring.Spara(new AktivPeriod
                              {
                                  Starttid = new DateTime(2001, 1, 1),
                                  Tidsmängd = new TimeSpan(1, 2, 3),
                                  Användarnamn = "Test A"
                              });
            Lagring.Spara(new AktivPeriod
                              {
                                  Starttid = new DateTime(2001, 1, 1),
                                  Tidsmängd = new TimeSpan(4, 5, 6),
                                  Användarnamn = "Test A"
                              });
            Lagring.Spara(new AktivPeriod
                              {
                                  Starttid = new DateTime(2002, 1, 1),
                                  Tidsmängd = new TimeSpan(7, 8, 9),
                                  Användarnamn = "Test B"
                              });
        }
    }

    [TestClass]
    public class NärDataHämtas : GivetAttDetFinnsData
    {
        private IList<AktivPeriod> _lista;

        [TestInitialize]
        public new void SkapaFörutsättningar()
        {
            base.SkapaFörutsättningar();

            _lista = Lagring.HämtaAllaFörEnVissDag(new DateTime(2001, 1, 1));
        }

        [TestMethod]
        public void SåFårViRättAntalTräffar()
        {
            Assert.AreEqual(2, _lista.Count);
        }
    }

    [TestClass]
    public class NärSenasteVeckansDataHämtas : GivetAttDetFinnsData
    {
        private IList<AktivPeriod> _lista;

        [TestInitialize]
        public new void SkapaFörutsättningar()
        {
            base.SkapaFörutsättningar();

            SystemTime.Now += () => new DateTime(2002, 1, 5);
            _lista = Lagring.HämtaSenasteVeckansData();
        }

        [TestMethod]
        public void SåFårViRättAntalTräffar()
        {
            Assert.AreEqual(1, _lista.Count);
        }
    }
}