using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinPass11.GuidedInstaller
{
    class Strings
    {
        public class Titles
        {
            private static readonly string Prefix       = "WinPass11";
            public  static readonly string Information  = $"{Prefix} Information";
            public  static readonly string Error        = $"{Prefix} Error";
        }

        public class Body
        {
            public static readonly string DeleteFailed           = "Failed to delete {0}";

            public static readonly string DownloadFailed         = "Failed to download {0}";
            public static readonly string DownloadSuccess        = "Successfully downloaded {0}";

            public static readonly string FileNotDownloaded      = "Windows Update hasn't downloaded the required file";

            public static readonly string ReplaceFailed          = "Failed to replace {0}";
            public static readonly string ReplaceSuccess         = "Successfully replaced {0}";

            public static readonly string RegApplyFailed         = "Failed to apply registry tweaks";
            public static readonly string RegApplySuccess        = "Successfully applied registry tweaks";

            public static readonly string SetupCleanFailed       = "Failed to clean past setups";
            public static readonly string SetupCleanSuccess      = "Successfully cleaned past setups";
            public static readonly string NoSetupsDetected       = "No past setups detected";

            public static readonly string WinUpdateOpenFailed    = "Failed to open Windows Update window";
        }
    }
}
