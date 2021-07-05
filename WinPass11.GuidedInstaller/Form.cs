using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinPass11.GuidedInstaller
{
    public partial class Form : System.Windows.Forms.Form
    {
        Page mCurrentPage = Page.Warning;
        public static string mTempWorkingDir = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), "WinPass11");

        public Form()
        {
            InitializeComponent();
            InitWorkingFolder();

            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void ShowMessageBox(string msg, MessageBoxType type)
        {
            string title = Strings.Titles.Information;
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
                case Page.Warning:
                    Utils.OpenUrlInBrowser(Constants.Url.GitHubRepo);
                    break;

                case Page.CleanPrev:
                    string setupFile = Path.Combine(sysDrive, Constants.OldSetupConfigPath);
                    if (File.Exists(setupFile))
                    {
                        // Try to delete old setups
                        try
                        {
                            File.Delete(setupFile);
                            ShowMessageBox(Strings.Body.SetupCleanSuccess, MessageBoxType.Information);
                        }
                        catch
                        {
                            ShowMessageBox(Strings.Body.SetupCleanFailed, MessageBoxType.Error);
                        }
                    }
                    else
                    {
                        // Create an message box if no old setups exist
                        ShowMessageBox(Strings.Body.NoSetupsDetected, MessageBoxType.Information);
                    }
                    break;

                case Page.ApplyTweaks:
                    string regTweaksDownloadPath = $@"{mTempWorkingDir}\regtweaks.reg";
                    try
                    {
                        // Download the Registry Tweaks
                        Utils.DownloadFile(Constants.Url.RegTweaks, regTweaksDownloadPath);
                        ShowMessageBox(string.Format(Strings.Body.DownloadSuccess, "registry tweaks"), MessageBoxType.Information);
                    }
                    catch
                    {
                        ShowMessageBox(string.Format(Strings.Body.DownloadFailed, "registry tweaks"), MessageBoxType.Error);
                        break;
                    }

                    try
                    {
                        int ret = Utils.StartProcess("regedit.exe", $"/s {regTweaksDownloadPath}", true);
                        Console.WriteLine("regedit exited with exit code of {0}", ret);
                        ShowMessageBox(Strings.Body.RegApplySuccess, MessageBoxType.Information);
                    }
                    catch
                    {
                        ShowMessageBox(Strings.Body.RegApplyFailed, MessageBoxType.Error);
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
                    string appraiserResPath = $@"{sysDrive}\$WINDOWS.~BT\Sources\AppraiserRes.dll";
                    if (File.Exists(appraiserResPath))
                    {
                        try
                        {
                            File.Delete(appraiserResPath);
                            Utils.DownloadFile(Constants.Url.AppraiserRes, appraiserResPath, true);
                            ShowMessageBox(string.Format(Strings.Body.ReplaceSuccess, "AppraiserRes.dll"), MessageBoxType.Information);
                        }
                        catch
                        {
                            ShowMessageBox(string.Format(Strings.Body.ReplaceFailed, "AppraiserRes.dll"), MessageBoxType.Error);
                        }
                    }
                    else
                        // Make an error box if the required DLL file doesn't exist yet
                        ShowMessageBox(Strings.Body.FileNotDownloaded, MessageBoxType.Information);
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

        private void InitWorkingFolder()
        {
            try
            {
                if (Directory.Exists(mTempWorkingDir))
                    Directory.Delete(mTempWorkingDir, true);
                Directory.CreateDirectory(mTempWorkingDir);
            }
            catch { }
        }

        private void NextButtonClick(object sender, EventArgs e)
        {
            mCurrentPage++;
            loadNext();
        }

        private void BackButtonClick(object sender, EventArgs e)
        {
            if (mCurrentPage > Page.Warning)
            {
                mCurrentPage--;
            }

            loadNext();
        }

        // Visual changes for every stage in the process of installation
        public void loadNext()
        {
            backButton.Enabled = mCurrentPage > Page.Warning;
;
            switch (mCurrentPage)
            {
                case Page.Warning:
                    descriptionBox.Text = "If you downloaded this from any other source than the GitHub repository or GitHub io page, there is a possibility of the program being infected with malware or outdated. We are not responsible for damage to your system. Please use at your own risk!";
                    buttonLabel.Text = "Go to GitHub repository >";
                    pictureBox.Image = Utils.GetDownloadedImage("https://cdn.discordapp.com/attachments/859934909607313428/859964793774145536/Logo.png");
                    actionButton.Text = "GitHub";
                    break;
                case Page.CleanPrev:
                    descriptionBox.Text = "If you have previously attempted to install Windows 11 with WinPass11, you should click clean now. If not, it won't affect the rest of the process installation.";
                    buttonLabel.Text = "Clean Previous Installations >";
                    pictureBox.Image = Utils.GetDownloadedImage("https://cdn.discordapp.com/attachments/859934909607313428/859962905813581884/Updates.png");
                    actionButton.Text = "Clean";
                    break;
                case Page.ApplyTweaks:
                    buttonLabel.Text = "Apply registry tweaks >";
                    pictureBox.Image = Utils.GetDownloadedImage("https://cdn.discordapp.com/attachments/859934909607313428/859964793774145536/Logo.png");
                    actionButton.Text = "Apply";
                    descriptionBox.Text = "This stage will apply our registry tweaks. The tweaks applied here will bypass the TPM 2.0 and Secure Boot checks. Before you apply them, please ensure you are in the Release Preview channel. Restart if necessary.";
                    break;
                case Page.UpdateInit:
                    buttonLabel.Text = "Update Settings >";
                    pictureBox.Image = Utils.GetDownloadedImage("https://cdn.discordapp.com/attachments/859570021599412236/859934248541356112/unknown.png");
                    descriptionBox.Text = "Now we're ready to update. Click the button to go to the Settings app and click check now, if everything went well, you should see downloading Windows 11 Insider Preview, but dont leave just yet, we still need to bypass the requirements.";
                    actionButton.Text = "Settings";
                    break;
                case Page.ReplaceDll:
                    actionButton.Text = "Replace";
                    buttonLabel.Text = "Replace AppraiserRes.dll >";
                    pictureBox.Image = Utils.GetDownloadedImage("https://cdn.discordapp.com/attachments/859934909607313428/860124553342353418/unknown.png");
                    descriptionBox.Text = "Next up, you will have to wait for the install to fail. Once the installation window says install failed due to TPM 2.0 and/or Secure Boot, close that and click Replace.";
                    nextButton.Text = "Next >";
                    break;
                case Page.UpdateFinal:
                    buttonLabel.Text = "Update Settings >";
                    pictureBox.Image = Utils.GetDownloadedImage("https://cdn.discordapp.com/attachments/859934909607313428/859960424090173460/unknown.png");
                    descriptionBox.Text = "This is the last step! Go back to the update screen and click \"Check for Updates\" or \"Fix issues\". (There is a chance the download just continues or restarts) After this, it should work and it is safe to close this application!";
                    nextButton.Text = "Finish";
                    actionButton.Text = "Settings";
                    break;
                case Page.Last:
                    Application.Exit();
                    break;
            }
        }
    }
}
