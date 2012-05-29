using System;
using Microsoft.Win32;

namespace ActivityMonitor2.Doman.Aktivitetsdetektor
{
    public interface IStrömspararkontroll
    {
        event EventHandler GårNerIStrömsparläge;
        event EventHandler TillbaksFrånStrömsparläge;
    }

    public class Strömspararkontroll : IStrömspararkontroll
    {
        public event EventHandler GårNerIStrömsparläge;
        public event EventHandler TillbaksFrånStrömsparläge;

        public Strömspararkontroll()
        {
            SystemEvents.PowerModeChanged += (s, e) => KastaEvent(e.Mode);
        }

        private void KastaEvent(PowerModes powerMode)
        {
            switch (powerMode)
            {
                case PowerModes.Resume:
                    TillbaksFrånStrömsparläge(this, new EventArgs());
                    break;

                case PowerModes.Suspend:
                    GårNerIStrömsparläge(this, new EventArgs());
                    break;
            }
        }
    }
}
