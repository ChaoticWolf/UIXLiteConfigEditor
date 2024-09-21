using Microsoft.VisualBasic.ApplicationServices;
using IniParser;
using IniParser.Model;
using IniParser.Model.Configuration;
using FluentFTP;
using System.Configuration;
using System.Windows.Forms;
using System.Reflection;
using System.Net;
using FluentFTP.Exceptions;
using System.Linq.Expressions;
using System.Diagnostics;
using System.Xml;
using System.Security.Cryptography;

namespace UIXLiteConfigEditor
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private string ConfigFile = "";
        bool ConfigLoaded;
        bool ConfigDownloaded;
        FtpClient FTP;

        private void MainForm_Load(object sender, EventArgs e)
        {
            Text += String.Format(" v{0}", ProductVersion);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Open UIX Lite Config";
                ofd.Filter = "UIX Lite Config (*.xbx,*.ini)|*.xbx;*.ini";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    ConfigFile = ofd.FileName;
                    if (LoadUIXConfig(ConfigFile))
                    {
                        toolStripStatusLabel1.Text = "Loaded config";
                    }
                }
            }
        }

        private void saveToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            //If the user opened a config to edit, save the settings to it, otherwise prompt for where to save the config
            if (!ConfigLoaded)
            {
                using (SaveFileDialog save = new SaveFileDialog())
                {
                    save.Title = "Save UIX Lite Config";
                    save.Filter = "UIX Lite Config (*.xbx,*.ini)|*.xbx;*.ini";
                    save.FileName = "config";

                    if (save.ShowDialog() == DialogResult.OK)
                    {
                        FileStream fs = (FileStream)save.OpenFile();
                        fs.Close();

                        ConfigFile = save.FileName;
                    }
                    else
                    {
                        return;
                    }
                }
            }

            if (SaveUIXConfig(ConfigFile))
            {
                toolStripStatusLabel1.Text = "Config saved";

                //If the user downloaded the config from their Xbox, ask them if they want to upload it
                if (ConfigDownloaded)
                {
                    DialogResult = MessageBox.Show("Do you want to upload the config to your Xbox?", "Upload Config?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DialogResult == DialogResult.Yes)
                    {
                        if (UploadToXbox())
                        {
                            this.toolStripStatusLabel1.Text = "Saved config uploaded to Xbox";
                            MessageBox.Show("Saved config uploaded to the Xbox!", "Config Uploaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        if (FTP.IsConnected)
                        {
                            FTP.Disconnect();
                        }
                    }
                }
            }
        }

        private void downloadFromXboxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DownloadFromXbox())
            {
                toolStripStatusLabel1.Text = "Downloaded config from Xbox";
            }

            if (FTP.IsConnected)
            {
                FTP.Disconnect();
            }
        }

        private void uploadToXboxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UploadToXbox())
            {
                this.toolStripStatusLabel1.Text = "Config uploaded to Xbox";
                MessageBox.Show("Config uploaded to the Xbox!", "Config Uploaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (FTP.IsConnected)
            {
                FTP.Disconnect();
            }
        }

        private void exitToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            this.Close();
        }

        private void configureFTPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FTPConfigForm FTPConfig = new FTPConfigForm())
            {
                if (FTPConfig.ShowDialog() == DialogResult.OK)
                {
                    toolStripStatusLabel1.Text = "Xbox FTP settings configured";
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Form AboutForm = new AboutForm())
            {
                AboutForm.ShowDialog();
            }
        }

        private bool LoadUIXConfig(string ConfigFile)
        {
            try
            {
                var parser = new FileIniDataParser();
                IniData data = parser.ReadFile(ConfigFile);

                //Reset the form settings
                this.Controls.Clear();
                this.InitializeComponent();

                //Load settings from the config and add them to the form

                //Default
                string MainOrbStyle = data["default"]["MainOrbStyle"];
                string ShowInsignia = data["default"]["ShowInsignia"];
                string ConfigPanelIcon = data["default"]["ConfigPanelIcon"];
                string LauncherOrbIcon = data["default"]["LauncherOrbIcon"];
                string UseMainMenuAttract = data["default"]["UseMainMenuAttract"];
                string UsetHcScreensaver = data["default"]["UsetHcScreenSaver"];

                this.MainOrbStyleComboBox.Text = MainOrbStyle;
                if (ShowInsignia == "true")
                {
                    this.ShowInsigniaCheckBox.Checked = true;
                }
                this.LauncherOrbIconComboBox.Text = LauncherOrbIcon;
                this.UIXSettingsIconComboBox.Text = ConfigPanelIcon;
                if (UseMainMenuAttract == "true")
                {
                    this.AnimateMainMenuCheckBox.Checked = true;
                }
                if (UsetHcScreensaver == "true")
                {
                    this.UsetHcScreensaverCheckBox.Checked = true;
                }

                //Main Menu
                string MainMenuItems = data["MainMenu"]["MainMenuItems"];
                string Button1Text = data["MainMenu"]["Button1Text"];
                string Button1Action = data["MainMenu"]["Button1Action"];
                string Button2Text = data["MainMenu"]["Button2Text"];
                string Button2Action = data["MainMenu"]["Button2Action"];
                string Button3Text = data["MainMenu"]["Button3Text"];
                string Button3Action = data["MainMenu"]["Button3Action"];
                string Button4Text = data["MainMenu"]["Button4Text"];
                string Button4Action = data["MainMenu"]["Button4Action"];
                string ButtonYXAction = data["MainMenu"]["ButtonYXAction"];
                string AdvancedMode = data["MainMenu"]["AdvancedMode"];

                this.MainMenuItemsNumericUpDown.Text = MainMenuItems;
                this.Item1TextComboBox.Text = Button1Text;
                this.Item1ActionComboBox.Text = Button1Action;
                this.Item2TextComboBox.Text = Button2Text;
                this.Item2ActionComboBox.Text = Button2Action;
                this.Item3TextComboBox.Text = Button3Text;
                this.Item3ActionComboBox.Text = Button3Action;
                this.Item4TextComboBox.Text = Button4Text;
                this.Item4ActionComboBox.Text = Button4Action;
                this.ButtonYXActionComboBox.Text = ButtonYXAction;
                if (AdvancedModeCheckBox.Checked)
                {
                    this.AdvancedModeCheckBox.Checked = true;
                }

                //Launcher Menu
                string Title0 = data["LauncherMenu"]["Title0"];
                string Path0 = data["LauncherMenu"]["Path0"];
                string Title1 = data["LauncherMenu"]["Title1"];
                string Path1 = data["LauncherMenu"]["Path1"];
                string Title2 = data["LauncherMenu"]["Title2"];
                string Path2 = data["LauncherMenu"]["Path2"];
                string Title3 = data["LauncherMenu"]["Title3"];
                string Path3 = data["LauncherMenu"]["Path3"];
                string Title4 = data["LauncherMenu"]["Title4"];
                string Path4 = data["LauncherMenu"]["Path4"];
                string Title5 = data["LauncherMenu"]["Title5"];
                string Path5 = data["LauncherMenu"]["Path5"];
                string Title6 = data["LauncherMenu"]["Title6"];
                string Path6 = data["LauncherMenu"]["Path6"];
                string Title7 = data["LauncherMenu"]["Title7"];
                string Path7 = data["LauncherMenu"]["Path7"];

                this.Title0TextBox.Text = Title0;
                this.Title0PathTextBox.Text = Path0;
                this.Title1TextBox.Text = Title1;
                this.Title1PathTextBox.Text = Path1;
                this.Title2TextBox.Text = Title2;
                this.Title2PathTextBox.Text = Path2;
                this.Title3TextBox.Text = Title3;
                this.Title3PathTextBox.Text = Path3;
                this.Title4TextBox.Text = Title4;
                this.Title4PathTextBox.Text = Path4;
                this.Title5TextBox.Text = Title5;
                this.Title5PathTextBox.Text = Path5;
                this.Title6TextBox.Text = Title6;
                this.Title6PathTextBox.Text = Path6;
                this.Title7TextBox.Text = Title7;
                this.Title7PathTextBox.Text = Path7;

                //Show in Settings
                string Memory = data["ShowInSettings"]["Memory"];
                string Music = data["ShowInSettings"]["Music"];
                string XOnline = data["ShowInSettings"]["XOnline"];
                string Launcher = data["ShowInSettings"]["Launcher"];
                string Configuration = data["ShowInSettings"]["Configuration"];
                string Reboot = data["ShowInSettings"]["Reboot"];
                string Shutdown = data["ShowInSettings"]["Shutdown"];

                if (Memory == "true")
                {
                    this.ShowMemoryCheckBox.Checked = true;
                }
                if (Music == "true")
                {
                    this.ShowMusicCheckBox.Checked = true;
                }
                if (XOnline == "true")
                {
                    this.ShowXboxLiveCheckBox.Checked = true;
                }
                if (Launcher == "true")
                {
                    this.ShowLauncherCheckBox.Checked = true;
                }
                if (Configuration == "true")
                {
                    this.ShowUIXSettingsCheckBox.Checked = true;
                }
                if (Reboot == "true")
                {
                    this.ShowRebootCheckBox.Checked = true;
                }
                if (Shutdown == "true")
                {
                    this.ShowShutdownCheckBox.Checked = true;
                }

                //Quick Launch
                string QuickLaunchA = data["QuickLaunch"]["QuickLaunchA"];
                string QuickLaunchB = data["QuickLaunch"]["QuickLaunchB"];
                string QuickLaunchX = data["QuickLaunch"]["QuickLaunchX"];
                string QuickLaunchY = data["QuickLaunch"]["QuickLaunchY"];

                this.QuickLaunchATextBox.Text = QuickLaunchA;
                this.QuickLaunchBTextBox.Text = QuickLaunchB;
                this.QuickLaunchXTextBox.Text = QuickLaunchX;
                this.QuickLaunchYTextBox.Text = QuickLaunchY;

                ConfigLoaded = true;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error loading the config.\n" + ex.Message, "Config Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        private bool SaveUIXConfig(string ConfigFile)
        {
            try
            {
                if (ConfigLoaded || ConfigDownloaded)
                {
                    //Create a backup of the config
                    File.Copy(ConfigFile, ConfigFile + ".bak", true);
                }

                var parser = new FileIniDataParser();
                parser.Parser.Configuration.AssigmentSpacer = ""; //No spaces so that UIX can read the config settings
                IniData data = parser.ReadFile(ConfigFile);

                //Default
                data["default"]["MainOrbStyle"] = this.MainOrbStyleComboBox.Text;
                if (this.ShowInsigniaCheckBox.Checked)
                {
                    data["default"]["ShowInsignia"] = "true";
                }
                else
                {
                    data["default"]["ShowInsignia"] = "false";
                }
                data["default"]["LauncherOrbIcon"] = this.LauncherOrbIconComboBox.Text;
                data["default"]["ConfigPanelIcon"] = this.UIXSettingsIconComboBox.Text;
                if (this.AnimateMainMenuCheckBox.Checked)
                {
                    data["default"]["UseMainMenuAttract"] = "true";
                }
                else
                {
                    data["default"]["UseMainMenuAttract"] = "false";
                }
                if (this.UsetHcScreensaverCheckBox.Checked)
                {
                    data["default"]["UsetHcScreenSaver"] = "true";
                }
                else
                {
                    data["default"]["UsetHcScreenSaver"] = "false";
                }

                //Main Menu
                data["MainMenu"]["MainMenuItems"] = this.MainMenuItemsNumericUpDown.Text;
                data["MainMenu"]["Button1Text"] = this.Item1TextComboBox.Text;
                data["MainMenu"]["Button1Action"] = this.Item1ActionComboBox.Text;
                data["MainMenu"]["Button2Text"] = this.Item2TextComboBox.Text;
                data["MainMenu"]["Button2Action"] = this.Item2ActionComboBox.Text;
                data["MainMenu"]["Button3Text"] = this.Item3TextComboBox.Text;
                data["MainMenu"]["Button3Action"] = this.Item3ActionComboBox.Text;
                data["MainMenu"]["Button4Text"] = this.Item4TextComboBox.Text;
                data["MainMenu"]["Button4Action"] = this.Item4ActionComboBox.Text;
                data["MainMenu"]["ButtonYXAction"] = this.ButtonYXActionComboBox.Text;
                if (AdvancedModeCheckBox.Checked)
                {
                    data["MainMenu"]["AdvancedMode"] = "true";
                }
                else
                {
                    data["MainMenu"]["AdvancedMode"] = "false";
                }

                //Launcher Menu
                data["LauncherMenu"]["Title0"] = this.Title0TextBox.Text;
                data["LauncherMenu"]["Path0"] = this.Title0PathTextBox.Text;
                data["LauncherMenu"]["Title1"] = this.Title1TextBox.Text;
                data["LauncherMenu"]["Path1"] = this.Title1PathTextBox.Text;
                data["LauncherMenu"]["Title2"] = this.Title2TextBox.Text;
                data["LauncherMenu"]["Path2"] = this.Title2PathTextBox.Text;
                data["LauncherMenu"]["Title3"] = this.Title3TextBox.Text;
                data["LauncherMenu"]["Path3"] = this.Title3PathTextBox.Text;
                data["LauncherMenu"]["Title4"] = this.Title4TextBox.Text;
                data["LauncherMenu"]["Path4"] = this.Title4PathTextBox.Text;
                data["LauncherMenu"]["Title5"] = this.Title5TextBox.Text;
                data["LauncherMenu"]["Path5"] = this.Title5PathTextBox.Text;
                data["LauncherMenu"]["Title6"] = this.Title6TextBox.Text;
                data["LauncherMenu"]["Path6"] = this.Title6PathTextBox.Text;
                data["LauncherMenu"]["Title7"] = this.Title7TextBox.Text;
                data["LauncherMenu"]["Path7"] = this.Title7PathTextBox.Text;

                //Show in Settings
                if (ShowMemoryCheckBox.Checked)
                {
                    data["ShowInSettings"]["Memory"] = "true";
                }
                else
                {
                    data["ShowInSettings"]["Memory"] = "false";
                }
                if (ShowMusicCheckBox.Checked)
                {
                    data["ShowInSettings"]["Music"] = "true";
                }
                else
                {
                    data["ShowInSettings"]["Music"] = "false";
                }
                if (ShowXboxLiveCheckBox.Checked)
                {
                    data["ShowInSettings"]["XOnline"] = "true";
                }
                else
                {
                    data["ShowInSettings"]["XOnline"] = "false";
                }
                if (ShowLauncherCheckBox.Checked)
                {
                    data["ShowInSettings"]["Launcher"] = "true";
                }
                else
                {
                    data["ShowInSettings"]["Launcher"] = "false";
                }
                if (ShowUIXSettingsCheckBox.Checked)
                {
                    data["ShowInSettings"]["Configuration"] = "true";
                }
                else
                {
                    data["ShowInSettings"]["Configuration"] = "false";
                }
                if (ShowRebootCheckBox.Checked)
                {
                    data["ShowInSettings"]["Reboot"] = "true";
                }
                else
                {
                    data["ShowInSettings"]["Reboot"] = "false";
                }
                if (ShowShutdownCheckBox.Checked)
                {
                    data["ShowInSettings"]["Shutdown"] = "true";
                }
                else
                {
                    data["ShowInSettings"]["Shutdown"] = "false";
                }

                //Quick Launch
                data["QuickLaunch"]["QuickLaunchA"] = this.QuickLaunchATextBox.Text;
                data["QuickLaunch"]["QuickLaunchB"] = this.QuickLaunchBTextBox.Text;
                data["QuickLaunch"]["QuickLaunchX"] = this.QuickLaunchXTextBox.Text;
                data["QuickLaunch"]["QuickLaunchY"] = this.QuickLaunchYTextBox.Text;

                //Save the config file
                parser.WriteFile(ConfigFile, data);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error saving the config.\n" + ex.Message, "Config Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        private bool ConnectToXbox()
        {
            FTP = new FtpClient();

            //Load Xbox FTP settings from the config

            //If the user hasn't set an IP, prompt to set one
            if (string.IsNullOrEmpty(Properties.Settings.Default.IP))
            {
                using (FTPConfigForm FTPConfig = new FTPConfigForm())
                {
                    if (FTPConfig.ShowDialog() == DialogResult.OK)
                    {
                        MessageBox.Show("FTP settings configured.\nThey can be reconfigured by going to Settings > Configure FTP.", "FTP Configured", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            string XboxIP = Properties.Settings.Default.IP;
            string XboxUsername = Properties.Settings.Default.Username;
            string XboxPassword = Properties.Settings.Default.Password;
            int XboxPort = (int)Properties.Settings.Default.Port;

            //Connect to the Xbox
            FTP.Host = XboxIP;
            FTP.Credentials = new NetworkCredential(XboxUsername, XboxPassword);
            FTP.Port = XboxPort;

            this.toolStripStatusLabel1.Text = "Connecting to Xbox " + XboxIP + "...";

            try
            {
                FTP.Connect();
                return true;
            }
            catch (FtpAuthenticationException ex)
            {
                this.toolStripStatusLabel1.Text = "Invalid FTP login details for Xbox";
                MessageBox.Show("The FTP login details provided are invalid.\n" + ex.Message, "Invalid Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                this.toolStripStatusLabel1.Text = "Could not connect to Xbox";
                MessageBox.Show("Could not connect to the Xbox.\n" + ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        private bool DownloadFromXbox()
        {
            if (!ConnectToXbox())
            {
                return false;
            }

            this.toolStripStatusLabel1.Text = "Searching for the config...";

            //Download the config and read it
            try
            {
                if (FTP.DownloadFile("config.xbx", "/C/UIX Configs/config.xbx") == FtpStatus.Success)
                {
                    ConfigFile = "config.xbx";
                    ConfigDownloaded = true;
                    if (LoadUIXConfig(ConfigFile))
                    {
                        return true;
                    }
                }
                else
                {
                    this.toolStripStatusLabel1.Text = "No config found on Xbox";
                    MessageBox.Show("No config found on Xbox.", "Config Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (FtpException ex)
            {
                MessageBox.Show("Couldn't download the config file.\n" + ex.Message, "Couldn't Download", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        private bool UploadToXbox()
        {
            if (!ConnectToXbox())
            {
                return false;
            }

            if (string.IsNullOrEmpty(ConfigFile))
            {
                ConfigFile = "config.xbx";
            }

            if (!SaveUIXConfig(ConfigFile))
            {
                return false;
            }

            //Upload the config
            try
            {
                //For XBMC's FTP server
                FTP.Execute("CWD /C/");

                if (!FTP.DirectoryExists("/C/UIX Configs/"))
                {
                    FTP.CreateDirectory("/C/UIX Configs/");
                }

                FTP.UploadFile(ConfigFile, "/C/UIX Configs/config.xbx", FtpRemoteExists.Overwrite);
                return true;
            }
            catch (Exception ex)
            {
                this.toolStripStatusLabel1.Text = "Couldn't upload config to Xbox";
                MessageBox.Show("Couldn't upload the config to the Xbox.\n" + ex.Message, "Config Upload Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }
    }
}
