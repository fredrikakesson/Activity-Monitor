using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ActivityMonitor2.Doman.Entiteter;

namespace ActivityMonitor2.GUI.UserControls.Dagdiagram
{
    public partial class Diagramarea : UserControl
    {
        public Diagramarea()
        {
            InitializeComponent();
        }

        public void VisaData(IList<AktivPeriod> perioder)
        {
            panel1.Controls.Clear();

            var lista = new Dictionary<DateTime, double>();

            foreach (var dag in perioder.Select(o => o.Starttid.Date).Distinct())
            {
                var summeradDag = dag;
                lista.Add(dag,
                          perioder.Where(o => o.Starttid.Date.Equals(summeradDag)).Sum(o => o.Tidsmängd.TotalMinutes));
            }

            if (!lista.Any()) return;

            var max = lista.Max(o => o.Value);
            var xposition = 10;
            foreach (var d in lista)
            {
                var stapel = new Dagstapel
                                 {
                                     Height = panel1.Height - 20,
                                     Top = 10,
                                     Left = xposition
                                 };

                stapel.VisaUppgifter(d.Key, d.Value, max);
                xposition += stapel.Width;
                panel1.Controls.Add(stapel);
            }
        }
    }
}