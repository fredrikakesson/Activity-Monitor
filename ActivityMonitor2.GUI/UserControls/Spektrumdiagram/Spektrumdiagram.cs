using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ActivityMonitor2.Doman.Entiteter;

namespace ActivityMonitor2.GUI.UserControls.Spektrumdiagram
{
    public partial class Spektrumdiagram : UserControl
    {
        public Spektrumdiagram()
        {
            InitializeComponent();
        }

        public bool VisaVarningDataSaknas { get { return label1.Visible; } set { label1.Visible = value; } }

        public void VisaData(IList<AktivPeriod> perioder)
        {
            panel1.Controls.Clear();
            double pixlarPerDygn = 1000;// (double)panel1.Width;

            var förstaTimme = perioder.OrderBy(o => o.Starttid.Hour).FirstOrDefault().Starttid.Hour;
            var sistaTimme = perioder.OrderBy(o => o.Starttid.Hour).LastOrDefault().Starttid.Hour;

            var daghöjd = 30;
            var marginal = 10;
            var vänsterMarginal = 120;
            var minutlängd = (pixlarPerDygn - vänsterMarginal) / (sistaTimme - förstaTimme) / 60; // pixlarPerDygn / 24 / 60;
            var position = 0;
            for (int timme = förstaTimme; timme <= sistaTimme; timme++)
            {
                var timstreck = new TimMarkering(timme);
                timstreck.Top = 0;
                timstreck.Left = (int)(position * minutlängd * 60) + vänsterMarginal;
                panel1.Controls.Add(timstreck);
                position++;

            }

            var y = marginal + daghöjd;


            foreach (var period in perioder.OrderByDescending(o => o.Starttid).GroupBy(o => o.Starttid.Date).Select(o =>
                new
                {
                    Datum = o.Key,
                    Perioder = o
                }))
            {
                var daglabel = new Label();
                daglabel.Top = y;
                daglabel.Left = marginal;
                daglabel.Text = period.Datum.ToString("dddd, d MMM");
                panel1.Controls.Add(daglabel);

                foreach (var item in period.Perioder)
                {
                    var aktivitet = new Aktivitetsmarkering(item);
                    aktivitet.BackColor = System.Drawing.Color.Blue;
                    aktivitet.Top = y;
                    aktivitet.Height = daghöjd;
                    aktivitet.Width = (int)(minutlängd * item.Tidsmängd.TotalMinutes);
                    if (aktivitet.Width == 0) aktivitet.Width = 1;
                    aktivitet.Left = (int)(item.Starttid.Subtract(period.Datum).TotalMinutes * minutlängd) + vänsterMarginal - (int)(förstaTimme * 60 * minutlängd);
                    panel1.Controls.Add(aktivitet);

                }
                y += daghöjd + marginal;
            }
        }
    }
}