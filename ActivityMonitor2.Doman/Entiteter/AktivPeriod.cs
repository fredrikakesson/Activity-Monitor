using System;

namespace ActivityMonitor2.Doman.Entiteter
{
    public class AktivPeriod
    {
        public DateTime Starttid { get; set; }
        public TimeSpan Tidsm�ngd { get; set; }
        public string Anv�ndarnamn { get; set; }

        public override string ToString()
        {
            return Anv�ndarnamn;
        }
    }
}