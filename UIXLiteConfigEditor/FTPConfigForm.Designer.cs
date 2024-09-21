namespace UIXLiteConfigEditor
{
    partial class FTPConfigForm
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
            components = new System.ComponentModel.Container();
            OkButton = new Button();
            CancelButton = new Button();
            label1 = new Label();
            groupBox1 = new GroupBox();
            IPTextBox = new TextBox();
            label4 = new Label();
            PortNumericUpDown = new NumericUpDown();
            PasswordTextBox = new TextBox();
            UsernameTextBox = new TextBox();
            label3 = new Label();
            label2 = new Label();
            toolTip1 = new ToolTip(components);
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PortNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // OkButton
            // 
            OkButton.DialogResult = DialogResult.OK;
            OkButton.Enabled = false;
            OkButton.Location = new Point(12, 192);
            OkButton.Name = "OkButton";
            OkButton.Size = new Size(94, 29);
            OkButton.TabIndex = 0;
            OkButton.Text = "OK";
            toolTip1.SetToolTip(OkButton, "Save the FTP settings.");
            OkButton.UseVisualStyleBackColor = true;
            OkButton.Click += OkButton_Click;
            // 
            // CancelButton
            // 
            CancelButton.Location = new Point(291, 192);
            CancelButton.Name = "CancelButton";
            CancelButton.Size = new Size(94, 29);
            CancelButton.TabIndex = 1;
            CancelButton.Text = "Cancel";
            CancelButton.UseVisualStyleBackColor = true;
            CancelButton.Click += CancelButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(114, 25);
            label1.Name = "label1";
            label1.Size = new Size(21, 20);
            label1.TabIndex = 2;
            label1.Text = "IP";
            toolTip1.SetToolTip(label1, "The IP address of the Xbox to connect to.");
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(IPTextBox);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(PortNumericUpDown);
            groupBox1.Controls.Add(PasswordTextBox);
            groupBox1.Controls.Add(UsernameTextBox);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(13, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(370, 170);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "FTP Settings";
            toolTip1.SetToolTip(groupBox1, "FTP settings to connect to an Xbox for downloading and uploading config files.");
            // 
            // IPTextBox
            // 
            IPTextBox.Location = new Point(141, 22);
            IPTextBox.Name = "IPTextBox";
            IPTextBox.Size = new Size(170, 27);
            IPTextBox.TabIndex = 12;
            toolTip1.SetToolTip(IPTextBox, "The IP address of the Xbox to connect to.");
            IPTextBox.TextChanged += IPTextBox_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(149, 123);
            label4.Name = "label4";
            label4.Size = new Size(35, 20);
            label4.TabIndex = 10;
            label4.Text = "Port";
            toolTip1.SetToolTip(label4, "The port to use for connecting to the Xbox. This is usually Port 21.");
            // 
            // PortNumericUpDown
            // 
            PortNumericUpDown.Location = new Point(190, 121);
            PortNumericUpDown.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            PortNumericUpDown.Name = "PortNumericUpDown";
            PortNumericUpDown.Size = new Size(63, 27);
            PortNumericUpDown.TabIndex = 9;
            toolTip1.SetToolTip(PortNumericUpDown, "The port to use for connecting to the Xbox. This is usually Port 21.");
            PortNumericUpDown.ValueChanged += PortNumericUpDown_ValueChanged;
            // 
            // PasswordTextBox
            // 
            PasswordTextBox.Location = new Point(141, 88);
            PasswordTextBox.Name = "PasswordTextBox";
            PasswordTextBox.Size = new Size(170, 27);
            PasswordTextBox.TabIndex = 8;
            toolTip1.SetToolTip(PasswordTextBox, "The Password for logging into the Xbox. This is usually \"xbox\".");
            // 
            // UsernameTextBox
            // 
            UsernameTextBox.Location = new Point(141, 55);
            UsernameTextBox.Name = "UsernameTextBox";
            UsernameTextBox.Size = new Size(170, 27);
            UsernameTextBox.TabIndex = 7;
            toolTip1.SetToolTip(UsernameTextBox, "The Username for logging into the Xbox. This is usually \"xbox\".");
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(65, 91);
            label3.Name = "label3";
            label3.Size = new Size(70, 20);
            label3.TabIndex = 6;
            label3.Text = "Password";
            toolTip1.SetToolTip(label3, "The Password for logging into the Xbox. This is usually \"xbox\".");
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(60, 58);
            label2.Name = "label2";
            label2.Size = new Size(75, 20);
            label2.TabIndex = 5;
            label2.Text = "Username";
            toolTip1.SetToolTip(label2, "The Username for logging into the Xbox. This is usually \"xbox\".");
            // 
            // FTPConfigForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(397, 233);
            Controls.Add(groupBox1);
            Controls.Add(CancelButton);
            Controls.Add(OkButton);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FTPConfigForm";
            ShowIcon = false;
            Text = "Configure Xbox FTP Settings";
            Load += FTPConfigForm_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PortNumericUpDown).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button OkButton;
        private Button CancelButton;
        private Label label1;
        private GroupBox groupBox1;
        private Label label2;
        private TextBox IPTextBox;
        private TextBox PasswordTextBox;
        private TextBox UsernameTextBox;
        private Label label3;
        private ToolTip toolTip1;
        private NumericUpDown PortNumericUpDown;
        private Label label4;
    }
}