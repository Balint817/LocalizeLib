using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LocalizeLib.Properties
{
    [Obfuscation(Exclude = true, ApplyToMembers = true, Feature = "all")]
    internal static class MelonModInfo
    {
        public const string Name = "LocalizeLib";

        public const string Description = "Mod to make localizing easier (and some resource-utilities)";

        public const string Author = "PBalint817";

        public const string Version = "2.2.0";

        public const string DownloadLink = "";

        public const int Priority = 0;
    }
}
