using System;

namespace ActivityMonitor2.Doman.Entiteter
{
    public class AktivPeriod
    {
        public DateTime Starttid { get; set; }
        public TimeSpan Tidsmängd { get; set; }
        public string Användarnamn { get; set; }

        public override string ToString()
        {
            return Användarnamn;
        }
    }
}