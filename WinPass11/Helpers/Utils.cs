using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinPass11.Helpers
{
    class Utils
    {
        public static void DownloadFile(string url, string dest, bool overwrite = false)
        {
            if (File.Exists(dest) && !overwrite)
                return;

            string dirName = Path.GetDirectoryName(dest);
            if (!Directory.Exists(dirName))
                Directory.CreateDirectory(dirName);

            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(url, dest);
            }
        }

        public static int StartProcess(string exePath, string args, bool waitForExit = true)
        {
            Process process = new Process();
            process.StartInfo.FileName = exePath;
            process.StartInfo.Arguments = args;

            process.Start();

            if (waitForExit)
            {
                process.WaitForExit();
                return process.ExitCode;
            }
            else
                return 0;
        }

        public static void ShowMessageBox(string msg, MessageBoxType type)
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
    }
}
