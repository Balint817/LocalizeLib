using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LocalizeLib.Properties
{
    [Obfuscation(Exclude = true, ApplyToMembers = true, Feature = "all")]
    internal static class MelonGameInfo
    {
        public const string Name = "MuseDash";

        public const string Developer = "PeroPeroGames";

        public const string Version = null; // IsUniversal => string.IsNullOrEmpty(Version)
    }
}
