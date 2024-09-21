using FluentFTP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UIXLiteConfigEditor
{
    public partial class FTPConfigForm : Form
    {
        public FTPConfigForm()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            //Verify IP
            if (!Regex.IsMatch(IPTextBox.Text, "^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$"))
            {
                MessageBox.Show("Invalid IP.", "IP Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }

            string XboxIP = IPTextBox.Text;
            string XboxUsername = UsernameTextBox.Text;
            string XboxPassword = PasswordTextBox.Text;
            int XboxPort = (int)PortNumericUpDown.Value;

            //Save settings
            Properties.Settings.Default.IP = XboxIP;
            Properties.Settings.Default.Username = XboxUsername;
            Properties.Settings.Default.Password = XboxPassword;
            Properties.Settings.Default.Port = XboxPort;

            Properties.Settings.Default.Save();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void IPTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(IPTextBox.Text) && PortNumericUpDown.Value > 0)
            {
                OkButton.Enabled = true;
            }
            else
            {
                OkButton.Enabled = false;
            }
        }

        private void PortNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (PortNumericUpDown.Value > 0 && !string.IsNullOrEmpty(IPTextBox.Text))
            {
                OkButton.Enabled = true;
            } else
            {
                OkButton.Enabled = false;
            }
        }

        private void FTPConfigForm_Load(object sender, EventArgs e)
        {
            //Load settings
            IPTextBox.Text = Properties.Settings.Default.IP;
            UsernameTextBox.Text = Properties.Settings.Default.Username;
            PasswordTextBox.Text = Properties.Settings.Default.Password;
            PortNumericUpDown.Value = Properties.Settings.Default.Port;
        }
    }
}
