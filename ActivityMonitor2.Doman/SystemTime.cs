using System;

namespace ActivityMonitor2.Doman
{
    public static class SystemTime
    {
        public static Func<DateTime> Now = () => DateTime.Now;
    }
}