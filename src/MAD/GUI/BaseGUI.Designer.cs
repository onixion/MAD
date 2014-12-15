namespace MAD.GUI
{
    public partial class BaseGUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseGUI));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showNodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowNewStripSeperator = new System.Windows.Forms.ToolStripSeparator();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newJobToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadNodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadJobsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveNodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveJobsToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openWorkingDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenDirSettingsStringSperator = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SettingsExitStringSeperators = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.actionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startScanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectivityCheckToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manuallyCheckNodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.titlePanel = new System.Windows.Forms.Panel();
            this.title = new System.Windows.Forms.Label();
            this.titlePicBox = new System.Windows.Forms.PictureBox();
            this.menuStrip.SuspendLayout();
            this.titlePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.titlePicBox)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            resources.ApplyResources(this.menuStrip, "menuStrip");
            this.menuStrip.BackColor = System.Drawing.Color.DarkCyan;
            this.menuStrip.GripMargin = new System.Windows.Forms.Padding(2);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.actionsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            //this.menuStrip.Font = BaseGUI.DEFAULT_FONT;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripMenuItem,
            this.ShowNewStripSeperator,
            this.newToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAllToolStripMenuItem,
            this.openWorkingDirectoryToolStripMenuItem,
            this.OpenDirSettingsStringSperator,
            this.settingsToolStripMenuItem,
            this.SettingsExitStringSeperators,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showNodesToolStripMenuItem,
            this.showLogToolStripMenuItem});
            resources.ApplyResources(this.showToolStripMenuItem, "showToolStripMenuItem");
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            // 
            // showNodesToolStripMenuItem
            // 
            resources.ApplyResources(this.showNodesToolStripMenuItem, "showNodesToolStripMenuItem");
            this.showNodesToolStripMenuItem.Name = "showNodesToolStripMenuItem";
            // 
            // showLogToolStripMenuItem
            // 
            resources.ApplyResources(this.showLogToolStripMenuItem, "showLogToolStripMenuItem");
            this.showLogToolStripMenuItem.Name = "showLogToolStripMenuItem";
            // 
            // ShowNewStripSeperator
            // 
            this.ShowNewStripSeperator.Name = "ShowNewStripSeperator";
            resources.ApplyResources(this.ShowNewStripSeperator, "ShowNewStripSeperator");
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newNodeToolStripMenuItem,
            this.newJobToolStripMenuItem});
            resources.ApplyResources(this.newToolStripMenuItem, "newToolStripMenuItem");
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            // 
            // newNodeToolStripMenuItem
            // 
            resources.ApplyResources(this.newNodeToolStripMenuItem, "newNodeToolStripMenuItem");
            this.newNodeToolStripMenuItem.Name = "newNodeToolStripMenuItem";
            // 
            // newJobToolStripMenuItem
            // 
            resources.ApplyResources(this.newJobToolStripMenuItem, "newJobToolStripMenuItem");
            this.newJobToolStripMenuItem.Name = "newJobToolStripMenuItem";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadNodesToolStripMenuItem,
            this.loadJobsToolStripMenuItem});
            resources.ApplyResources(this.loadToolStripMenuItem, "loadToolStripMenuItem");
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            // 
            // loadNodesToolStripMenuItem
            // 
            resources.ApplyResources(this.loadNodesToolStripMenuItem, "loadNodesToolStripMenuItem");
            this.loadNodesToolStripMenuItem.Name = "loadNodesToolStripMenuItem";
            // 
            // loadJobsToolStripMenuItem
            // 
            resources.ApplyResources(this.loadJobsToolStripMenuItem, "loadJobsToolStripMenuItem");
            this.loadJobsToolStripMenuItem.Name = "loadJobsToolStripMenuItem";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveNodesToolStripMenuItem,
            this.saveJobsToolStripMenuItem3});
            resources.ApplyResources(this.saveToolStripMenuItem, "saveToolStripMenuItem");
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            // 
            // saveNodesToolStripMenuItem
            // 
            resources.ApplyResources(this.saveNodesToolStripMenuItem, "saveNodesToolStripMenuItem");
            this.saveNodesToolStripMenuItem.Name = "saveNodesToolStripMenuItem";
            // 
            // saveJobsToolStripMenuItem3
            // 
            resources.ApplyResources(this.saveJobsToolStripMenuItem3, "saveJobsToolStripMenuItem3");
            this.saveJobsToolStripMenuItem3.Name = "saveJobsToolStripMenuItem3";
            // 
            // saveAllToolStripMenuItem
            // 
            resources.ApplyResources(this.saveAllToolStripMenuItem, "saveAllToolStripMenuItem");
            this.saveAllToolStripMenuItem.Name = "saveAllToolStripMenuItem";
            // 
            // openWorkingDirectoryToolStripMenuItem
            // 
            resources.ApplyResources(this.openWorkingDirectoryToolStripMenuItem, "openWorkingDirectoryToolStripMenuItem");
            this.openWorkingDirectoryToolStripMenuItem.Name = "openWorkingDirectoryToolStripMenuItem";
            // 
            // OpenDirSettingsStringSperator
            // 
            this.OpenDirSettingsStringSperator.Name = "OpenDirSettingsStringSperator";
            resources.ApplyResources(this.OpenDirSettingsStringSperator, "OpenDirSettingsStringSperator");
            // 
            // settingsToolStripMenuItem
            // 
            resources.ApplyResources(this.settingsToolStripMenuItem, "settingsToolStripMenuItem");
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            // 
            // SettingsExitStringSeperators
            // 
            this.SettingsExitStringSeperators.Name = "SettingsExitStringSeperators";
            resources.ApplyResources(this.SettingsExitStringSeperators, "SettingsExitStringSeperators");
            // 
            // exitToolStripMenuItem
            // 
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem1_Click);
            // 
            // actionsToolStripMenuItem
            // 
            this.actionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startScanToolStripMenuItem,
            this.connectivityCheckToolStripMenuItem,
            this.manuallyCheckNodesToolStripMenuItem});
            this.actionsToolStripMenuItem.Name = "actionsToolStripMenuItem";
            resources.ApplyResources(this.actionsToolStripMenuItem, "actionsToolStripMenuItem");
            // 
            // startScanToolStripMenuItem
            // 
            resources.ApplyResources(this.startScanToolStripMenuItem, "startScanToolStripMenuItem");
            this.startScanToolStripMenuItem.Name = "startScanToolStripMenuItem";
            this.startScanToolStripMenuItem.Click += new System.EventHandler(this.startScanToolStripMenuItem_Click);
            // 
            // connectivityCheckToolStripMenuItem
            // 
            resources.ApplyResources(this.connectivityCheckToolStripMenuItem, "connectivityCheckToolStripMenuItem");
            this.connectivityCheckToolStripMenuItem.Name = "connectivityCheckToolStripMenuItem";
            // 
            // manuallyCheckNodesToolStripMenuItem
            // 
            resources.ApplyResources(this.manuallyCheckNodesToolStripMenuItem, "manuallyCheckNodesToolStripMenuItem");
            this.manuallyCheckNodesToolStripMenuItem.Name = "manuallyCheckNodesToolStripMenuItem";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem,
            this.infoToolStripMenuItem});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
            // 
            // helpToolStripMenuItem
            // 
            resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            // 
            // infoToolStripMenuItem
            // 
            resources.ApplyResources(this.infoToolStripMenuItem, "infoToolStripMenuItem");
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            // 
            // titlePanel
            // 
            this.titlePanel.BackColor = System.Drawing.Color.Cyan;
            this.titlePanel.Controls.Add(this.title);
            this.titlePanel.Controls.Add(this.titlePicBox);
            resources.ApplyResources(this.titlePanel, "titlePanel");
            this.titlePanel.Name = "titlePanel";
            // 
            // title
            // 
            this.title.AccessibleRole = System.Windows.Forms.AccessibleRole.TitleBar;
            resources.ApplyResources(this.title, "title");
            this.title.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.title.Name = "title";
            // 
            // titlePicBox
            // 
            resources.ApplyResources(this.titlePicBox, "titlePicBox");
            this.titlePicBox.Name = "titlePicBox";
            this.titlePicBox.TabStop = false;
            // 
            // BaseGUI
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Controls.Add(this.titlePanel);
            this.Controls.Add(this.menuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "BaseGUI";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.titlePanel.ResumeLayout(false);
            this.titlePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.titlePicBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.MenuStrip menuStrip;
        protected System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        protected System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
        protected System.Windows.Forms.ToolStripSeparator ShowNewStripSeperator;
        protected System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        protected System.Windows.Forms.ToolStripMenuItem loadJobsToolStripMenuItem;
        protected System.Windows.Forms.ToolStripMenuItem loadNodesToolStripMenuItem;
        protected System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        protected System.Windows.Forms.ToolStripMenuItem saveAllToolStripMenuItem;
        protected System.Windows.Forms.ToolStripMenuItem openWorkingDirectoryToolStripMenuItem;
        protected System.Windows.Forms.ToolStripSeparator OpenDirSettingsStringSperator;
        protected System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        protected System.Windows.Forms.ToolStripSeparator SettingsExitStringSeperators;
        protected System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        protected System.Windows.Forms.ToolStripMenuItem saveJobsToolStripMenuItem3;
        protected System.Windows.Forms.ToolStripMenuItem saveNodesToolStripMenuItem;
        protected System.Windows.Forms.ToolStripMenuItem actionsToolStripMenuItem;
        protected System.Windows.Forms.ToolStripMenuItem startScanToolStripMenuItem;
        protected System.Windows.Forms.ToolStripMenuItem connectivityCheckToolStripMenuItem;
        protected System.Windows.Forms.ToolStripMenuItem manuallyCheckNodesToolStripMenuItem;
        protected System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        protected System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        protected System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
        protected System.Windows.Forms.ToolStripMenuItem showNodesToolStripMenuItem;
        protected System.Windows.Forms.ToolStripMenuItem showLogToolStripMenuItem;
        protected System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        protected System.Windows.Forms.ToolStripMenuItem newNodeToolStripMenuItem;
        protected System.Windows.Forms.ToolStripMenuItem newJobToolStripMenuItem;
        protected System.Windows.Forms.Panel titlePanel;
        protected System.Windows.Forms.Label title;
        protected System.Windows.Forms.PictureBox titlePicBox;
    }
}