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
        int progress = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("ms-settings:windowsupdate");
            String sysDrive = Environment.GetEnvironmentVariable("SYSTEMDRIVE");
            String sysRoot = Environment.GetEnvironmentVariable("SYSTEMROOT");
            switch (progress)
            {
                // Clean up old setup
                case 0:
                    // Delete old setups if they exist
                    if (File.Exists($@"{sysDrive}\Users\Default\AppData\Local\Microsoft\Windows\WSUS\setupconfig.ini"))
                    {
                        // Try to delete old setups
                        try
                        {
                            File.Delete($@"{sysDrive}\Users\Default\AppData\Local\Microsoft\Windows\WSUS\setupconfig.ini");
                            MessageBox.Show("Successfully cleaned up past setups.", "WinPass11 Dialogue", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        // Create an error box if download fails
                        catch
                        {
                            MessageBox.Show("Failed to clean up past setup.", "WinPass11 Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        // Create an message box if no old setups exist
                        MessageBox.Show("No past setups detected.", "WinPass11 Dialogue", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;
                case 1:
                    Process _process;
                    try
                    {
                        // Download the Registry Twekas
                        WebClient downloader = new WebClient();
                        downloader.DownloadFile("https://raw.githubusercontent.com/project-winpass11/WinPass11.GuidedInstaller/main/WinPass11.GuidedInstaller/files/regtweaks.reg", $@"{sysRoot}\Temp\regtweaks.reg");
                        MessageBox.Show("Successfully downloaded registry tweaks.", "WinPass11 Dialogue", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                    break;
                case 2:
                    try
                    {
                        Process.Start(sInfo);
                    }
                    catch
                    {
                        MessageBox.Show("Failed to open ms-settings:windowsupdate", "WinPass11 Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                case 3:
                    // Delete the old DLL file
                    if (File.Exists($@"{sysDrive}\$WINDOWS.~BT\Sources\AppraiserRes.dll"))
                    {
                        try
                        {
                            File.Delete($@"{sysDrive}\$WINDOWS.~BT\Sources\AppraiserRes.dll");
                        }
                        catch
                        {
                            MessageBox.Show("Failed to delete setup file: AppraiserRes.dll", "WinPass11 Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    // Make an error box if the required DLL file doesn't exist yet
                    {
                        MessageBox.Show("The installer hasn't downloaded the file we need to replace yet.", "WinPass11 Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    try
                    {
                        WebClient downloader = new WebClient();
                        downloader.DownloadFile("https://github.com/CodeProf14/Fix-TPM/blob/main/Fix%20TPM/appraiserres.dll?raw=true", $@"{sysDrive}\$WINDOWS.~BT\Sources\AppraiserRes.dll");
                    }
                    // Create an error box if download fails
                    catch
                    {
                        MessageBox.Show("Failed to download patched file: AppraiserRes.dll", "WinPass11 Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                case 4:
                    try
                    {
                        Process.Start(sInfo);
                    }
                    catch
                    {
                        MessageBox.Show("Failed to open ms-settings:windowsupdate", "WinPass11 Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Close the application
            Application.Exit();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            richTextBox1.ReadOnly = true;
            loadNext();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button4.Enabled = true;
            progress++;
            loadNext();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (progress >= 0)
            {
                progress--;
            }
            if (progress == 0)
            {
                button4.Enabled = false;
            }
            loadNext();
        }

        // Visual changes for every stage in the process of installation
        public void loadNext()
        {
            switch (progress)
            {
                case 0:
                    richTextBox1.Text = "If you have previously attempted to install Windows 11 with WinPass11, you should probably click clean. If not, it doesn't hurt to click it regardless.";
                    label1.Text = "Clean Previous Installations >";
                    pictureBox1.ImageLocation = "https://cdn.discordapp.com/attachments/859934909607313428/859962905813581884/Updates.png";
                    button1.Text = "Clean";
                    break;
                case 1:
                    label1.Text = "Apply registry tweaks >";
                    pictureBox1.ImageLocation = "https://cdn.discordapp.com/attachments/859934909607313428/859964793774145536/Logo.png";
                    button1.Text = "Apply";
                    richTextBox1.Text = "This stage will apply our registry tweaks. The tweaks applied here will bypass the TPM 2.0 and Secure Boot checks. Before you apply them, ensure you are at least in the Release Preview channel. Restart if necessary.";
                    break;
                case 2:
                    label1.Text = "Update Settings >";
                    pictureBox1.ImageLocation = "https://cdn.discordapp.com/attachments/859570021599412236/859934248541356112/unknown.png";
                    richTextBox1.Text = "Now we're ready to update! Click the button to go to the Settings app and click check now, if everything went well, you should see downloading Windows 11 Insider Preview, but dont leave just yet, we still need to bypass the requirements!";
                    button1.Text = "Settings";
                    break;
                case 3:
                    button1.Text = "Replace";
                    label1.Text = "Replace appraiserres.dll >";
                    pictureBox1.ImageLocation = "https://cdn.discordapp.com/attachments/859934909607313428/860124553342353418/unknown.png";
                    richTextBox1.Text = "Next up, you will have to wait for the install to fail. Once the installation window says install failed due to TPM 2.0 and/or Secure Boot, close that and click Replace.";
                    button3.Text = "Next >";
                    break;
                case 4:
                    label1.Text = "Update Settings >";
                    pictureBox1.ImageLocation = "https://cdn.discordapp.com/attachments/859934909607313428/859960424090173460/unknown.png";
                    richTextBox1.Text = "This is the last step! Go back to the update screen and click \"Check for Updates\", \"Fix issues\", or whatever there is in place of the button. (There is a chance the download just continues) After this, it should work and it is safe to close this application!";
                    button3.Text = "Finish";
                    button1.Text = "Settings";
                    break;
                case 5:
                    Application.Exit();
                    break;
            }
        }
    }
}
