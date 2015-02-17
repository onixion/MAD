namespace MAD.GUI
{
    partial class Info
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Info));
            this.panelTop = new System.Windows.Forms.Panel();
            this.buttonAbout = new System.Windows.Forms.Button();
            this.buttonCLI = new System.Windows.Forms.Button();
            this.pictureBoxTopPanel = new System.Windows.Forms.PictureBox();
            this.labelTopBoxTitle = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelTitleVersion = new System.Windows.Forms.Label();
            this.labelTitleDesignedBy = new System.Windows.Forms.Label();
            this.labelTitleFindOn = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.labelDesignedBy = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTopPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panelTop.Controls.Add(this.buttonAbout);
            this.panelTop.Controls.Add(this.buttonCLI);
            this.panelTop.Controls.Add(this.pictureBoxTopPanel);
            this.panelTop.Controls.Add(this.labelTopBoxTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(289, 77);
            this.panelTop.TabIndex = 1;
            // 
            // buttonAbout
            // 
            this.buttonAbout.BackColor = System.Drawing.SystemColors.Control;
            this.buttonAbout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAbout.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.buttonAbout.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.buttonAbout.Location = new System.Drawing.Point(672, 25);
            this.buttonAbout.Name = "buttonAbout";
            this.buttonAbout.Size = new System.Drawing.Size(62, 30);
            this.buttonAbout.TabIndex = 11;
            this.buttonAbout.Text = "About";
            this.buttonAbout.UseVisualStyleBackColor = false;
            // 
            // buttonCLI
            // 
            this.buttonCLI.BackColor = System.Drawing.SystemColors.Control;
            this.buttonCLI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCLI.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.buttonCLI.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.buttonCLI.Location = new System.Drawing.Point(740, 25);
            this.buttonCLI.Name = "buttonCLI";
            this.buttonCLI.Size = new System.Drawing.Size(132, 30);
            this.buttonCLI.TabIndex = 10;
            this.buttonCLI.Text = "CLI";
            this.buttonCLI.UseVisualStyleBackColor = false;
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
            this.labelTopBoxTitle.Size = new System.Drawing.Size(101, 54);
            this.labelTopBoxTitle.TabIndex = 1;
            this.labelTopBoxTitle.Text = "Info";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(11, 93);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(261, 120);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // labelTitleVersion
            // 
            this.labelTitleVersion.AutoSize = true;
            this.labelTitleVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.labelTitleVersion.Location = new System.Drawing.Point(3, 0);
            this.labelTitleVersion.Name = "labelTitleVersion";
            this.labelTitleVersion.Size = new System.Drawing.Size(68, 17);
            this.labelTitleVersion.TabIndex = 3;
            this.labelTitleVersion.Text = "Version:";
            // 
            // labelTitleDesignedBy
            // 
            this.labelTitleDesignedBy.AutoSize = true;
            this.labelTitleDesignedBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.labelTitleDesignedBy.Location = new System.Drawing.Point(3, 50);
            this.labelTitleDesignedBy.Name = "labelTitleDesignedBy";
            this.labelTitleDesignedBy.Size = new System.Drawing.Size(103, 17);
            this.labelTitleDesignedBy.TabIndex = 4;
            this.labelTitleDesignedBy.Text = "Designed by:";
            // 
            // labelTitleFindOn
            // 
            this.labelTitleFindOn.AutoSize = true;
            this.labelTitleFindOn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.labelTitleFindOn.Location = new System.Drawing.Point(3, 100);
            this.labelTitleFindOn.Name = "labelTitleFindOn";
            this.labelTitleFindOn.Size = new System.Drawing.Size(99, 17);
            this.labelTitleFindOn.TabIndex = 5;
            this.labelTitleFindOn.Text = "Sourcecode:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.linkLabel1, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.labelDesignedBy, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelVersion, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelTitleVersion, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelTitleFindOn, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.labelTitleDesignedBy, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 219);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.6675F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.6675F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.6675F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66583F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66583F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66583F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(260, 153);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // linkLabel1
            // 
            this.linkLabel1.ActiveLinkColor = System.Drawing.Color.Teal;
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabel1.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.linkLabel1.Location = new System.Drawing.Point(3, 125);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(90, 17);
            this.linkLabel1.TabIndex = 7;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "GitHub Repo";
            this.linkLabel1.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // labelDesignedBy
            // 
            this.labelDesignedBy.AutoSize = true;
            this.labelDesignedBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelDesignedBy.Location = new System.Drawing.Point(3, 75);
            this.labelDesignedBy.Name = "labelDesignedBy";
            this.labelDesignedBy.Size = new System.Drawing.Size(235, 15);
            this.labelDesignedBy.TabIndex = 8;
            this.labelDesignedBy.Text = "Alin P. | Daniel R. | Manpreet S. | Marko S.";
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.labelVersion.Location = new System.Drawing.Point(3, 25);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(86, 17);
            this.labelVersion.TabIndex = 7;
            this.labelVersion.Text = "labelVersion";
            // 
            // buttonOK
            // 
            this.buttonOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.buttonOK.ForeColor = System.Drawing.SystemColors.Control;
            this.buttonOK.Location = new System.Drawing.Point(12, 378);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(260, 31);
            this.buttonOK.TabIndex = 14;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = false;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // Info
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 419);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panelTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Info";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MAD Networkmonitoring | Info";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTopPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button buttonAbout;
        private System.Windows.Forms.Button buttonCLI;
        private System.Windows.Forms.PictureBox pictureBoxTopPanel;
        private System.Windows.Forms.Label labelTopBoxTitle;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelTitleVersion;
        private System.Windows.Forms.Label labelTitleDesignedBy;
        private System.Windows.Forms.Label labelTitleFindOn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label labelDesignedBy;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Button buttonOK;
    }
}