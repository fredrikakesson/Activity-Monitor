using ActivityMonitor2.GUI.UserControls.Ganttdiagram;

namespace ActivityMonitor2.GUI.Formular
{
    partial class Spektrumformulär
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
            this.spektrumdiagram1 = new ActivityMonitor2.GUI.UserControls.Spektrumdiagram.Spektrumdiagram();
            this.SuspendLayout();
            // 
            // spektrumdiagram1
            // 
            this.spektrumdiagram1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.spektrumdiagram1.Location = new System.Drawing.Point(12, 12);
            this.spektrumdiagram1.Name = "spektrumdiagram1";
            this.spektrumdiagram1.Size = new System.Drawing.Size(508, 347);
            this.spektrumdiagram1.TabIndex = 0;
            this.spektrumdiagram1.VisaVarningDataSaknas = false;
            // 
            // Spektrumformulär
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 371);
            this.Controls.Add(this.spektrumdiagram1);
            this.Name = "Spektrumformulär";
            this.Text = "Spektrum över senaste månaden";
            this.ResumeLayout(false);

        }

        #endregion

        private UserControls.Spektrumdiagram.Spektrumdiagram spektrumdiagram1;



    }
}