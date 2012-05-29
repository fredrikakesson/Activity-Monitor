using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using ActivityMonitor2.Doman.Entiteter;

namespace ActivityMonitor2.Doman
{
    public class SqliteLagring : ILagring
    {
        private const string Tabellnamn = "AktivaPerioder";
        private readonly SQLiteConnection _conn;

        public SqliteLagring(string connectionString)
        {
            _conn = new SQLiteConnection(connectionString);
            if (!TabellenFinns)
                SkapaTabellen();
        }

        public bool TabellenFinns
        {
            get
            {
                SQLiteCommand cmd = _conn.CreateCommand();
                cmd.CommandText = string.Format("SELECT name FROM sqlite_master WHERE name='{0}'", Tabellnamn);
                if (_conn.State!=ConnectionState.Open)
                    _conn.Open();
                bool tabellenFinns = cmd.ExecuteReader().HasRows;
                _conn.Close();
                return tabellenFinns;
            }
        }

        #region ILagring Members

        public void Spara(AktivPeriod period)
        {
            SQLiteCommand cmd = _conn.CreateCommand();
            cmd.CommandText = string.Format("INSERT INTO {0} VALUES (null, @start, @mangd, @namn);",
                                            Tabellnamn);

            cmd.Parameters.Add(new SQLiteParameter("@start", period.Starttid));
            cmd.Parameters.Add(new SQLiteParameter("@mangd", period.Tidsmängd));
            cmd.Parameters.Add(new SQLiteParameter("@namn", period.Användarnamn));

            if (_conn.State != ConnectionState.Open)
                _conn.Open();
            cmd.ExecuteNonQuery();
            _conn.Close();
        }

        public IList<AktivPeriod> HämtaAllaFörEnVissDag(DateTime dag)
        {
            var perioder = HämtaAllaPerioder();
            return perioder.Where(o => o.Starttid.Date.Equals(dag.Date)).ToList();
        }

        public IList<AktivPeriod> HämtaSenasteVeckansData()
        {
            var perioder = HämtaAllaPerioder();
            return perioder.Where(o => o.Starttid.Date >= (SystemTime.Now().AddDays(-7))).ToList();
        }

        public IList<AktivPeriod> HämtaAllaPerioder()
        {
            var perioder = new List<AktivPeriod>();

            SQLiteCommand cmd = _conn.CreateCommand();
            cmd.CommandText = string.Format("SELECT start, mangd, anvandarnamn FROM {0} ORDER BY start", Tabellnamn);

            if (_conn.State != ConnectionState.Open)
                _conn.Open();
            SQLiteDataReader result = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (result.Read())
            {
                perioder.Add(
                    new AktivPeriod
                        {
                            Starttid = DateTime.Parse(result["start"].ToString()),
                            Tidsmängd = TimeSpan.Parse(result["mangd"].ToString()),
                            Användarnamn = result["anvandarnamn"].ToString()
                        }
                    );
            }
            result.Close();

            return perioder;
        }

        #endregion

        private void SkapaTabellen()
        {
            SQLiteCommand cmd = _conn.CreateCommand();
            cmd.CommandText = string.Format(
                "CREATE TABLE {0} (id INTEGER PRIMARY KEY ASC, start, mangd, anvandarnamn);", Tabellnamn);

            if (_conn.State != ConnectionState.Open)
                _conn.Open();
            cmd.ExecuteNonQuery();
            _conn.Close();
        }

    }
}