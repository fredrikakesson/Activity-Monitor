using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ActivityMonitor2.Doman.Entiteter;
using ActivityMonitor2.Doman.ExtensionMethods;

namespace ActivityMonitor2.GUI.UserControls.Ganttdiagram
{
    public partial class GanttDiagram : UserControl
    {
        private const double PixlarPerTimme = 70;

        public GanttDiagram()
        {
            InitializeComponent();
        }

        public bool VisaVarningDataSaknas { get { return label1.Visible; } set { label1.Visible = value; } }

        private int TidTillPixlar(DateTime dateTime)
        {
            return (int)(PixlarPerTimme * dateTime.Hour + (PixlarPerTimme / 60) * dateTime.Minute);
        }

        internal void VisaPerioder(IList<AktivPeriod> perioder, DateTime dag)
        {
            panel1.Controls.Clear();

            var dagensPerioder = perioder.Where(o => o.Starttid.Date.Equals(dag.Date)).OrderBy(o => o.Starttid);

            if (!dagensPerioder.Any()) return;

            var överkant = 10;
            var förstaPerioden = dagensPerioder.Min(o => o.Starttid);
            var förskjutning = TidTillPixlar(förstaPerioden);
            foreach (var period in dagensPerioder)
            {
                var bar = new GanttBar(
                    (int)(period.Tidsmängd.TotalMinutes * (PixlarPerTimme / 60)),
                    string.Format("{1} - {0}", period.Användarnamn, period.Tidsmängd.ToShortString()),
                    string.Format("{0:t} - {1:t}", period.Starttid, period.Starttid.Add(period.Tidsmängd))) { Left = TidTillPixlar(period.Starttid) - förskjutning, Top = överkant + 5 };
                överkant = bar.Top + bar.Height;
                panel1.Controls.Add(bar);
            }
        }
    }
}