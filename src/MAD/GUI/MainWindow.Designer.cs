namespace MAD.GUI
{
    partial class MainWindow
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.panelTop = new System.Windows.Forms.Panel();
            this.pictureBoxTopPanel = new System.Windows.Forms.PictureBox();
            this.labelTopBoxTitle = new System.Windows.Forms.Label();
            this.panelSeperator = new System.Windows.Forms.Panel();
            this.listBoxNodes = new System.Windows.Forms.ListBox();
            this.buttonReload = new System.Windows.Forms.Button();
            this.toolTipNodeListing = new System.Windows.Forms.ToolTip(this.components);
            this.labelConfigStatusTitle = new System.Windows.Forms.Label();
            this.labelConfigStatus = new System.Windows.Forms.Label();
            this.labelLastReloadTimeTitle = new System.Windows.Forms.Label();
            this.labelLastReloadTime = new System.Windows.Forms.Label();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTopPanel)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panelTop.Controls.Add(this.pictureBoxTopPanel);
            this.panelTop.Controls.Add(this.labelTopBoxTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(884, 75);
            this.panelTop.TabIndex = 0;
            // 
            // pictureBoxTopPanel
            // 
            this.pictureBoxTopPanel.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxTopPanel.Image")));
            this.pictureBoxTopPanel.Location = new System.Drawing.Point(11, 9);
            this.pictureBoxTopPanel.Name = "pictureBoxTopPanel";
            this.pictureBoxTopPanel.Size = new System.Drawing.Size(64, 60);
            this.pictureBoxTopPanel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxTopPanel.TabIndex = 2;
            this.pictureBoxTopPanel.TabStop = false;
            // 
            // labelTopBoxTitle
            // 
            this.labelTopBoxTitle.AutoSize = true;
            this.labelTopBoxTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 35F);
            this.labelTopBoxTitle.ForeColor = System.Drawing.SystemColors.Control;
            this.labelTopBoxTitle.Location = new System.Drawing.Point(73, 12);
            this.labelTopBoxTitle.Name = "labelTopBoxTitle";
            this.labelTopBoxTitle.Size = new System.Drawing.Size(159, 54);
            this.labelTopBoxTitle.TabIndex = 1;
            this.labelTopBoxTitle.Text = "Nodes";
            // 
            // panelSeperator
            // 
            this.panelSeperator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panelSeperator.Location = new System.Drawing.Point(390, 75);
            this.panelSeperator.Name = "panelSeperator";
            this.panelSeperator.Size = new System.Drawing.Size(3, 488);
            this.panelSeperator.TabIndex = 1;
            // 
            // listBoxNodes
            // 
            this.listBoxNodes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBoxNodes.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F);
            this.listBoxNodes.FormattingEnabled = true;
            this.listBoxNodes.ItemHeight = 38;
            this.listBoxNodes.Location = new System.Drawing.Point(12, 127);
            this.listBoxNodes.Name = "listBoxNodes";
            this.listBoxNodes.Size = new System.Drawing.Size(363, 380);
            this.listBoxNodes.TabIndex = 2;
            this.toolTipNodeListing.SetToolTip(this.listBoxNodes, "Please choose one \r\nof the listed Nodes \r\nin order to get Information.");
            // 
            // buttonReload
            // 
            this.buttonReload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.buttonReload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonReload.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.buttonReload.ForeColor = System.Drawing.SystemColors.Control;
            this.buttonReload.Location = new System.Drawing.Point(11, 520);
            this.buttonReload.Name = "buttonReload";
            this.buttonReload.Size = new System.Drawing.Size(364, 30);
            this.buttonReload.TabIndex = 3;
            this.buttonReload.Text = "Reload";
            this.buttonReload.UseVisualStyleBackColor = false;
            this.buttonReload.Click += new System.EventHandler(this.buttonReload_Click);
            // 
            // toolTipNodeListing
            // 
            this.toolTipNodeListing.BackColor = System.Drawing.SystemColors.Control;
            this.toolTipNodeListing.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            // 
            // labelConfigStatusTitle
            // 
            this.labelConfigStatusTitle.AutoSize = true;
            this.labelConfigStatusTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.labelConfigStatusTitle.Location = new System.Drawing.Point(12, 82);
            this.labelConfigStatusTitle.Name = "labelConfigStatusTitle";
            this.labelConfigStatusTitle.Size = new System.Drawing.Size(111, 17);
            this.labelConfigStatusTitle.TabIndex = 4;
            this.labelConfigStatusTitle.Text = "Config-Status:";
            // 
            // labelConfigStatus
            // 
            this.labelConfigStatus.AutoSize = true;
            this.labelConfigStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.labelConfigStatus.Location = new System.Drawing.Point(108, 82);
            this.labelConfigStatus.Name = "labelConfigStatus";
            this.labelConfigStatus.Size = new System.Drawing.Size(0, 17);
            this.labelConfigStatus.TabIndex = 5;
            // 
            // labelLastReloadTimeTitle
            // 
            this.labelLastReloadTimeTitle.AutoSize = true;
            this.labelLastReloadTimeTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.labelLastReloadTimeTitle.Location = new System.Drawing.Point(12, 101);
            this.labelLastReloadTimeTitle.Name = "labelLastReloadTimeTitle";
            this.labelLastReloadTimeTitle.Size = new System.Drawing.Size(100, 17);
            this.labelLastReloadTimeTitle.TabIndex = 6;
            this.labelLastReloadTimeTitle.Text = "Last Reload:";
            // 
            // labelLastReloadTime
            // 
            this.labelLastReloadTime.AutoSize = true;
            this.labelLastReloadTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.labelLastReloadTime.Location = new System.Drawing.Point(97, 101);
            this.labelLastReloadTime.Name = "labelLastReloadTime";
            this.labelLastReloadTime.Size = new System.Drawing.Size(149, 17);
            this.labelLastReloadTime.TabIndex = 7;
            this.labelLastReloadTime.Text = "Please press \"Reload\"";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 562);
            this.Controls.Add(this.labelLastReloadTime);
            this.Controls.Add(this.labelLastReloadTimeTitle);
            this.Controls.Add(this.labelConfigStatus);
            this.Controls.Add(this.labelConfigStatusTitle);
            this.Controls.Add(this.buttonReload);
            this.Controls.Add(this.listBoxNodes);
            this.Controls.Add(this.panelSeperator);
            this.Controls.Add(this.panelTop);
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "BaseGUIrebulid";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTopPanel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label labelTopBoxTitle;
        private System.Windows.Forms.PictureBox pictureBoxTopPanel;
        private System.Windows.Forms.Panel panelSeperator;
        private System.Windows.Forms.ListBox listBoxNodes;
        private System.Windows.Forms.Button buttonReload;
        private System.Windows.Forms.ToolTip toolTipNodeListing;
        private System.Windows.Forms.Label labelConfigStatusTitle;
        private System.Windows.Forms.Label labelConfigStatus;
        private System.Windows.Forms.Label labelLastReloadTimeTitle;
        private System.Windows.Forms.Label labelLastReloadTime;
    }
}