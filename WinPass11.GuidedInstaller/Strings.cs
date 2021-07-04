using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winPass11_guided_install
{
    class Strings
    {
        public class Titles
        {
            private const string Prefix         = "WinPass11";
            public  const string Success        = Prefix + " Success";
            public  const string Error          = Prefix + " Error";
        }

        public class Body
        {
            public const string DeleteFailed            = "Failed to delete {0}";
            public const string DownloadFailed          = "Failed to download {0}";
            public const string DownloadSuccess         = "Successfully downloaded {0}";

            public const string CleanedPastSuccess      = "Successfully cleaned past setups";
            public const string CleanedPastFailed       = "Failed to clean past setups";
            public const string NoPastDetected          = "No past setups detected";

            public const string RegistryCantApply       = "Failed to apply registry tweaks";

            public const string WinUpdateOpenFailed     = "Failed to open Windows Update window";

            public const string InstallerNotDownloaded  = "Windows update hasn't downloaded the file required to modify";
        }

    }
}
