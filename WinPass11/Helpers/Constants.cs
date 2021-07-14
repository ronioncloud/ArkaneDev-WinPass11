using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinPass11.Helpers
{
    public enum MessageBoxType
    {
        Information,
        Error
    }

    public static class Constants
    {
        
        public class Url
        {
            public static readonly string RegTweaks       = "https://raw.githubusercontent.com/ArkaneDev/WinPass11/main/WinPass11/files/regtweaks.reg";
            public static readonly string AppraiserRes    = "https://raw.github.com/CodeProf14/Fix-TPM/blob/main/Fix%20TPM/appraiserres.dll";
        }

        public class Path
        {
            public static readonly string SysDrive        = Environment.GetEnvironmentVariable("SYSTEMDRIVE");
            public static readonly string AppraiserRes    = $@"{SysDrive}\$WINDOWS.~BT\Sources\AppraiserRes.dll";
        }
    }
}
