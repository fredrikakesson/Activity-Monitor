using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ActivityMonitor2.Doman.Entiteter;
using ActivityMonitor2.GUI.Formular.Vyer;
using System.Linq;

namespace ActivityMonitor2.GUI.Formular
{
    public partial class Spektrumformulär : Form, ISpektrumVy
    {
        private IList<AktivPeriod> _perioder;
        public Spektrumformulär()
        {
            InitializeComponent();
            FormClosing += (s, e) =>
                               {
                                   Hide();
                                   e.Cancel = true;
                               };
        }

        public void VisaGränssnitt()
        {
            Show();
        }

        public void VisaData(IList<AktivPeriod> perioder)
        {
            _perioder = perioder;
            spektrumdiagram1.VisaVarningDataSaknas = false;
            spektrumdiagram1.VisaData(perioder);
        }

        public void VisaDataSaknas()
        {
            spektrumdiagram1.VisaVarningDataSaknas = true;
        }
    }
}