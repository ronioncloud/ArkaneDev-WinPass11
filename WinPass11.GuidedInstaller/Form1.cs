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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (progress)
            {
                case 0:
                    if (File.Exists("C:\\Users\\Default\\AppData\\Local\\Microsoft\\Windows\\WSUS\\setupconfig.ini")) // clean up old setup
                        File.Delete("C:\\Users\\Default\\AppData\\Local\\Microsoft\\Windows\\WSUS\\setupconfig.ini");
                    break;
                case 1:
                    Process _process;
                    _process = Process.Start("regedit.exe", "/s files\\regtweaks.reg"); // Location of the modified registry file
                    _process.WaitForExit();
                    Console.WriteLine("Exit");
                    break;
                case 3:
                    if (File.Exists("C:\\$WINDOWS.~BT\\Sources\\AppraiserRes.dll"))
                    { // clean up old setup
                        File.Delete("C:\\$WINDOWS.~BT\\Sources\\AppraiserRes.dll");
                    } else
                    {
                        MessageBox.Show("Hey! You pressed the button to early, it seems as if the installer hasn't downloaded the file we need to replace yet... Try again after reading the directions");
                    }
                    try
                    {
                        WebClient downloader = new WebClient();
                        downloader.DownloadFile("https://github.com/CodeProf14/Fix-TPM/blob/main/Fix%20TPM/appraiserres.dll?raw=true", "C:\\$WINDOWS.~BT\\Sources\\AppraiserRes.dll");
                    } catch
                    {
                        MessageBox.Show("Failed to download file :(");
                    }
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label3_Click(object sender, EventArgs e)
        {

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
        public void loadNext()
        {
            switch (progress)
            {
                case 0:
                    richTextBox1.Text = "If you have previously installed Windows 11 using WinPass11 or attempted to, you should probably click this button, if not, it doesn't hurt to click it regardless.";
                    label1.Text = "Clean Previous Installations >";
                    pictureBox1.ImageLocation = "https://cdn.discordapp.com/attachments/859934909607313428/859962905813581884/Updates.png";
                    button1.Text = "Clean";
                    button1.Enabled = true;
                    break;
                case 1:
                    label1.Text = "Apply registry tweaks >";
                    pictureBox1.ImageLocation = "https://cdn.discordapp.com/attachments/859934909607313428/859964793774145536/Logo.png";
                    button1.Text = "Apply";
                    button1.Enabled = true;
                    richTextBox1.Text = "This stage will apply our registry tweaks. The tweaks applied here will bypass the TPM 2.0 and Secure Boot checks. Before you apply them, ensure you are at least in the Release channel. Restart if necessary.";
                    break;
                case 2:
                    button1.Enabled = false;
                    label1.Text = "Refer to image and box for directions";
                    pictureBox1.ImageLocation = "https://media.discordapp.net/attachments/859570021599412236/859934248541356112/unknown.png";
                    richTextBox1.Text = "Now we're ready to update, this is simple. Go to Settings > Windows Update > Windows Insider Program.\nEnsure you are now in the Dev Channel. Scroll up and click Windows Update in the left bar and click check now, if everything went well, you should see downloading Windows 11 Insider Preview, but dont leave just yet, we still need to bypass the requirements!";
                    break;
                case 3:
                    button1.Enabled = true;
                    button1.Text = "Replace";
                    label1.Text = "Replace appraiserres.dll >";
                    pictureBox1.ImageLocation = "https://media.discordapp.net/attachments/859934909607313428/859947549775495168/ThisButBordered.png";
                    richTextBox1.Text = "Next up, you will have to wait for the install to fail. Once the window that says install failed due to TPM 2.0, close that out and click Replace.";
                    button3.Text = "Next >";
                    break;
                case 4:
                    button1.Enabled = false;
                    label1.Text = "Refer to image and box for directions";
                    pictureBox1.ImageLocation = "https://cdn.discordapp.com/attachments/859934909607313428/859960424090173460/unknown.png";
                    richTextBox1.Text = "This is the last step! All that needs to be done is for you to go back to the update screen and click \"Check for Updates\", \"Fix issues\", or whatever there is in place of the button. (There is a chance the download just continues) After this is should work and it is safe to close this application!";
                    button3.Text = "Finish";
                    break;
                case 5:
                    Application.Exit();
                    break;
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
