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

namespace WinPass11.Helpers
{
    class Handlers
    {
        public class AppraiserRes
        {
            public void checkForExist()
            {
                while (!File.Exists(Constants.Path.AppraiserRes))
                {
                    Thread.Sleep(500);
                }
                Thread.Sleep(3000); // Wait to make sure file is downloaded
                while (IsFileLocked(new FileInfo(Constants.Path.AppraiserRes)))
                {
                    foreach (var process in Process.GetProcessesByName("SetupHost.exe"))
                    {
                        process.Kill();
                    }
                }

                if (File.Exists(Constants.Path.AppraiserRes))
                {
                    try
                    {
                        File.Delete(Constants.Path.AppraiserRes);
                    }
                    catch
                    {
                        Utils.ShowMessageBox(string.Format(Strings.Body.DeleteFailed, "AppraiserRes.dll"), MessageBoxType.Error);
                    }
                }
                else
                // Make an error box if the required DLL file doesn't exist yet
                {
                    Utils.ShowMessageBox(string.Format(Strings.Body.FileNotDownloaded, "AppraiserRes.dll"), MessageBoxType.Error);
                }
                try
                {
                    Utils.DownloadFile(Constants.Url.AppraiserRes, Constants.Path.AppraiserRes);
                }
                // Create an error box if download fails
                catch
                {
                    Utils.ShowMessageBox(string.Format(Strings.Body.DownloadFailed, "AppraiserRes.dll"), MessageBoxType.Error);
                }
            }
            public bool IsFileLocked(FileInfo file)
            {
                var stream = (FileStream)null;
                try
                {
                    stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                }
                catch (IOException)
                {
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
}
