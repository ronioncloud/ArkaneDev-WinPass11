using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinPass11.GuidedInstaller
{
    public enum MessageBoxType
    {
        Information,
        Error
    }

    public enum Page
    {
        Welcome = 0,
        CleanPrev,
        RegistryTweak,
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
            public const string GitRepo         = "https://github.com/project-winpass11/WinPass11.GuidedInstaller";
            public const string RegTweaks       = "https://raw.githubusercontent.com/project-winpass11/WinPass11.GuidedInstaller/main/WinPass11.GuidedInstaller/files/regtweaks.reg";
            public const string AppraiserRes    = "https://github.com/CodeProf14/Fix-TPM/blob/main/Fix%20TPM/appraiserres.dll?raw=true";
            public const string WinUpdate       = "ms-settings:windowsupdate";
        }
    }
}
