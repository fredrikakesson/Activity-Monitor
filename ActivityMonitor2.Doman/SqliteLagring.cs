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
        private readonly string _connStr;

        public SqliteLagring(string connectionString)
        {
            _connStr = connectionString;
            if (!TabellenFinns)
                SkapaTabellen();
        }

        public bool TabellenFinns
        {
            get
            {
                return ExecQuery(cmd =>
                                     {
                                         cmd.CommandText = string.Format("SELECT name FROM sqlite_master WHERE name='{0}'", Tabellnamn);
                                     }, reader => reader.HasRows);
            }
        }

        #region ILagring Members

        public void Spara(AktivPeriod period)
        {
            ExecNonQuery(cmd =>
                             {
                                 cmd.CommandText = string.Format("INSERT INTO {0} VALUES (null, @start, @mangd, @namn);",
                                                                 Tabellnamn);

                                 cmd.Parameters.Add(new SQLiteParameter("@start", period.Starttid));
                                 cmd.Parameters.Add(new SQLiteParameter("@mangd", period.Tidsmängd));
                                 cmd.Parameters.Add(new SQLiteParameter("@namn", period.Användarnamn));
                             });
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

            return ExecQuery(cmd =>
                                 {
                                     cmd.CommandText = string.Format("SELECT start, mangd, anvandarnamn FROM {0} ORDER BY start", Tabellnamn);
                                 },
                                 result =>
                                 {
                                     var perioder = new List<AktivPeriod>();
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
                                     return perioder;
                                 });
        }

        #endregion

        private void SkapaTabellen()
        {
            ExecNonQuery(cmd =>
                             {
                                 cmd.CommandText = string.Format(
                                     "CREATE TABLE {0} (id INTEGER PRIMARY KEY ASC, start, mangd, anvandarnamn);", Tabellnamn);
                             });
        }

        private void ExecNonQuery(Action<SQLiteCommand> configureCommand)
        {
            using (var conn = new SQLiteConnection(_connStr))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    configureCommand(cmd);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private T ExecQuery<T>(Action<SQLiteCommand> configureCommand, Func<SQLiteDataReader, T> getter)
        {
            using (var conn = new SQLiteConnection(_connStr))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    configureCommand(cmd);
                    using (var reader = cmd.ExecuteReader())
                    {
                        return getter(reader);
                    }
                }
            }
        }


        public IList<AktivPeriod> HämtaSenasteMånadensData()
        {
            var perioder = HämtaAllaPerioder();
            return perioder.Where(o => o.Starttid.Date >= (SystemTime.Now().AddMonths(-1))).ToList();
        }

    }
}