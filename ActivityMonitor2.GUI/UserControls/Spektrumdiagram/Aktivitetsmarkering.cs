using System;
using System.Windows.Forms;
using ActivityMonitor2.Doman.ExtensionMethods;

namespace ActivityMonitor2.GUI.UserControls.Spektrumdiagram
{
    public partial class Aktivitetsmarkering : UserControl
    {
        public Aktivitetsmarkering()
        {
            InitializeComponent();
        }

        public Aktivitetsmarkering(Doman.Entiteter.AktivPeriod item)
        {
            InitializeComponent();
            var tt = new ToolTip();
            tt.SetToolTip(pictureBox1, 
                string.Format("{0:0} min, {1:t} - {2:t}",
                    item.Tidsmängd.TotalMinutes, 
                    item.Starttid, 
                    item.Starttid.Add(item.Tidsmängd)));
        }
    }
}