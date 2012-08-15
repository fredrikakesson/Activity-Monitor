using System;
using System.Collections.Generic;
using System.Linq;
using ActivityMonitor2.Doman;
using ActivityMonitor2.Doman.Entiteter;
using ActivityMonitor2.GUI.Formular.Vyer;

namespace ActivityMonitor2.GUI
{
    public class Presentatör
    {
        private readonly ILagring _lagring;
        private readonly IStartvy _startvy;
        private bool _ärAktiv;
        private DateTime _aktivStarttid;

        public Presentatör(IStartvy vy, IAktivitetsdetektor detektor, ILagring lagring, ITimer aktivDelTimer)
        {
            _startvy = vy;
            _lagring = lagring;

            detektor.AktivitetUpptäckt += (s, e) => NoteraAktivitet();
            detektor.InaktivitetUpptäckt += (s, e) => NoteraInaktivitet();
            vy.VisaDagsöversikt += (s, e) => VisaGanttschema(this, new EventArgs());
            vy.VisaVeckoöversikt += (s, e) => VisaVeckoöversikt(this, new EventArgs());
            aktivDelTimer.Tick += (s, e) => VisaAktivDel();

            VisaAktivDel();
        }

        private void NoteraAktivitet()
        {
            _ärAktiv = true;
            _aktivStarttid = SystemTime.Now();
        }

        private void VisaAktivDel()
        {
            // Testar att stänga av visningen vid inaktivitet för att 
            // lösa problem med samtidighet i databasen.
            if (_ärAktiv)
                _startvy.VisaAktivDel(BeräknaAktivDel(), _ärAktiv);
        }

        private void NoteraInaktivitet()
        {
            _ärAktiv = false;
        }

        private double BeräknaAktivDel()
        {
            IList<AktivPeriod> perioder = _lagring.HämtaAllaFörEnVissDag(SystemTime.Now());
            AktivPeriod förstaPerioden = perioder.OrderBy(o => o.Starttid).FirstOrDefault();

            var minuterFrånStartTillNu = förstaPerioden == null
                ? SystemTime.Now().Subtract(_aktivStarttid).TotalMinutes 
                : SystemTime.Now().Subtract(förstaPerioden.Starttid).TotalMinutes;

            double aktivaMinuter = perioder.Sum(o => o.Tidsmängd.TotalMinutes);

            if (_ärAktiv)
                aktivaMinuter += SystemTime.Now().Subtract(_aktivStarttid).TotalMinutes;

            if (aktivaMinuter > 0 && minuterFrånStartTillNu > 0)
                return aktivaMinuter / minuterFrånStartTillNu;

            return 0;
        }

        internal void VisaGränssnitt()
        {
            throw new NotImplementedException();
        }

        public event EventHandler VisaGanttschema;
        public event EventHandler VisaVeckoöversikt;
    }
}