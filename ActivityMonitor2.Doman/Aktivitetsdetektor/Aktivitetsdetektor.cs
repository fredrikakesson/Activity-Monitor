using System;

namespace ActivityMonitor2.Doman.Aktivitetsdetektor
{
    public class Aktivitetsdetektor : IAktivitetsdetektor
    {
        private bool _aktivEventKastat;
        private bool _inaktivEventKastat;

        public Aktivitetsdetektor(ITimer timer, IAnvändaraktivitet användaraktivitet, IStrömspararkontroll strömspararkontroll)
        {
            timer.Tick += (s, e) =>
                               {
                                   if (användaraktivitet.ÄrAktiv())
                                   {
                                       if (!_aktivEventKastat)
                                       {
                                           AktivitetUpptäckt(this, new EventArgs());
                                           _aktivEventKastat = true;
                                           _inaktivEventKastat = false;
                                       }
                                   }
                                   else
                                   {
                                       if (!_inaktivEventKastat)
                                       {
                                           InaktivitetUpptäckt(this, new EventArgs());
                                           _inaktivEventKastat = true;
                                           _aktivEventKastat = false;
                                       }
                                   }
                               };

            strömspararkontroll.GårNerIStrömsparläge += (s, e) => InaktivitetUpptäckt(this, new EventArgs());
        }

        #region IAktivitetsdetektor Members

        public event EventHandler AktivitetUpptäckt;
        public event EventHandler InaktivitetUpptäckt;

        public string NuvarandeAnvändare
        {
            get { return Environment.UserName; }
        }

        #endregion
    }
}