﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ActivityMonitor2.Doman;
using ActivityMonitor2.Doman.Entiteter;
using ActivityMonitor2.GUI.Formular.Vyer;
using System.Linq;

namespace ActivityMonitor2.GUI.Formular
{
    public partial class Ganttformulär : Form, IGanttVy
    {
        private IList<AktivPeriod> _perioder;
        public Ganttformulär()
        {
            InitializeComponent();
            FormClosing += (s, e) =>
                               {
                                   Hide();
                                   e.Cancel = true;
                               };
            comboBox1.SelectedIndexChanged += (s, e) => UppdateraDiagram();
        }

        private void UppdateraDiagram()
        {
            ganttDiagram1.VisaPerioder(_perioder, DateTime.Parse((string)comboBox1.SelectedItem));
        }

        #region IGanttVy Members

        public void VisaGränssnitt()
        {
            Show();
        }

        public void VisaData(IList<AktivPeriod> perioder)
        {
            _perioder = perioder;
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(perioder.Select(o => o.Starttid.ToShortDateString()).Distinct().OrderByDescending(o => o).ToArray());

            var dataSaknas = (comboBox1.Items.Count == 0);
            ganttDiagram1.VisaVarningDataSaknas = dataSaknas;
            comboBox1.Enabled = !dataSaknas;

            if (!dataSaknas)
            {
                ganttDiagram1.VisaVarningDataSaknas = false;
                comboBox1.SelectedIndex = 0;
                ganttDiagram1.VisaPerioder(perioder, DateTime.Parse((string)comboBox1.SelectedItem));
            }
        }

        #endregion
    }
}