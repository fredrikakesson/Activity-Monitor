using System;
using System.Windows.Forms;
using ActivityMonitor2.Doman.ExtensionMethods;

namespace ActivityMonitor2.GUI.UserControls.Dagdiagram
{
    public partial class Dagstapel : UserControl
    {
        public Dagstapel()
        {
            InitializeComponent();
        }

        internal void VisaUppgifter(DateTime dateTime, double minuter, double maxMinuter)
        {
            var totalTid = new TimeSpan(0, 0, (int) minuter, 0);
            label2.Text = dateTime.ToShortDateString();
            label1.Text = totalTid.ToShortString();
            pictureBox1.Height = (int) ((Height - label1.Height - label2.Height)*(minuter/maxMinuter));
            pictureBox1.Top = label2.Top-pictureBox1.Height;
            label1.Top = pictureBox1.Top - label1.Height;
        }
    }
}