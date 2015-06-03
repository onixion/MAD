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
            this.buttonScan = new System.Windows.Forms.Button();
            this.buttonInfo = new System.Windows.Forms.Button();
            this.buttonCLI = new System.Windows.Forms.Button();
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
            this.panelInformation = new System.Windows.Forms.Panel();
            this.listBoxInfoJobs = new System.Windows.Forms.ListBox();
            this.pictureBoxLightRedoff = new System.Windows.Forms.PictureBox();
            this.pictureBoxLightRedOn = new System.Windows.Forms.PictureBox();
            this.pictureBoxLightGreenOn = new System.Windows.Forms.PictureBox();
            this.pictureBoxLightGreenOff = new System.Windows.Forms.PictureBox();
            this.panelSeperatorInformation = new System.Windows.Forms.Panel();
            this.tableLayoutPanelInfoJobs = new System.Windows.Forms.TableLayoutPanel();
            this.labelInfoJobsOutstate = new System.Windows.Forms.Label();
            this.labelInfoJobsType = new System.Windows.Forms.Label();
            this.labelInfoJobsTitleGUID = new System.Windows.Forms.Label();
            this.labelInfoJobsTitleID = new System.Windows.Forms.Label();
            this.labelInfoJobsTitleType = new System.Windows.Forms.Label();
            this.labelInfoJobsGUID = new System.Windows.Forms.Label();
            this.labelInfoJobsTitleOutstate = new System.Windows.Forms.Label();
            this.labelInfoJobsID = new System.Windows.Forms.Label();
            this.labelInfoJobsTitle = new System.Windows.Forms.Label();
            this.tableLayoutPanelInfoGeneral = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxInfoJobsMemo2 = new System.Windows.Forms.TextBox();
            this.labelInfoTitleMac = new System.Windows.Forms.Label();
            this.labelInfoJobsTitleMemo2 = new System.Windows.Forms.Label();
            this.labelInfoState = new System.Windows.Forms.Label();
            this.labelInfoName = new System.Windows.Forms.Label();
            this.labelInfoIP = new System.Windows.Forms.Label();
            this.labelInfoID = new System.Windows.Forms.Label();
            this.labelInfoMAC = new System.Windows.Forms.Label();
            this.labelInfoTitleID = new System.Windows.Forms.Label();
            this.labelInfoGUID = new System.Windows.Forms.Label();
            this.labelInfoTitleGUID = new System.Windows.Forms.Label();
            this.labelInfoTitleIP = new System.Windows.Forms.Label();
            this.labelInfoTitleName = new System.Windows.Forms.Label();
            this.labelInfoJobsTitleMemo1 = new System.Windows.Forms.Label();
            this.labelInfoTitleState = new System.Windows.Forms.Label();
            this.textBoxInfoJobsMemo1 = new System.Windows.Forms.TextBox();
            this.labelInformationTitle = new System.Windows.Forms.Label();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTopPanel)).BeginInit();
            this.panelInformation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLightRedoff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLightRedOn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLightGreenOn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLightGreenOff)).BeginInit();
            this.tableLayoutPanelInfoJobs.SuspendLayout();
            this.tableLayoutPanelInfoGeneral.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panelTop.Controls.Add(this.buttonScan);
            this.panelTop.Controls.Add(this.buttonInfo);
            this.panelTop.Controls.Add(this.buttonCLI);
            this.panelTop.Controls.Add(this.pictureBoxTopPanel);
            this.panelTop.Controls.Add(this.labelTopBoxTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(884, 75);
            this.panelTop.TabIndex = 0;
            // 
            // buttonScan
            // 
            this.buttonScan.BackColor = System.Drawing.SystemColors.Control;
            this.buttonScan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonScan.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.buttonScan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.buttonScan.Location = new System.Drawing.Point(676, 25);
            this.buttonScan.Name = "buttonScan";
            this.buttonScan.Size = new System.Drawing.Size(95, 30);
            this.buttonScan.TabIndex = 12;
            this.buttonScan.Text = "Scan";
            this.buttonScan.UseVisualStyleBackColor = false;
            this.buttonScan.Click += new System.EventHandler(this.buttonScan_Click);
            // 
            // buttonInfo
            // 
            this.buttonInfo.BackColor = System.Drawing.SystemColors.Control;
            this.buttonInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.buttonInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.buttonInfo.Location = new System.Drawing.Point(575, 25);
            this.buttonInfo.Name = "buttonInfo";
            this.buttonInfo.Size = new System.Drawing.Size(95, 30);
            this.buttonInfo.TabIndex = 11;
            this.buttonInfo.Text = "Info";
            this.buttonInfo.UseVisualStyleBackColor = false;
            this.buttonInfo.Click += new System.EventHandler(this.buttonInfo_Click);
            // 
            // buttonCLI
            // 
            this.buttonCLI.BackColor = System.Drawing.SystemColors.Control;
            this.buttonCLI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCLI.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.buttonCLI.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.buttonCLI.Location = new System.Drawing.Point(777, 25);
            this.buttonCLI.Name = "buttonCLI";
            this.buttonCLI.Size = new System.Drawing.Size(95, 30);
            this.buttonCLI.TabIndex = 10;
            this.buttonCLI.Text = "CLI";
            this.buttonCLI.UseVisualStyleBackColor = false;
            this.buttonCLI.Click += new System.EventHandler(this.buttonCLI_Click);
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
            this.panelSeperator.Location = new System.Drawing.Point(350, 75);
            this.panelSeperator.Name = "panelSeperator";
            this.panelSeperator.Size = new System.Drawing.Size(3, 488);
            this.panelSeperator.TabIndex = 1;
            // 
            // listBoxNodes
            // 
            this.listBoxNodes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxNodes.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F);
            this.listBoxNodes.FormattingEnabled = true;
            this.listBoxNodes.ItemHeight = 38;
            this.listBoxNodes.Location = new System.Drawing.Point(12, 127);
            this.listBoxNodes.Name = "listBoxNodes";
            this.listBoxNodes.Size = new System.Drawing.Size(321, 382);
            this.listBoxNodes.TabIndex = 2;
            this.toolTipNodeListing.SetToolTip(this.listBoxNodes, "Please choose one \r\nof the listed Nodes \r\nin order to get Information.");
            this.listBoxNodes.SelectedIndexChanged += new System.EventHandler(this.listBoxNodes_SelectedIndexChanged);
            // 
            // buttonReload
            // 
            this.buttonReload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.buttonReload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonReload.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.buttonReload.ForeColor = System.Drawing.SystemColors.Control;
            this.buttonReload.Location = new System.Drawing.Point(11, 520);
            this.buttonReload.Name = "buttonReload";
            this.buttonReload.Size = new System.Drawing.Size(322, 30);
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
            // panelInformation
            // 
            this.panelInformation.Controls.Add(this.listBoxInfoJobs);
            this.panelInformation.Controls.Add(this.pictureBoxLightRedoff);
            this.panelInformation.Controls.Add(this.pictureBoxLightRedOn);
            this.panelInformation.Controls.Add(this.pictureBoxLightGreenOn);
            this.panelInformation.Controls.Add(this.pictureBoxLightGreenOff);
            this.panelInformation.Controls.Add(this.panelSeperatorInformation);
            this.panelInformation.Controls.Add(this.tableLayoutPanelInfoJobs);
            this.panelInformation.Controls.Add(this.labelInfoJobsTitle);
            this.panelInformation.Controls.Add(this.tableLayoutPanelInfoGeneral);
            this.panelInformation.Controls.Add(this.labelInformationTitle);
            this.panelInformation.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelInformation.Location = new System.Drawing.Point(353, 75);
            this.panelInformation.Name = "panelInformation";
            this.panelInformation.Size = new System.Drawing.Size(531, 487);
            this.panelInformation.TabIndex = 9;
            // 
            // listBoxInfoJobs
            // 
            this.listBoxInfoJobs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxInfoJobs.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.listBoxInfoJobs.FormattingEnabled = true;
            this.listBoxInfoJobs.ItemHeight = 18;
            this.listBoxInfoJobs.Location = new System.Drawing.Point(22, 294);
            this.listBoxInfoJobs.Margin = new System.Windows.Forms.Padding(0);
            this.listBoxInfoJobs.Name = "listBoxInfoJobs";
            this.listBoxInfoJobs.Size = new System.Drawing.Size(124, 164);
            this.listBoxInfoJobs.TabIndex = 17;
            this.listBoxInfoJobs.SelectedIndexChanged += new System.EventHandler(this.listBoxInfoJobs_SelectedIndexChanged);
            // 
            // pictureBoxLightRedoff
            // 
            this.pictureBoxLightRedoff.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxLightRedoff.Image")));
            this.pictureBoxLightRedoff.Location = new System.Drawing.Point(463, 122);
            this.pictureBoxLightRedoff.Name = "pictureBoxLightRedoff";
            this.pictureBoxLightRedoff.Size = new System.Drawing.Size(56, 56);
            this.pictureBoxLightRedoff.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLightRedoff.TabIndex = 23;
            this.pictureBoxLightRedoff.TabStop = false;
            // 
            // pictureBoxLightRedOn
            // 
            this.pictureBoxLightRedOn.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxLightRedOn.Image")));
            this.pictureBoxLightRedOn.Location = new System.Drawing.Point(463, 122);
            this.pictureBoxLightRedOn.Name = "pictureBoxLightRedOn";
            this.pictureBoxLightRedOn.Size = new System.Drawing.Size(56, 56);
            this.pictureBoxLightRedOn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLightRedOn.TabIndex = 22;
            this.pictureBoxLightRedOn.TabStop = false;
            this.pictureBoxLightRedOn.Visible = false;
            // 
            // pictureBoxLightGreenOn
            // 
            this.pictureBoxLightGreenOn.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxLightGreenOn.Image")));
            this.pictureBoxLightGreenOn.Location = new System.Drawing.Point(463, 52);
            this.pictureBoxLightGreenOn.Name = "pictureBoxLightGreenOn";
            this.pictureBoxLightGreenOn.Size = new System.Drawing.Size(56, 56);
            this.pictureBoxLightGreenOn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLightGreenOn.TabIndex = 21;
            this.pictureBoxLightGreenOn.TabStop = false;
            this.pictureBoxLightGreenOn.Visible = false;
            // 
            // pictureBoxLightGreenOff
            // 
            this.pictureBoxLightGreenOff.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxLightGreenOff.Image")));
            this.pictureBoxLightGreenOff.Location = new System.Drawing.Point(463, 52);
            this.pictureBoxLightGreenOff.Name = "pictureBoxLightGreenOff";
            this.pictureBoxLightGreenOff.Size = new System.Drawing.Size(56, 56);
            this.pictureBoxLightGreenOff.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLightGreenOff.TabIndex = 20;
            this.pictureBoxLightGreenOff.TabStop = false;
            // 
            // panelSeperatorInformation
            // 
            this.panelSeperatorInformation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panelSeperatorInformation.Location = new System.Drawing.Point(0, 246);
            this.panelSeperatorInformation.Name = "panelSeperatorInformation";
            this.panelSeperatorInformation.Size = new System.Drawing.Size(533, 3);
            this.panelSeperatorInformation.TabIndex = 2;
            // 
            // tableLayoutPanelInfoJobs
            // 
            this.tableLayoutPanelInfoJobs.ColumnCount = 2;
            this.tableLayoutPanelInfoJobs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.25519F));
            this.tableLayoutPanelInfoJobs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 77.74481F));
            this.tableLayoutPanelInfoJobs.Controls.Add(this.labelInfoJobsOutstate, 1, 3);
            this.tableLayoutPanelInfoJobs.Controls.Add(this.labelInfoJobsType, 1, 2);
            this.tableLayoutPanelInfoJobs.Controls.Add(this.labelInfoJobsTitleGUID, 0, 0);
            this.tableLayoutPanelInfoJobs.Controls.Add(this.labelInfoJobsTitleID, 0, 1);
            this.tableLayoutPanelInfoJobs.Controls.Add(this.labelInfoJobsTitleType, 0, 2);
            this.tableLayoutPanelInfoJobs.Controls.Add(this.labelInfoJobsGUID, 1, 0);
            this.tableLayoutPanelInfoJobs.Controls.Add(this.labelInfoJobsTitleOutstate, 0, 3);
            this.tableLayoutPanelInfoJobs.Controls.Add(this.labelInfoJobsID, 1, 1);
            this.tableLayoutPanelInfoJobs.Location = new System.Drawing.Point(146, 294);
            this.tableLayoutPanelInfoJobs.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelInfoJobs.Name = "tableLayoutPanelInfoJobs";
            this.tableLayoutPanelInfoJobs.RowCount = 4;
            this.tableLayoutPanelInfoJobs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 23.94366F));
            this.tableLayoutPanelInfoJobs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 23.94366F));
            this.tableLayoutPanelInfoJobs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.35212F));
            this.tableLayoutPanelInfoJobs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 26.76058F));
            this.tableLayoutPanelInfoJobs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelInfoJobs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelInfoJobs.Size = new System.Drawing.Size(374, 87);
            this.tableLayoutPanelInfoJobs.TabIndex = 18;
            // 
            // labelInfoJobsOutstate
            // 
            this.labelInfoJobsOutstate.AutoSize = true;
            this.labelInfoJobsOutstate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.labelInfoJobsOutstate.Location = new System.Drawing.Point(86, 62);
            this.labelInfoJobsOutstate.Name = "labelInfoJobsOutstate";
            this.labelInfoJobsOutstate.Size = new System.Drawing.Size(0, 16);
            this.labelInfoJobsOutstate.TabIndex = 24;
            // 
            // labelInfoJobsType
            // 
            this.labelInfoJobsType.AutoSize = true;
            this.labelInfoJobsType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoJobsType.Location = new System.Drawing.Point(86, 40);
            this.labelInfoJobsType.Name = "labelInfoJobsType";
            this.labelInfoJobsType.Size = new System.Drawing.Size(0, 16);
            this.labelInfoJobsType.TabIndex = 24;
            // 
            // labelInfoJobsTitleGUID
            // 
            this.labelInfoJobsTitleGUID.AutoSize = true;
            this.labelInfoJobsTitleGUID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoJobsTitleGUID.Location = new System.Drawing.Point(3, 0);
            this.labelInfoJobsTitleGUID.Name = "labelInfoJobsTitleGUID";
            this.labelInfoJobsTitleGUID.Size = new System.Drawing.Size(49, 16);
            this.labelInfoJobsTitleGUID.TabIndex = 10;
            this.labelInfoJobsTitleGUID.Text = "GUID:";
            // 
            // labelInfoJobsTitleID
            // 
            this.labelInfoJobsTitleID.AutoSize = true;
            this.labelInfoJobsTitleID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoJobsTitleID.Location = new System.Drawing.Point(3, 20);
            this.labelInfoJobsTitleID.Name = "labelInfoJobsTitleID";
            this.labelInfoJobsTitleID.Size = new System.Drawing.Size(27, 16);
            this.labelInfoJobsTitleID.TabIndex = 3;
            this.labelInfoJobsTitleID.Text = "ID:";
            // 
            // labelInfoJobsTitleType
            // 
            this.labelInfoJobsTitleType.AutoSize = true;
            this.labelInfoJobsTitleType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoJobsTitleType.Location = new System.Drawing.Point(3, 40);
            this.labelInfoJobsTitleType.Name = "labelInfoJobsTitleType";
            this.labelInfoJobsTitleType.Size = new System.Drawing.Size(48, 16);
            this.labelInfoJobsTitleType.TabIndex = 2;
            this.labelInfoJobsTitleType.Text = "Type:";
            // 
            // labelInfoJobsGUID
            // 
            this.labelInfoJobsGUID.AutoSize = true;
            this.labelInfoJobsGUID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoJobsGUID.Location = new System.Drawing.Point(86, 0);
            this.labelInfoJobsGUID.Name = "labelInfoJobsGUID";
            this.labelInfoJobsGUID.Size = new System.Drawing.Size(0, 16);
            this.labelInfoJobsGUID.TabIndex = 12;
            // 
            // labelInfoJobsTitleOutstate
            // 
            this.labelInfoJobsTitleOutstate.AutoSize = true;
            this.labelInfoJobsTitleOutstate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoJobsTitleOutstate.Location = new System.Drawing.Point(3, 62);
            this.labelInfoJobsTitleOutstate.Name = "labelInfoJobsTitleOutstate";
            this.labelInfoJobsTitleOutstate.Size = new System.Drawing.Size(69, 16);
            this.labelInfoJobsTitleOutstate.TabIndex = 11;
            this.labelInfoJobsTitleOutstate.Text = "Outstate:";
            // 
            // labelInfoJobsID
            // 
            this.labelInfoJobsID.AutoSize = true;
            this.labelInfoJobsID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoJobsID.Location = new System.Drawing.Point(86, 20);
            this.labelInfoJobsID.Name = "labelInfoJobsID";
            this.labelInfoJobsID.Size = new System.Drawing.Size(0, 16);
            this.labelInfoJobsID.TabIndex = 13;
            // 
            // labelInfoJobsTitle
            // 
            this.labelInfoJobsTitle.AutoSize = true;
            this.labelInfoJobsTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.labelInfoJobsTitle.Location = new System.Drawing.Point(227, 252);
            this.labelInfoJobsTitle.Name = "labelInfoJobsTitle";
            this.labelInfoJobsTitle.Size = new System.Drawing.Size(69, 29);
            this.labelInfoJobsTitle.TabIndex = 16;
            this.labelInfoJobsTitle.Text = "Jobs";
            this.labelInfoJobsTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tableLayoutPanelInfoGeneral
            // 
            this.tableLayoutPanelInfoGeneral.ColumnCount = 2;
            this.tableLayoutPanelInfoGeneral.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 69F));
            this.tableLayoutPanelInfoGeneral.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelInfoGeneral.Controls.Add(this.textBoxInfoJobsMemo2, 1, 7);
            this.tableLayoutPanelInfoGeneral.Controls.Add(this.labelInfoTitleMac, 0, 0);
            this.tableLayoutPanelInfoGeneral.Controls.Add(this.labelInfoJobsTitleMemo2, 0, 7);
            this.tableLayoutPanelInfoGeneral.Controls.Add(this.labelInfoState, 1, 5);
            this.tableLayoutPanelInfoGeneral.Controls.Add(this.labelInfoName, 1, 4);
            this.tableLayoutPanelInfoGeneral.Controls.Add(this.labelInfoIP, 1, 3);
            this.tableLayoutPanelInfoGeneral.Controls.Add(this.labelInfoID, 1, 1);
            this.tableLayoutPanelInfoGeneral.Controls.Add(this.labelInfoMAC, 1, 0);
            this.tableLayoutPanelInfoGeneral.Controls.Add(this.labelInfoTitleID, 0, 1);
            this.tableLayoutPanelInfoGeneral.Controls.Add(this.labelInfoGUID, 1, 2);
            this.tableLayoutPanelInfoGeneral.Controls.Add(this.labelInfoTitleGUID, 0, 2);
            this.tableLayoutPanelInfoGeneral.Controls.Add(this.labelInfoTitleIP, 0, 3);
            this.tableLayoutPanelInfoGeneral.Controls.Add(this.labelInfoTitleName, 0, 4);
            this.tableLayoutPanelInfoGeneral.Controls.Add(this.labelInfoJobsTitleMemo1, 0, 6);
            this.tableLayoutPanelInfoGeneral.Controls.Add(this.labelInfoTitleState, 0, 5);
            this.tableLayoutPanelInfoGeneral.Controls.Add(this.textBoxInfoJobsMemo1, 1, 6);
            this.tableLayoutPanelInfoGeneral.Location = new System.Drawing.Point(15, 52);
            this.tableLayoutPanelInfoGeneral.Name = "tableLayoutPanelInfoGeneral";
            this.tableLayoutPanelInfoGeneral.RowCount = 8;
            this.tableLayoutPanelInfoGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.tableLayoutPanelInfoGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.8F));
            this.tableLayoutPanelInfoGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.8F));
            this.tableLayoutPanelInfoGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.8F));
            this.tableLayoutPanelInfoGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.6F));
            this.tableLayoutPanelInfoGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.tableLayoutPanelInfoGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanelInfoGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanelInfoGeneral.Size = new System.Drawing.Size(420, 188);
            this.tableLayoutPanelInfoGeneral.TabIndex = 15;
            // 
            // textBoxInfoJobsMemo2
            // 
            this.textBoxInfoJobsMemo2.BackColor = System.Drawing.Color.White;
            this.textBoxInfoJobsMemo2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxInfoJobsMemo2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.textBoxInfoJobsMemo2.Location = new System.Drawing.Point(72, 162);
            this.textBoxInfoJobsMemo2.Multiline = true;
            this.textBoxInfoJobsMemo2.Name = "textBoxInfoJobsMemo2";
            this.textBoxInfoJobsMemo2.ReadOnly = true;
            this.textBoxInfoJobsMemo2.Size = new System.Drawing.Size(345, 23);
            this.textBoxInfoJobsMemo2.TabIndex = 27;
            // 
            // labelInfoTitleMac
            // 
            this.labelInfoTitleMac.AutoSize = true;
            this.labelInfoTitleMac.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoTitleMac.Location = new System.Drawing.Point(3, 0);
            this.labelInfoTitleMac.Name = "labelInfoTitleMac";
            this.labelInfoTitleMac.Size = new System.Drawing.Size(44, 16);
            this.labelInfoTitleMac.TabIndex = 1;
            this.labelInfoTitleMac.Text = "MAC:";
            // 
            // labelInfoJobsTitleMemo2
            // 
            this.labelInfoJobsTitleMemo2.AutoSize = true;
            this.labelInfoJobsTitleMemo2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoJobsTitleMemo2.Location = new System.Drawing.Point(3, 159);
            this.labelInfoJobsTitleMemo2.Name = "labelInfoJobsTitleMemo2";
            this.labelInfoJobsTitleMemo2.Size = new System.Drawing.Size(62, 16);
            this.labelInfoJobsTitleMemo2.TabIndex = 26;
            this.labelInfoJobsTitleMemo2.Text = "Memo2:";
            // 
            // labelInfoState
            // 
            this.labelInfoState.AutoSize = true;
            this.labelInfoState.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoState.Location = new System.Drawing.Point(72, 110);
            this.labelInfoState.Name = "labelInfoState";
            this.labelInfoState.Size = new System.Drawing.Size(0, 16);
            this.labelInfoState.TabIndex = 10;
            // 
            // labelInfoName
            // 
            this.labelInfoName.AutoSize = true;
            this.labelInfoName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoName.Location = new System.Drawing.Point(72, 87);
            this.labelInfoName.Name = "labelInfoName";
            this.labelInfoName.Size = new System.Drawing.Size(0, 16);
            this.labelInfoName.TabIndex = 11;
            // 
            // labelInfoIP
            // 
            this.labelInfoIP.AutoSize = true;
            this.labelInfoIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoIP.Location = new System.Drawing.Point(72, 65);
            this.labelInfoIP.Name = "labelInfoIP";
            this.labelInfoIP.Size = new System.Drawing.Size(0, 16);
            this.labelInfoIP.TabIndex = 12;
            // 
            // labelInfoID
            // 
            this.labelInfoID.AutoSize = true;
            this.labelInfoID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoID.Location = new System.Drawing.Point(72, 21);
            this.labelInfoID.Name = "labelInfoID";
            this.labelInfoID.Size = new System.Drawing.Size(0, 16);
            this.labelInfoID.TabIndex = 14;
            // 
            // labelInfoMAC
            // 
            this.labelInfoMAC.AutoSize = true;
            this.labelInfoMAC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoMAC.Location = new System.Drawing.Point(72, 0);
            this.labelInfoMAC.Name = "labelInfoMAC";
            this.labelInfoMAC.Size = new System.Drawing.Size(0, 16);
            this.labelInfoMAC.TabIndex = 5;
            // 
            // labelInfoTitleID
            // 
            this.labelInfoTitleID.AutoSize = true;
            this.labelInfoTitleID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoTitleID.Location = new System.Drawing.Point(3, 21);
            this.labelInfoTitleID.Name = "labelInfoTitleID";
            this.labelInfoTitleID.Size = new System.Drawing.Size(27, 16);
            this.labelInfoTitleID.TabIndex = 2;
            this.labelInfoTitleID.Text = "ID:";
            // 
            // labelInfoGUID
            // 
            this.labelInfoGUID.AutoSize = true;
            this.labelInfoGUID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoGUID.Location = new System.Drawing.Point(72, 43);
            this.labelInfoGUID.Name = "labelInfoGUID";
            this.labelInfoGUID.Size = new System.Drawing.Size(0, 16);
            this.labelInfoGUID.TabIndex = 13;
            // 
            // labelInfoTitleGUID
            // 
            this.labelInfoTitleGUID.AutoSize = true;
            this.labelInfoTitleGUID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoTitleGUID.Location = new System.Drawing.Point(3, 43);
            this.labelInfoTitleGUID.Name = "labelInfoTitleGUID";
            this.labelInfoTitleGUID.Size = new System.Drawing.Size(49, 16);
            this.labelInfoTitleGUID.TabIndex = 9;
            this.labelInfoTitleGUID.Text = "GUID:";
            // 
            // labelInfoTitleIP
            // 
            this.labelInfoTitleIP.AutoSize = true;
            this.labelInfoTitleIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoTitleIP.Location = new System.Drawing.Point(3, 65);
            this.labelInfoTitleIP.Name = "labelInfoTitleIP";
            this.labelInfoTitleIP.Size = new System.Drawing.Size(26, 16);
            this.labelInfoTitleIP.TabIndex = 6;
            this.labelInfoTitleIP.Text = "IP:";
            // 
            // labelInfoTitleName
            // 
            this.labelInfoTitleName.AutoSize = true;
            this.labelInfoTitleName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoTitleName.Location = new System.Drawing.Point(3, 87);
            this.labelInfoTitleName.Name = "labelInfoTitleName";
            this.labelInfoTitleName.Size = new System.Drawing.Size(53, 16);
            this.labelInfoTitleName.TabIndex = 8;
            this.labelInfoTitleName.Text = "Name:";
            // 
            // labelInfoJobsTitleMemo1
            // 
            this.labelInfoJobsTitleMemo1.AutoSize = true;
            this.labelInfoJobsTitleMemo1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoJobsTitleMemo1.Location = new System.Drawing.Point(3, 131);
            this.labelInfoJobsTitleMemo1.Name = "labelInfoJobsTitleMemo1";
            this.labelInfoJobsTitleMemo1.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.labelInfoJobsTitleMemo1.Size = new System.Drawing.Size(62, 18);
            this.labelInfoJobsTitleMemo1.TabIndex = 25;
            this.labelInfoJobsTitleMemo1.Text = "Memo1:";
            // 
            // labelInfoTitleState
            // 
            this.labelInfoTitleState.AutoSize = true;
            this.labelInfoTitleState.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoTitleState.Location = new System.Drawing.Point(3, 110);
            this.labelInfoTitleState.Name = "labelInfoTitleState";
            this.labelInfoTitleState.Size = new System.Drawing.Size(48, 16);
            this.labelInfoTitleState.TabIndex = 7;
            this.labelInfoTitleState.Text = "State:";
            // 
            // textBoxInfoJobsMemo1
            // 
            this.textBoxInfoJobsMemo1.BackColor = System.Drawing.Color.White;
            this.textBoxInfoJobsMemo1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxInfoJobsMemo1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.textBoxInfoJobsMemo1.Location = new System.Drawing.Point(72, 134);
            this.textBoxInfoJobsMemo1.Multiline = true;
            this.textBoxInfoJobsMemo1.Name = "textBoxInfoJobsMemo1";
            this.textBoxInfoJobsMemo1.ReadOnly = true;
            this.textBoxInfoJobsMemo1.Size = new System.Drawing.Size(345, 22);
            this.textBoxInfoJobsMemo1.TabIndex = 24;
            // 
            // labelInformationTitle
            // 
            this.labelInformationTitle.AutoSize = true;
            this.labelInformationTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInformationTitle.Location = new System.Drawing.Point(174, 12);
            this.labelInformationTitle.Name = "labelInformationTitle";
            this.labelInformationTitle.Size = new System.Drawing.Size(161, 31);
            this.labelInformationTitle.TabIndex = 0;
            this.labelInformationTitle.Text = "Information";
            this.labelInformationTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 562);
            this.Controls.Add(this.panelInformation);
            this.Controls.Add(this.labelLastReloadTime);
            this.Controls.Add(this.labelLastReloadTimeTitle);
            this.Controls.Add(this.labelConfigStatus);
            this.Controls.Add(this.labelConfigStatusTitle);
            this.Controls.Add(this.buttonReload);
            this.Controls.Add(this.listBoxNodes);
            this.Controls.Add(this.panelSeperator);
            this.Controls.Add(this.panelTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MAD - Networkmonitoring | Nodes";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTopPanel)).EndInit();
            this.panelInformation.ResumeLayout(false);
            this.panelInformation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLightRedoff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLightRedOn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLightGreenOn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLightGreenOff)).EndInit();
            this.tableLayoutPanelInfoJobs.ResumeLayout(false);
            this.tableLayoutPanelInfoJobs.PerformLayout();
            this.tableLayoutPanelInfoGeneral.ResumeLayout(false);
            this.tableLayoutPanelInfoGeneral.PerformLayout();
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
        private System.Windows.Forms.Panel panelInformation;
        private System.Windows.Forms.Label labelInfoTitleID;
        private System.Windows.Forms.Label labelInfoTitleMac;
        private System.Windows.Forms.Label labelInformationTitle;
        private System.Windows.Forms.Label labelInfoTitleGUID;
        private System.Windows.Forms.Label labelInfoTitleName;
        private System.Windows.Forms.Label labelInfoTitleState;
        private System.Windows.Forms.Label labelInfoTitleIP;
        private System.Windows.Forms.Label labelInfoMAC;
        private System.Windows.Forms.Label labelInfoID;
        private System.Windows.Forms.Label labelInfoGUID;
        private System.Windows.Forms.Label labelInfoIP;
        private System.Windows.Forms.Label labelInfoName;
        private System.Windows.Forms.Label labelInfoState;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelInfoGeneral;
        private System.Windows.Forms.ListBox listBoxInfoJobs;
        private System.Windows.Forms.Label labelInfoJobsTitle;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelInfoJobs;
        private System.Windows.Forms.Button buttonCLI;
        private System.Windows.Forms.Panel panelSeperatorInformation;
        private System.Windows.Forms.PictureBox pictureBoxLightGreenOn;
        private System.Windows.Forms.PictureBox pictureBoxLightGreenOff;
        private System.Windows.Forms.PictureBox pictureBoxLightRedoff;
        private System.Windows.Forms.PictureBox pictureBoxLightRedOn;
        private System.Windows.Forms.Label labelInfoJobsTitleID;
        private System.Windows.Forms.Label labelInfoJobsTitleType;
        private System.Windows.Forms.Label labelInfoJobsTitleGUID;
        private System.Windows.Forms.Label labelInfoJobsTitleOutstate;
        private System.Windows.Forms.Label labelInfoJobsOutstate;
        private System.Windows.Forms.Label labelInfoJobsType;
        private System.Windows.Forms.Label labelInfoJobsGUID;
        private System.Windows.Forms.Label labelInfoJobsID;
        private System.Windows.Forms.Button buttonInfo;
        private System.Windows.Forms.TextBox textBoxInfoJobsMemo2;
        private System.Windows.Forms.TextBox textBoxInfoJobsMemo1;
        private System.Windows.Forms.Label labelInfoJobsTitleMemo2;
        private System.Windows.Forms.Label labelInfoJobsTitleMemo1;
        public System.Windows.Forms.Button buttonScan;
    }
}