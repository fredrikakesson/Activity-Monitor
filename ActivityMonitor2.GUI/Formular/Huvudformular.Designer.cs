namespace ActivityMonitor2.GUI.Formular
{
    partial class Huvudformulär
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.VisaGanttMeny = new System.Windows.Forms.ToolStripMenuItem();
            this.AvslutaApplikationMeny = new System.Windows.Forms.ToolStripMenuItem();
            this.VisaVeckoöversiktMeny = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.VisaGanttMeny,
            this.VisaVeckoöversiktMeny,
            this.toolStripMenuItem1,
            this.AvslutaApplikationMeny});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
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
            // 
            // Huvudformulär
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(260, 89);
            this.Name = "Huvudformulär";
            this.ShowInTaskbar = false;
            this.Text = "Huvudformular";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem VisaGanttMeny;
        private System.Windows.Forms.ToolStripMenuItem AvslutaApplikationMeny;
        private System.Windows.Forms.ToolStripMenuItem VisaVeckoöversiktMeny;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
    }
}