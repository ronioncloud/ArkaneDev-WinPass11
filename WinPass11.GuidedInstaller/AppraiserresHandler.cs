using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinPass11.GuidedInstaller;

namespace winPass11_guided_install
{
    class AppraiserresHandler
    {
        public void checkForExist()
        {
            while(!File.Exists($@"C:\$WINDOWS.~BT\Sources\AppraiserRes.dll"))
            {
                Thread.Sleep(500);
            }
            Thread.Sleep(3000); // Wait to make sure file is downloaded
            while(IsFileLocked(new FileInfo($@"C:\$WINDOWS.~BT\Sources\AppraiserRes.dll")))
            {
                foreach (var process in Process.GetProcessesByName("SetupHost.exe")) // FREEZE YOU ARE UNDER ARREST
                {
                    process.Kill(); // Capital punishment 
                } // File (hostage) should be free now
            } // ^^ repeat if it didn't work
              // Delete the old DLL file
            if (File.Exists($@"C:\$WINDOWS.~BT\Sources\AppraiserRes.dll"))
            {
                try
                {
                    File.Delete($@"C:\$WINDOWS.~BT\Sources\AppraiserRes.dll"); // If you kill the hostage then you dont have to worry about it!
                }
                catch
                {
                    MessageBox.Show("Failed to delete setup file: AppraiserRes.dll", "WinPass11 Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            // Make an error box if the required DLL file doesn't exist yet
            {
                MessageBox.Show("The installer hasn't downloaded the file we need to replace yet.", "WinPass11 Error", MessageBoxButtons.OK, MessageBoxIcon.Information); // Wha- did we murder the hostage that quick?
            }
            try
            {
                WebClient downloader = new WebClient(); // Create VIP transport for the hostage
                downloader.DownloadFile("https://github.com/CodeProf14/Fix-TPM/blob/main/Fix%20TPM/appraiserres.dll?raw=true", $@"C:\$WINDOWS.~BT\Sources\AppraiserRes.dll"); // Replace Hostage with puppet
            }
            // Create an error box if download fails
            catch
            {
                MessageBox.Show("Failed to download patched file: AppraiserRes.dll", "WinPass11 Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            int ret = Utils.StartProcess("UsoClient.exe", "StartInteractiveScan", true); // Once our grand scheme is complete invoke sysupdate to call the authorities and hide the evidence


        }
        public bool IsFileLocked(FileInfo file) // Check if file is locked by the updater
        {
            var stream = (FileStream)null;
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException ex)
            {
                //handle the exception your way
                return true;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return false;
        }
    }
}
