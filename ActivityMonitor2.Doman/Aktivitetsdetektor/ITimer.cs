using System;

namespace ActivityMonitor2.Doman.Aktivitetsdetektor
{
    public interface ITimer
    {
        event EventHandler Tick;
    }
}