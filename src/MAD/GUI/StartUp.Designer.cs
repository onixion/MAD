namespace MAD
{
    partial class StartUp
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showNodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jobsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jobsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.nodesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openWorkingDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.settingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.title_Nodes = new System.Windows.Forms.Label();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showNodesToolStripMenuItem,
            this.showLogToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.toolStripSeparator1,
            this.saveToolStripMenuItem,
            this.saveAllToolStripMenuItem,
            this.openWorkingDirectoryToolStripMenuItem,
            this.toolStripSeparator2,
            this.settingToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.ShowImageMargin = false;
            this.contextMenuStrip1.Size = new System.Drawing.Size(187, 192);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // showNodesToolStripMenuItem
            // 
            this.showNodesToolStripMenuItem.Name = "showNodesToolStripMenuItem";
            this.showNodesToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.showNodesToolStripMenuItem.Text = "Show Nodes";
            this.showNodesToolStripMenuItem.Click += new System.EventHandler(this.showNodesToolStripMenuItem_Click);
            // 
            // showLogToolStripMenuItem
            // 
            this.showLogToolStripMenuItem.Name = "showLogToolStripMenuItem";
            this.showLogToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.showLogToolStripMenuItem.Text = "Show Log";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.jobsToolStripMenuItem,
            this.nodesToolStripMenuItem});
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.loadToolStripMenuItem.Text = "Load";
            // 
            // jobsToolStripMenuItem
            // 
            this.jobsToolStripMenuItem.Name = "jobsToolStripMenuItem";
            this.jobsToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.jobsToolStripMenuItem.Text = "Jobs";
            // 
            // nodesToolStripMenuItem
            // 
            this.nodesToolStripMenuItem.Name = "nodesToolStripMenuItem";
            this.nodesToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.nodesToolStripMenuItem.Text = "Nodes";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(183, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.jobsToolStripMenuItem1,
            this.nodesToolStripMenuItem1});
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // jobsToolStripMenuItem1
            // 
            this.jobsToolStripMenuItem1.Name = "jobsToolStripMenuItem1";
            this.jobsToolStripMenuItem1.Size = new System.Drawing.Size(112, 22);
            this.jobsToolStripMenuItem1.Text = "Jobs";
            // 
            // nodesToolStripMenuItem1
            // 
            this.nodesToolStripMenuItem1.Name = "nodesToolStripMenuItem1";
            this.nodesToolStripMenuItem1.Size = new System.Drawing.Size(112, 22);
            this.nodesToolStripMenuItem1.Text = "Nodes";
            // 
            // saveAllToolStripMenuItem
            // 
            this.saveAllToolStripMenuItem.Name = "saveAllToolStripMenuItem";
            this.saveAllToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.saveAllToolStripMenuItem.Text = "Save All";
            // 
            // openWorkingDirectoryToolStripMenuItem
            // 
            this.openWorkingDirectoryToolStripMenuItem.Name = "openWorkingDirectoryToolStripMenuItem";
            this.openWorkingDirectoryToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.openWorkingDirectoryToolStripMenuItem.Text = "Open Working Directory";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(183, 6);
            // 
            // settingToolStripMenuItem
            // 
            this.settingToolStripMenuItem.Name = "settingToolStripMenuItem";
            this.settingToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.settingToolStripMenuItem.Text = "Settings";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel1.Controls.Add(this.title_Nodes);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(885, 70);
            this.panel1.TabIndex = 1;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // title_Nodes
            // 
            this.title_Nodes.AccessibleRole = System.Windows.Forms.AccessibleRole.TitleBar;
            this.title_Nodes.AutoSize = true;
            this.title_Nodes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.title_Nodes.Font = new System.Drawing.Font("Microsoft JhengHei UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title_Nodes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.title_Nodes.Location = new System.Drawing.Point(10, 20);
            this.title_Nodes.Name = "title_Nodes";
            this.title_Nodes.Size = new System.Drawing.Size(99, 35);
            this.title_Nodes.TabIndex = 0;
            this.title_Nodes.Text = "Nodes";
            this.title_Nodes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(61, 4);
            // 
            // StartUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(884, 562);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.panel1);
            this.Name = "StartUp";
            this.Text = "StartUp";
            this.Load += new System.EventHandler(this.StartUp_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem showNodesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jobsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nodesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jobsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem nodesToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openWorkingDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label title_Nodes;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}