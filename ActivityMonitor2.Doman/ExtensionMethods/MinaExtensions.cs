using System;
using System.Text;

namespace ActivityMonitor2.Doman.ExtensionMethods
{
    public static class MinaExtensions
    {
        public static string ToShortString(this TimeSpan ts)
        {
            var sb = new StringBuilder();
            if (ts.Hours != 0)
                sb.AppendFormat("{0}h", ts.Hours);

            if (ts.Minutes != 0 || sb.Length == 0)
            {
                if (sb.Length > 0)
                    sb.Append(" ");
                sb.AppendFormat("{0}m", ts.Minutes);
            }

            return sb.ToString();
        }
    }
}