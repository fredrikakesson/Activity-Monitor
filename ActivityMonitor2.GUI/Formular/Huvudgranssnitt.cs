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
        private IIkongenerator _ikongenerator;

        public Huvudgränssnitt(IIkongenerator generator)
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

        private void Close()
        {
            Application.Exit();
        }

        public void VisaAktivDel(double procent, bool användarenÄrAktiv)
        {
            notifyIcon1.Icon = _ikongenerator.SkapaTrayikon((float)procent, användarenÄrAktiv);
            notifyIcon1.Text = string.Format("{0:p} aktivitet från start idag", procent);
        }

        private void InitializeComponent()
        {
            var container = new ControlContainer();

            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(container);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(container);
            this.VisaGanttMeny = new System.Windows.Forms.ToolStripMenuItem();
            this.AvslutaApplikationMeny = new System.Windows.Forms.ToolStripMenuItem();
            this.VisaVeckoöversiktMeny = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();

            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Visible = true;

            // contextMenuStrip1
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.VisaGanttMeny,
            this.VisaVeckoöversiktMeny,
            this.toolStripMenuItem1,
            this.AvslutaApplikationMeny});
            this.contextMenuStrip1.Size = new System.Drawing.Size(148, 76);
            // 
            // VisaGanttMeny
            // 
            this.VisaGanttMeny.Name = "VisaGanttMeny";
            this.VisaGanttMeny.Size = new System.Drawing.Size(147, 22);
            this.VisaGanttMeny.Text = "Ganttschema";
            // 
            // AvslutaApplikationMeny
            // 
            this.AvslutaApplikationMeny.Name = "AvslutaApplikationMeny";
            this.AvslutaApplikationMeny.Size = new System.Drawing.Size(147, 22);
            this.AvslutaApplikationMeny.Text = "Avsluta";
            // 
            // VisaVeckoöversiktMeny
            // 
            this.VisaVeckoöversiktMeny.Name = "VisaVeckoöversiktMeny";
            this.VisaVeckoöversiktMeny.Size = new System.Drawing.Size(147, 22);
            this.VisaVeckoöversiktMeny.Text = "Veckoöversikt";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(144, 6);
            this.contextMenuStrip1.ResumeLayout(false);
        }

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem VisaGanttMeny;
        private System.Windows.Forms.ToolStripMenuItem AvslutaApplikationMeny;
        private System.Windows.Forms.ToolStripMenuItem VisaVeckoöversiktMeny;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;

        class ControlContainer : System.ComponentModel.IContainer
        {
            public void Add(System.ComponentModel.IComponent component, string name) { }
            public void Add(System.ComponentModel.IComponent component) { }
            public System.ComponentModel.ComponentCollection Components { get { throw new NotImplementedException();  } }
            public void Remove(System.ComponentModel.IComponent component) { }
            public void Dispose() { }
        }

        public void VisaGränssnittFörAnvändaren()
        {
            throw new NotImplementedException();
        }
    }
}
