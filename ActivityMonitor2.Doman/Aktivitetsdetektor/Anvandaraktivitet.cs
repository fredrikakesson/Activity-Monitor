using System;
using System.Runtime.InteropServices;

namespace ActivityMonitor2.Doman.Aktivitetsdetektor
{
    public class Användaraktivitet : IAnvändaraktivitet
    {
        private readonly int _intervall;

        /// <param name="sekunder">Minsta antalet sekunders inaktivitet som kallas inaktivitet</param>
        public Användaraktivitet(int sekunder)
        {
            _intervall = sekunder;
        }

        #region IAnvändaraktivitet Members

        public bool ÄrAktiv()
        {
            int idleTid = 0;
            var lastInput = new Lastinputinfo();
            lastInput.cbSize = (UInt32) Marshal.SizeOf(lastInput);
            lastInput.dwTime = 0;
            int envTicks = Environment.TickCount;

            if (GetLastInputInfo(ref lastInput))
                idleTid = envTicks - (int) lastInput.dwTime;

            return (idleTid/1000) < _intervall;
        }

        #endregion

        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref Lastinputinfo plii);

        #region Nested type: Lastinputinfo

        [StructLayout(LayoutKind.Sequential)]
        private struct Lastinputinfo
        {
            // private static readonly int SizeOf = Marshal.SizeOf(typeof (Lastinputinfo)); // Körs ej enligt NCover. Funkar ändå?

            [MarshalAs(UnmanagedType.U4)] public UInt32 cbSize;

            [MarshalAs(UnmanagedType.U4)] public UInt32 dwTime;
        }

        #endregion
    }
}