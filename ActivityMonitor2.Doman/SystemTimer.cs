using System;
using System.Timers;

namespace ActivityMonitor2.Doman
{
    public class SystemTimer : ITimer
    {
        private readonly Timer _timer = new Timer();

        /// <param name="sekunder">Sekunder mellan tick</param>
        public SystemTimer(int sekunder)
        {
            _timer.Interval = sekunder*1000;
            _timer.Elapsed += (s, e) => Tick(this, new EventArgs());
            _timer.Enabled = true;
        }

        #region ITimer Members

        public event EventHandler Tick;

        #endregion
    }
}