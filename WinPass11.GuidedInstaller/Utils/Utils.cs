using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WinPass11.GuidedInstaller
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
        }

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

        private static Dictionary<string, Image> mImagesDict = new Dictionary<string, Image>();
        public static Image GetDownloadedImage(string url)
        {
            if (mImagesDict.ContainsKey(url))
                return mImagesDict[url];

            Uri uri = new Uri(url);
            string fileName = Path.GetFileName(uri.AbsolutePath);
            string dest = Path.Combine(Form.mTempWorkingDir, fileName);

            if (File.Exists(dest))
                dest = Path.Combine(Form.mTempWorkingDir, $"{Path.GetTempFileName()}.png");            //TODO: Should fix png hardcode

            try
            {
                DownloadFile(url, dest);
                mImagesDict.Add(url, Image.FromFile(dest));
                return mImagesDict[url];
            }
            catch
            {
                return null;
            }
        }
    }
}
