using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace winPass11_guided_install
{
    public partial class Form1 : Form
    {
        Page mCurrentPage = Page.Welcome;

        public Form1()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private void ShowMessageBox(string msg, MessageBoxType type)
        {
            string title = Strings.Titles.Success;
            MessageBoxIcon icon = MessageBoxIcon.Information;
            if (type == MessageBoxType.Error)
            {
                title = Strings.Titles.Error;
                icon = MessageBoxIcon.Error;
            }

            MessageBox.Show(msg, title, MessageBoxButtons.OK, icon);
        }

        private void ActionButtonClick(object sender, EventArgs e)
        {
            string sysDrive = Environment.GetEnvironmentVariable("SYSTEMDRIVE");

            switch (mCurrentPage)
            {
                case Page.Welcome:
                    Utils.OpenUrlInBrowser(Constants.Url.GitRepo);
                    //Console.WriteLine("process exited with exit code of {0}", _process.ExitCode);
                    break;

                case Page.CleanPrev:
                    string setupFile = Path.Combine(sysDrive, Constants.OldSetupConfigPath);
                    if (File.Exists(setupFile))
                    {
                        // Try to delete old setups
                        try
                        {
                            File.Delete(setupFile);
                            ShowMessageBox(Strings.Body.CleanedPastSuccess, MessageBoxType.Information);
                        }
                        catch
                        {
                            ShowMessageBox(Strings.Body.CleanedPastFailed, MessageBoxType.Error);
                        }
                    }
                    else
                    {
                        // Create an message box if no old setups exist
                        ShowMessageBox(Strings.Body.NoPastDetected, MessageBoxType.Information);
                    }
                    break;

                case Page.RegistryTweak:
                    string regTweaksDownloadPath = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), "regtweaks.reg");
                    try
                    {
                        // Download the Registry Tweaks
                        WebClient downloader = new WebClient();
                        downloader.DownloadFile(Constants.Url.RegTweaks, regTweaksDownloadPath);
                        ShowMessageBox(string.Format(Strings.Body.DownloadSuccess, "registry tweaks"), MessageBoxType.Information);
                    }
                    catch
                    {
                        ShowMessageBox(string.Format(Strings.Body.DownloadFailed, "registry tweaks"), MessageBoxType.Error);
                        break;      // No point trying to launch if download failed
                    }

                    try
                    {
                        int ret = Utils.StartProcess("regedit.exe", $"/s {regTweaksDownloadPath}", true);
                        Console.WriteLine("regedit exited with exit code of {0}", ret);
                    }
                    catch
                    {
                        ShowMessageBox(Strings.Body.RegistryCantApply, MessageBoxType.Error);
                    }

                    break;

                case Page.UpdateInit:
                    try
                    {
                        Utils.OpenUrlInBrowser(Constants.Url.WinUpdate);
                    }
                    catch
                    {
                        ShowMessageBox(Strings.Body.WinUpdateOpenFailed, MessageBoxType.Error);
                    }
                    break;

                case Page.ReplaceDll:
                    string appraiserDllSrcPath = Path.Combine(sysDrive, @"$WINDOWS.~BT\Sources\AppraiserRes.dll");
                    if (File.Exists(appraiserDllSrcPath))
                    {
                        try
                        {
                            File.Delete(appraiserDllSrcPath);
                        }
                        catch
                        {
                            ShowMessageBox(string.Format(Strings.Body.DeleteFailed, "AppraiserRes.dll"), MessageBoxType.Error);
                        }
                    }
                    else // Make an error box if the required DLL file doesn't exist yet
                    {
                        ShowMessageBox(Strings.Body.InstallerNotDownloaded, MessageBoxType.Information);
                    }

                    try
                    {
                        WebClient downloader = new WebClient();
                        downloader.DownloadFile(Constants.Url.AppraiserRes, Path.Combine(sysDrive, @"WINDOWS.~BT\Sources\AppraiserRes.dll"));
                    }
                    catch // Create an error box if download fails
                    {
                        ShowMessageBox(string.Format(Strings.Body.DownloadFailed, "AppraiserRes.dll"), MessageBoxType.Error);
                    }
                    break;

                case Page.UpdateFinal:
                    try
                    {
                        Utils.OpenUrlInBrowser(Constants.Url.WinUpdate);
                    }
                    catch
                    {
                        ShowMessageBox(Strings.Body.WinUpdateOpenFailed, MessageBoxType.Error);
                    }
                    break;
            }
        }

        private void ExitButtonClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadNext();
        }

        private void NextButtonClick(object sender, EventArgs e)
        {
            mCurrentPage++;
            loadNext();
        }

        private void BackButtonClick(object sender, EventArgs e)
        {
            if (mCurrentPage > Page.Welcome)
            {
                mCurrentPage--;
            }

            loadNext();
        }

        // Visual changes for every stage in the process of installation
        public void loadNext()
        {
            button4.Enabled = mCurrentPage > Page.Welcome;
;
            switch (mCurrentPage)
            {
                case Page.Welcome:
                    richTextBox1.Text = "If you downloaded this from any other source than the GitHub repository or GitHub io page, there is a possibility of the program being infected with malware or outdated. We are not responsible for damage to your system, USE AT YOUR OWN RISK!";
                    label1.Text = "Go to GitHub repository >";
                    pictureBox1.ImageLocation = "https://cdn.discordapp.com/attachments/859934909607313428/859964793774145536/Logo.png";
                    button1.Text = "GitHub";
                    break;
                case Page.CleanPrev:
                    richTextBox1.Text = "If you have previously attempted to install Windows 11 with WinPass11, you should probably click clean. If not, it doesn't hurt to click it regardless.";
                    label1.Text = "Clean Previous Installations >";
                    pictureBox1.ImageLocation = "https://cdn.discordapp.com/attachments/859934909607313428/859962905813581884/Updates.png";
                    button1.Text = "Clean";
                    break;
                case Page.RegistryTweak:
                    label1.Text = "Apply registry tweaks >";
                    pictureBox1.ImageLocation = "https://cdn.discordapp.com/attachments/859934909607313428/859964793774145536/Logo.png";
                    button1.Text = "Apply";
                    richTextBox1.Text = "This stage will apply our registry tweaks. The tweaks applied here will bypass the TPM 2.0 and Secure Boot checks. Before you apply them, ensure you are at least in the Release Preview channel. Restart if necessary.";
                    break;
                case Page.UpdateInit:
                    label1.Text = "Update Settings >";
                    pictureBox1.ImageLocation = "https://cdn.discordapp.com/attachments/859570021599412236/859934248541356112/unknown.png";
                    richTextBox1.Text = "Now we're ready to update! Click the button to go to the Settings app and click check now, if everything went well, you should see downloading Windows 11 Insider Preview, but dont leave just yet, we still need to bypass the requirements!";
                    button1.Text = "Settings";
                    break;
                case Page.ReplaceDll:
                    button1.Text = "Replace";
                    label1.Text = "Replace AppraiserRes.dll >";
                    pictureBox1.ImageLocation = "https://cdn.discordapp.com/attachments/859934909607313428/860124553342353418/unknown.png";
                    richTextBox1.Text = "Next up, you will have to wait for the install to fail. Once the installation window says install failed due to TPM 2.0 and/or Secure Boot, close that and click Replace.";
                    button3.Text = "Next >";
                    break;
                case Page.UpdateFinal:
                    label1.Text = "Update Settings >";
                    pictureBox1.ImageLocation = "https://cdn.discordapp.com/attachments/859934909607313428/859960424090173460/unknown.png";
                    richTextBox1.Text = "This is the last step! Go back to the update screen and click \"Check for Updates\", \"Fix issues\", or whatever there is in place of the button. (There is a chance the download just continues) After this, it should work and it is safe to close this application!";
                    button3.Text = "Finish";
                    button1.Text = "Settings";
                    break;
                case Page.Last:
                    Application.Exit();
                    break;
            }
        }
    }
}
