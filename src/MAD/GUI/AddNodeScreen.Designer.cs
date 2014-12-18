namespace MAD.GUI
{
    partial class AddNodeScreen
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
            this.nodeNameLabel = new System.Windows.Forms.Label();
            this.nodeNameTextBox = new System.Windows.Forms.TextBox();
            this.macAddrTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ipAddrTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.addButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.titlePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.titlePicBox)).BeginInit();
            this.SuspendLayout();
            // 
            // titlePanel
            // 
            this.titlePanel.Size = new System.Drawing.Size(323, 64);
            // 
            // title
            // 
            this.title.Size = new System.Drawing.Size(213, 51);
            this.title.Text = "Add Node";
            // 
            // nodeNameLabel
            // 
            this.nodeNameLabel.AutoSize = true;
            this.nodeNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nodeNameLabel.ForeColor = System.Drawing.Color.Cyan;
            this.nodeNameLabel.Location = new System.Drawing.Point(9, 102);
            this.nodeNameLabel.Name = "nodeNameLabel";
            this.nodeNameLabel.Size = new System.Drawing.Size(67, 13);
            this.nodeNameLabel.TabIndex = 4;
            this.nodeNameLabel.Text = "Node-Name:";
            // 
            // nodeNameTextBox
            // 
            this.nodeNameTextBox.Location = new System.Drawing.Point(12, 118);
            this.nodeNameTextBox.Name = "nodeNameTextBox";
            this.nodeNameTextBox.Size = new System.Drawing.Size(100, 20);
            this.nodeNameTextBox.TabIndex = 5;
            this.nodeNameTextBox.Text = "Boss";
            this.nodeNameTextBox.TextChanged += new System.EventHandler(this.nodeNameTextBox_TextChanged);
            // 
            // macAddrTextBox
            // 
            this.macAddrTextBox.Location = new System.Drawing.Point(158, 118);
            this.macAddrTextBox.Name = "macAddrTextBox";
            this.macAddrTextBox.Size = new System.Drawing.Size(100, 20);
            this.macAddrTextBox.TabIndex = 7;
            this.macAddrTextBox.Text = "FFFFFFFFFFFF";
            this.macAddrTextBox.TextChanged += new System.EventHandler(this.macAddrTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Cyan;
            this.label1.Location = new System.Drawing.Point(155, 102);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "MAC - Address:";
            // 
            // ipAddrTextBox
            // 
            this.ipAddrTextBox.Location = new System.Drawing.Point(12, 179);
            this.ipAddrTextBox.Name = "ipAddrTextBox";
            this.ipAddrTextBox.Size = new System.Drawing.Size(100, 20);
            this.ipAddrTextBox.TabIndex = 9;
            this.ipAddrTextBox.Text = "192.168.1.1";
            this.ipAddrTextBox.TextChanged += new System.EventHandler(this.ipAddrTextBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Cyan;
            this.label2.Location = new System.Drawing.Point(9, 163);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "IP-Address:";
            // 
            // addButton
            // 
            this.addButton.BackColor = System.Drawing.Color.Cyan;
            this.addButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addButton.Location = new System.Drawing.Point(115, 227);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 10;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = false;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.Cyan;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.Location = new System.Drawing.Point(209, 227);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 11;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // AddNodeScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 255);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.ipAddrTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.macAddrTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nodeNameTextBox);
            this.Controls.Add(this.nodeNameLabel);
            this.Name = "AddNodeScreen";
            this.Text = "AddNode";
            this.Controls.SetChildIndex(this.titlePanel, 0);
            this.Controls.SetChildIndex(this.nodeNameLabel, 0);
            this.Controls.SetChildIndex(this.nodeNameTextBox, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.macAddrTextBox, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.ipAddrTextBox, 0);
            this.Controls.SetChildIndex(this.addButton, 0);
            this.Controls.SetChildIndex(this.cancelButton, 0);
            this.titlePanel.ResumeLayout(false);
            this.titlePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.titlePicBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label nodeNameLabel;
        private System.Windows.Forms.TextBox nodeNameTextBox;
        private System.Windows.Forms.TextBox macAddrTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ipAddrTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button cancelButton;
    }
}