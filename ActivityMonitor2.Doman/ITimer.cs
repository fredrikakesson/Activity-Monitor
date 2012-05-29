using System;

namespace ActivityMonitor2.Doman
{
    public interface ITimer
    {
        event EventHandler Tick;
    }
}