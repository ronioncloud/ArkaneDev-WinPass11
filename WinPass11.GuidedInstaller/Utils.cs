using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winPass11_guided_install
{
    public class Utils
    {
        public static void OpenUrlInBrowser(string url)
        {
            Process.Start(url);
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

            ProcessStartInfo info = new ProcessStartInfo("");
        }
    }
}
