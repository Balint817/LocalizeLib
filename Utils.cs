using Il2CppAccount;
using Il2CppAssets.Scripts.PeroTools.Commons;
using Il2CppAssets.Scripts.PeroTools.Nice.Datas;
using Il2CppAssets.Scripts.PeroTools.Nice.Interface;
using Il2CppSystem.Runtime.Remoting.Messaging;
using System.Reflection;
using UnityEngine;

namespace LocalizeLib
{
    public static class Utils
    {
        public const int LanguageCount = 5;
        public static bool TryFormatAll(LocalString format, out LocalString? result, params object?[] args)
        {
            ArgumentNullException.ThrowIfNull(format, nameof(format));
            ArgumentNullException.ThrowIfNull(args, nameof(args));
            try
            {
                result = format.FormatAll(args);
            }
            catch (Exception)
            {
                result = null;
                return false;
            }
            return true;
        }
        public static bool TryFormat(string format, out string? result, params object?[] args)
        {
            ArgumentNullException.ThrowIfNull(format, nameof(format));
            ArgumentNullException.ThrowIfNull(args, nameof(args));
            try
            {
                result = string.Format(format, args);
            }
            catch (Exception)
            {
                result = null;
                return false;
            }
            return true;
        }
        static readonly Dictionary<int, object?[]> _cacheNullArrays = new()
        {
            {0, Array.Empty<object>()},
        };
        public static bool IsFormattable(string format, int count)
        {
            if (!_cacheNullArrays.TryGetValue(count, out var args))
            {
                args = _cacheNullArrays[count] = new object?[count];
            }
            return TryFormat(format, out _, args);
        }
        public static bool IsFormattable(LocalString format, int count)
        {
            if (!_cacheNullArrays.TryGetValue(count, out var args))
            {
                args = _cacheNullArrays[count] = new object?[count];
            }
            return TryFormatAll(format, out _, args);
        }
        /// <summary>
        /// Current language as a SystemLanguage enum
        /// </summary>
        public static SystemLanguage GetLanguage() => GameAccountSystem.instance.GetLanguage();

        /// <summary>
        /// Current language as a string
        /// </summary>
        public static string GetLangString() => VariableUtils.GetResult<string>(Singleton<DataManager>.instance["Account"]["Language"]);

        /// <summary>
        /// Converts SystemLanguage enum to its Muse Dash alternative
        /// </summary>
        public static string LanguageToString(SystemLanguage lang)
        {
            switch (lang)
            {
                case SystemLanguage.ChineseSimplified:
                    return AsString.ChineseSimplified;
                case SystemLanguage.ChineseTraditional:
                    return AsString.ChineseTraditional;
                case SystemLanguage.Korean:
                    return AsString.Korean;
                case SystemLanguage.Japanese:
                    return AsString.Japanese;
                default:
                    return AsString.English;
            }
        }

        /// <summary>
        /// Because I keep forgetting the languages.
        /// </summary>
        public static class AsString
        {
            public const string ChineseSimplified = "ChineseS";
            public const string ChineseTraditional = "ChineseT";
            public const string Korean = "Korean";
            public const string Japanese = "Japanese";
            public const string English = "English";
            public static IEnumerable<string> All()
            {
                yield return English;
                yield return Japanese;
                yield return Korean;
                yield return ChineseSimplified;
                yield return ChineseTraditional;
            }
        }
    }
}