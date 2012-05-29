using System.Windows.Forms;

namespace ActivityMonitor2.GUI.UserControls.Ganttdiagram
{
    public partial class GanttBar : UserControl
    {
        public GanttBar(int längd, string text, string mouseOver)
        {
            InitializeComponent();
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            pictureBox1.Width = längd;
            label1.Left = pictureBox1.Left + pictureBox1.Width;
            label1.Text = text;
            var tt = new ToolTip();
            tt.SetToolTip(pictureBox1, mouseOver);
            Width = label1.Left + label1.Width;
        }
    }
}