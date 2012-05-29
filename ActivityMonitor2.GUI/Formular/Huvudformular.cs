using System;
using System.Drawing;
using System.Windows.Forms;
using ActivityMonitor2.Doman;
using ActivityMonitor2.GUI.Formular.Vyer;

namespace ActivityMonitor2.GUI.Formular
{
    public partial class Huvudformulär : Form, IStartvy
    {
        private readonly IIkongenerator _ikongenerator;

        public Huvudformulär(IIkongenerator generator)
        {
            InitializeComponent();
            notifyIcon1.MouseClick +=
                (s, e) => { if (e.Button == MouseButtons.Left) VisaVeckoöversikt(this, new EventArgs()); };
            VisaGanttMeny.Click += (s, e) => VisaDagsöversikt(this, new EventArgs());
            VisaVeckoöversiktMeny.Click += (s, e) => VisaVeckoöversikt(this, new EventArgs());
            AvslutaApplikationMeny.Click += (s, e) =>
                                                {
                                                    notifyIcon1.Dispose();
                                                    Close();
                                                };
            _ikongenerator = generator;
        }

        #region IStartvy Members

        public event EventHandler VisaDagsöversikt;
       public event EventHandler VisaVeckoöversikt;
 
        public void VisaSomAktiv()
        {
            BackColor = Color.ForestGreen;
        }

        public void VisaSomInaktiv()
        {
            BackColor = Color.FromArgb(255, 254, 39, 0);
        }

        public void VisaGränssnittFörAnvändaren()
        {
            ShowDialog();
        }

        public void VisaAktivDel(double procent, bool användarenAktiv)
        {
            notifyIcon1.Icon = _ikongenerator.SkapaTrayikon((float) procent, användarenAktiv);
            notifyIcon1.Text = string.Format("{0:p} aktivitet från start idag", procent);
        }

        #endregion


    }
}