using System.Collections.Generic;
using System.Windows.Forms;
using ActivityMonitor2.Doman.Entiteter;
using ActivityMonitor2.GUI.Formular.Vyer;
using System.Linq;

namespace ActivityMonitor2.GUI.Formular
{
    public partial class Veckoformulär : Form, IVeckoöversiktVy
    {
        private IList<AktivPeriod> _perioder;

        public Veckoformulär()
        {
            InitializeComponent();
            FormClosing += (s, e) =>
                {
                    Hide();
                    e.Cancel = true;
                };

            comboBox1.SelectedIndexChanged += (s, e) => UppdateraDiagram();

        }

        public void VisaGränssnitt()
        {
            Show();
        }

        public void VisaData(IList<AktivPeriod> perioder)
        {
            _perioder = perioder;
            comboBox1.Items.Clear();

            comboBox1.Items.AddRange(_perioder.Select(o => o.Användarnamn).Distinct().OrderBy(o => o).ToArray());
            comboBox1.SelectedIndex = 0;
            comboBox1.Enabled = true;

            UppdateraDiagram();
        }

        private void UppdateraDiagram()
        {
            diagramarea1.VisaVarningDataSaknas = false;
            diagramarea1.VisaData(_perioder.Where(o => o.Användarnamn.Equals(comboBox1.Text)).ToList());
        }


        public void VisaDataSaknas()
        {
            comboBox1.Enabled = false;
            diagramarea1.VisaVarningDataSaknas = true;
        }
    }
}
