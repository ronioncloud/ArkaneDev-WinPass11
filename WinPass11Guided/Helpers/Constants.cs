using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinPass11Guided.Helpers
{
    public enum MessageBoxType
    {
        Information,
        Error
    }

    public enum Page
    {
        Warning = 0,
        CleanPrev,
        ApplyTweaks,
        UpdateInit,
        ReplaceDll,
        UpdateFinal,
        Last
    }

    public static class Constants
    {
        public const string OldSetupConfigPath  = @"Users\Default\AppData\Local\Microsoft\Windows\WSUS\setupconfig.ini";

        public static class Url
        {
            public const string GitHubRepo      = "https://github.com/ArkaneDev/WinPass11";
            public const string RegTweaks       = "https://raw.githubusercontent.com/ArkaneDev/WinPass11/main/WinPass11/files/regtweaks.reg";
            public const string AppraiserRes    = "https://raw.github.com/CodeProf14/Fix-TPM/blob/main/Fix%20TPM/appraiserres.dll";
            public const string WinUpdate       = "ms-settings:windowsupdate";
        }
    }
}
