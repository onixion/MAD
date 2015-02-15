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
            this.panelInformation = new System.Windows.Forms.Panel();
            this.tableLayoutPanelInfoGeneral = new System.Windows.Forms.TableLayoutPanel();
            this.labelInfoTitleMac = new System.Windows.Forms.Label();
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
            this.labelInfoTitleState = new System.Windows.Forms.Label();
            this.labelInformationTitle = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.listBoxInfoJobs = new System.Windows.Forms.ListBox();
            this.tableLayoutPanelInfoJobs = new System.Windows.Forms.TableLayoutPanel();
            this.listBoxInfoJobStates = new System.Windows.Forms.ListBox();
            this.listBoxInfoJobStateContent = new System.Windows.Forms.ListBox();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTopPanel)).BeginInit();
            this.panelInformation.SuspendLayout();
            this.tableLayoutPanelInfoGeneral.SuspendLayout();
            this.tableLayoutPanelInfoJobs.SuspendLayout();
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
            // panelInformation
            // 
            this.panelInformation.Controls.Add(this.tableLayoutPanelInfoJobs);
            this.panelInformation.Controls.Add(this.label1);
            this.panelInformation.Controls.Add(this.tableLayoutPanelInfoGeneral);
            this.panelInformation.Controls.Add(this.labelInformationTitle);
            this.panelInformation.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelInformation.Location = new System.Drawing.Point(393, 75);
            this.panelInformation.Name = "panelInformation";
            this.panelInformation.Size = new System.Drawing.Size(491, 487);
            this.panelInformation.TabIndex = 9;
            // 
            // tableLayoutPanelInfoGeneral
            // 
            this.tableLayoutPanelInfoGeneral.ColumnCount = 2;
            this.tableLayoutPanelInfoGeneral.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanelInfoGeneral.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelInfoGeneral.Controls.Add(this.labelInfoTitleMac, 0, 0);
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
            this.tableLayoutPanelInfoGeneral.Controls.Add(this.labelInfoTitleState, 0, 5);
            this.tableLayoutPanelInfoGeneral.Location = new System.Drawing.Point(15, 52);
            this.tableLayoutPanelInfoGeneral.Name = "tableLayoutPanelInfoGeneral";
            this.tableLayoutPanelInfoGeneral.RowCount = 6;
            this.tableLayoutPanelInfoGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanelInfoGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanelInfoGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanelInfoGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanelInfoGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanelInfoGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanelInfoGeneral.Size = new System.Drawing.Size(452, 125);
            this.tableLayoutPanelInfoGeneral.TabIndex = 15;
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
            // labelInfoState
            // 
            this.labelInfoState.AutoSize = true;
            this.labelInfoState.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoState.Location = new System.Drawing.Point(65, 100);
            this.labelInfoState.Name = "labelInfoState";
            this.labelInfoState.Size = new System.Drawing.Size(0, 16);
            this.labelInfoState.TabIndex = 10;
            // 
            // labelInfoName
            // 
            this.labelInfoName.AutoSize = true;
            this.labelInfoName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoName.Location = new System.Drawing.Point(65, 80);
            this.labelInfoName.Name = "labelInfoName";
            this.labelInfoName.Size = new System.Drawing.Size(0, 16);
            this.labelInfoName.TabIndex = 11;
            // 
            // labelInfoIP
            // 
            this.labelInfoIP.AutoSize = true;
            this.labelInfoIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoIP.Location = new System.Drawing.Point(65, 60);
            this.labelInfoIP.Name = "labelInfoIP";
            this.labelInfoIP.Size = new System.Drawing.Size(0, 16);
            this.labelInfoIP.TabIndex = 12;
            // 
            // labelInfoID
            // 
            this.labelInfoID.AutoSize = true;
            this.labelInfoID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoID.Location = new System.Drawing.Point(65, 20);
            this.labelInfoID.Name = "labelInfoID";
            this.labelInfoID.Size = new System.Drawing.Size(0, 16);
            this.labelInfoID.TabIndex = 14;
            // 
            // labelInfoMAC
            // 
            this.labelInfoMAC.AutoSize = true;
            this.labelInfoMAC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoMAC.Location = new System.Drawing.Point(65, 0);
            this.labelInfoMAC.Name = "labelInfoMAC";
            this.labelInfoMAC.Size = new System.Drawing.Size(0, 16);
            this.labelInfoMAC.TabIndex = 5;
            // 
            // labelInfoTitleID
            // 
            this.labelInfoTitleID.AutoSize = true;
            this.labelInfoTitleID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoTitleID.Location = new System.Drawing.Point(3, 20);
            this.labelInfoTitleID.Name = "labelInfoTitleID";
            this.labelInfoTitleID.Size = new System.Drawing.Size(27, 16);
            this.labelInfoTitleID.TabIndex = 2;
            this.labelInfoTitleID.Text = "ID:";
            // 
            // labelInfoGUID
            // 
            this.labelInfoGUID.AutoSize = true;
            this.labelInfoGUID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoGUID.Location = new System.Drawing.Point(65, 40);
            this.labelInfoGUID.Name = "labelInfoGUID";
            this.labelInfoGUID.Size = new System.Drawing.Size(0, 16);
            this.labelInfoGUID.TabIndex = 13;
            // 
            // labelInfoTitleGUID
            // 
            this.labelInfoTitleGUID.AutoSize = true;
            this.labelInfoTitleGUID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoTitleGUID.Location = new System.Drawing.Point(3, 40);
            this.labelInfoTitleGUID.Name = "labelInfoTitleGUID";
            this.labelInfoTitleGUID.Size = new System.Drawing.Size(49, 16);
            this.labelInfoTitleGUID.TabIndex = 9;
            this.labelInfoTitleGUID.Text = "GUID:";
            // 
            // labelInfoTitleIP
            // 
            this.labelInfoTitleIP.AutoSize = true;
            this.labelInfoTitleIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoTitleIP.Location = new System.Drawing.Point(3, 60);
            this.labelInfoTitleIP.Name = "labelInfoTitleIP";
            this.labelInfoTitleIP.Size = new System.Drawing.Size(26, 16);
            this.labelInfoTitleIP.TabIndex = 6;
            this.labelInfoTitleIP.Text = "IP:";
            // 
            // labelInfoTitleName
            // 
            this.labelInfoTitleName.AutoSize = true;
            this.labelInfoTitleName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoTitleName.Location = new System.Drawing.Point(3, 80);
            this.labelInfoTitleName.Name = "labelInfoTitleName";
            this.labelInfoTitleName.Size = new System.Drawing.Size(53, 16);
            this.labelInfoTitleName.TabIndex = 8;
            this.labelInfoTitleName.Text = "Name:";
            // 
            // labelInfoTitleState
            // 
            this.labelInfoTitleState.AutoSize = true;
            this.labelInfoTitleState.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoTitleState.Location = new System.Drawing.Point(3, 100);
            this.labelInfoTitleState.Name = "labelInfoTitleState";
            this.labelInfoTitleState.Size = new System.Drawing.Size(48, 16);
            this.labelInfoTitleState.TabIndex = 7;
            this.labelInfoTitleState.Text = "State:";
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(15, 212);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 29);
            this.label1.TabIndex = 16;
            this.label1.Text = "Jobs:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // listBoxInfoJobs
            // 
            this.listBoxInfoJobs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxInfoJobs.FormattingEnabled = true;
            this.listBoxInfoJobs.Location = new System.Drawing.Point(0, 0);
            this.listBoxInfoJobs.Margin = new System.Windows.Forms.Padding(0);
            this.listBoxInfoJobs.Name = "listBoxInfoJobs";
            this.listBoxInfoJobs.Size = new System.Drawing.Size(148, 197);
            this.listBoxInfoJobs.TabIndex = 17;
            // 
            // tableLayoutPanelInfoJobs
            // 
            this.tableLayoutPanelInfoJobs.ColumnCount = 3;
            this.tableLayoutPanelInfoJobs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelInfoJobs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelInfoJobs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelInfoJobs.Controls.Add(this.listBoxInfoJobStateContent, 2, 0);
            this.tableLayoutPanelInfoJobs.Controls.Add(this.listBoxInfoJobStates, 1, 0);
            this.tableLayoutPanelInfoJobs.Controls.Add(this.listBoxInfoJobs, 0, 0);
            this.tableLayoutPanelInfoJobs.Location = new System.Drawing.Point(21, 241);
            this.tableLayoutPanelInfoJobs.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelInfoJobs.Name = "tableLayoutPanelInfoJobs";
            this.tableLayoutPanelInfoJobs.RowCount = 1;
            this.tableLayoutPanelInfoJobs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelInfoJobs.Size = new System.Drawing.Size(446, 207);
            this.tableLayoutPanelInfoJobs.TabIndex = 18;
            // 
            // listBoxInfoJobStates
            // 
            this.listBoxInfoJobStates.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxInfoJobStates.FormattingEnabled = true;
            this.listBoxInfoJobStates.Location = new System.Drawing.Point(148, 0);
            this.listBoxInfoJobStates.Margin = new System.Windows.Forms.Padding(0);
            this.listBoxInfoJobStates.Name = "listBoxInfoJobStates";
            this.listBoxInfoJobStates.Size = new System.Drawing.Size(148, 197);
            this.listBoxInfoJobStates.TabIndex = 18;
            // 
            // listBoxInfoJobStateContent
            // 
            this.listBoxInfoJobStateContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxInfoJobStateContent.FormattingEnabled = true;
            this.listBoxInfoJobStateContent.Location = new System.Drawing.Point(296, 0);
            this.listBoxInfoJobStateContent.Margin = new System.Windows.Forms.Padding(0);
            this.listBoxInfoJobStateContent.Name = "listBoxInfoJobStateContent";
            this.listBoxInfoJobStateContent.Size = new System.Drawing.Size(150, 197);
            this.listBoxInfoJobStateContent.TabIndex = 19;
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
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "BaseGUIrebulid";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTopPanel)).EndInit();
            this.panelInformation.ResumeLayout(false);
            this.panelInformation.PerformLayout();
            this.tableLayoutPanelInfoGeneral.ResumeLayout(false);
            this.tableLayoutPanelInfoGeneral.PerformLayout();
            this.tableLayoutPanelInfoJobs.ResumeLayout(false);
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelInfoJobs;
        private System.Windows.Forms.ListBox listBoxInfoJobStateContent;
        private System.Windows.Forms.ListBox listBoxInfoJobStates;
    }
}