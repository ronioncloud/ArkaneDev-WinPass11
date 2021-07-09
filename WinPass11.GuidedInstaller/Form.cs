using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using static winPass11_guided_install.AppraiserresHandler;
using winPass11_guided_install;

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
            //loadNext();
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
            DialogResult ask = MessageBox.Show("Are you sure you want to do this? You cannot undo this action", "Confirmation", MessageBoxButtons.YesNoCancel);
            Process _process;
            string sysRoot = Environment.GetEnvironmentVariable("SYSTEMROOT");
            if (ask.Equals(DialogResult.Yes))
            {
                //MessageBox.Show("Debug: User Clicked Yes");
                try
                {
                    // Download the Registry Twekas
                    WebClient downloader = new WebClient();
                    downloader.DownloadFile("https://raw.githubusercontent.com/project-winpass11/WinPass11.GuidedInstaller/main/WinPass11.GuidedInstaller/files/regtweaks.reg", $@"{sysRoot}\Temp\regtweaks.reg");
                    //MessageBox.Show("Successfully downloaded registry tweaks.", "WinPass11 Dialogue", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                // Create an error box if download fails
                catch
                {
                    MessageBox.Show("Failed to download the registry tweaks", "WinPass11 Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                try
                {
                    _process = Process.Start("regedit.exe", $@"/s {sysRoot}\Temp\regtweaks.reg"); // Location of the modified registry file
                    _process.WaitForExit();
                    Console.WriteLine("regedit exited with exit code of 0");
                }
                catch
                {
                    MessageBox.Show("Failed to apply the registry tweaks.", "WinPass11 Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                int ret = Utils.StartProcess("UsoClient.exe", "StartInteractiveScan", true);
                MessageBox.Show("Debug: Invoked System Update");
                AppraiserresHandler obj = new AppraiserresHandler();

                // Creating thread
                // Using thread class
                Thread thr = new Thread(new ThreadStart(obj.checkForExist));
                thr.Start();
            } 
            else
            {
                MessageBox.Show("Debug: User Canceled action");
            }
        }

        


        // Visual changes for every stage in the process of installation
        private void titleLabel_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
    }
}
