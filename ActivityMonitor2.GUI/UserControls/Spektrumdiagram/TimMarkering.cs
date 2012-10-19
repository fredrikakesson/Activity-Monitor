using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ActivityMonitor2.GUI.UserControls.Spektrumdiagram
{
    public partial class TimMarkering : UserControl
    {
        public TimMarkering(int timme)
        {
            InitializeComponent();

            label1.Text = string.Format("{0:00}:00", timme);
        }

        private void TimMarkering_Paint(object sender, PaintEventArgs e)
        {
            using (var graphics = this.CreateGraphics())
            {
                graphics.DrawLine(new Pen(new SolidBrush(Color.Silver)), 0, 0, 0, 15);
            }
        }
    }
}
