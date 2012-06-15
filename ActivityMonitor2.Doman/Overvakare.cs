using ActivityMonitor2.Doman.Entiteter;
using System;

namespace ActivityMonitor2.Doman
{
    public class Övervakare : IDisposable
    {
        private readonly IAktivitetsdetektor _detektor;
        private readonly ILagring _lagring;
        private AktivPeriod _period;

        public Övervakare(IAktivitetsdetektor detektor, ILagring lagring)
        {
            _detektor = detektor;
            _lagring = lagring;

            _detektor.AktivitetUpptäckt += (s, e) => PåbörjaNyAktivitet();
            _detektor.InaktivitetUpptäckt += (s, e) => AvslutaPågåendeAktivitet();
        }

        private void PåbörjaNyAktivitet()
        {
            _period = new AktivPeriod { Starttid = SystemTime.Now(), Användarnamn = _detektor.NuvarandeAnvändare };
        }

        private void AvslutaPågåendeAktivitet()
        {
            if (_period == null) return;
            _period.Tidsmängd = SystemTime.Now().Subtract(_period.Starttid);
            _lagring.Spara(_period);
            _period = null;
        }

        public void Dispose()
        {
            if (_period != null)
                if (_period.Tidsmängd.TotalSeconds.Equals(0))
                    AvslutaPågåendeAktivitet();
        }
    }
}