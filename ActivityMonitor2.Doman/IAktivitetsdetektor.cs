using System;

namespace ActivityMonitor2.Doman
{
    public interface IAktivitetsdetektor
    {
        string NuvarandeAnvändare { get; }
        event EventHandler AktivitetUpptäckt;
        event EventHandler InaktivitetUpptäckt;
    }
}