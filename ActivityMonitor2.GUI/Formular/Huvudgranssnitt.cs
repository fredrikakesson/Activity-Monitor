using System;
using System.Windows.Forms;
using ActivityMonitor2.Doman;
using ActivityMonitor2.GUI.Formular.Vyer;

namespace ActivityMonitor2.GUI.Formular
{
    class Huvudgränssnitt : IStartvy
    {
        public event EventHandler VisaDagsöversikt;
        public event EventHandler VisaVeckoöversikt;
        public event EventHandler VisaSpektrum;
        private IIkongenerator _ikongenerator;
        private IApplikationskommandon _appkommandon;
        private NotifyIcon notifyIcon1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem VisaGanttMeny;
        private ToolStripMenuItem AvslutaApplikationMeny;
        private ToolStripMenuItem VisaVeckoöversiktMeny;
        private ToolStripMenuItem VisaSpektrumMeny;
        private ToolStripSeparator toolStripMenuItem1;

        public Huvudgränssnitt(IIkongenerator generator, IApplikationskommandon appkommandon)
        {
            InitializeComponent();
            notifyIcon1.MouseClick +=
                (s, e) => { if (e.Button == MouseButtons.Left) VisaVeckoöversikt(this, new EventArgs()); };
            VisaGanttMeny.Click += (s, e) => VisaDagsöversikt(this, new EventArgs());
            VisaSpektrumMeny.Click += (s, e) => VisaSpektrum(this, new EventArgs());
            VisaVeckoöversiktMeny.Click += (s, e) => VisaVeckoöversikt(this, new EventArgs());
            AvslutaApplikationMeny.Click += (s, e) =>
                                                {
                                                    notifyIcon1.Dispose();
                                                    _appkommandon.StängNerApplikationen();
                                                };
            _ikongenerator = generator;
            _appkommandon = appkommandon;
        }

        public void VisaAktivDel(double procent, bool användarenÄrAktiv)
        {
            notifyIcon1.Icon = _ikongenerator.SkapaTrayikon((float)procent, användarenÄrAktiv);
            notifyIcon1.Text = string.Format("{0:p} aktivitet från start idag", procent);
        }

        private void InitializeComponent()
        {
            var container = new ControlContainer();

            notifyIcon1 = new NotifyIcon(container);
            contextMenuStrip1 = new ContextMenuStrip(container);
            VisaGanttMeny = new ToolStripMenuItem("Ganttschema");
            VisaSpektrumMeny = new ToolStripMenuItem("Spektrumdiagram");
            AvslutaApplikationMeny = new ToolStripMenuItem("Avsluta");
            VisaVeckoöversiktMeny = new ToolStripMenuItem("Veckoöversikt");
            toolStripMenuItem1 = new ToolStripSeparator();

            notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            notifyIcon1.Visible = true;

            contextMenuStrip1.Items.AddRange(new ToolStripItem[] {
                VisaGanttMeny,
                VisaVeckoöversiktMeny,
                VisaSpektrumMeny,
                toolStripMenuItem1,
                AvslutaApplikationMeny});

            contextMenuStrip1.ResumeLayout(false);
        }

        class ControlContainer : System.ComponentModel.IContainer
        {
            public void Add(System.ComponentModel.IComponent component, string name) { }
            public void Add(System.ComponentModel.IComponent component) { }
            public System.ComponentModel.ComponentCollection Components { get { throw new NotImplementedException(); } }
            public void Remove(System.ComponentModel.IComponent component) { }
            public void Dispose() { }
        }

        public void VisaGränssnittFörAnvändaren()
        {
            throw new NotImplementedException();
        }
    }
}
