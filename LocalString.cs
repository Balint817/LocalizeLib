using System.Text;

namespace LocalizeLib
{
    /// <summary>
    /// Class used to store localized strings
    /// </summary>
    public class LocalString
    {
        static LocalString EmptyString { get; } = new();
        public static IEnumerable<string?[]> GetContents(params LocalString[] objects)
        {
            string?[] values = new string?[objects.Length + 1];
            for (int i = 0; i < Utils.LanguageCount; i++)
            {
                values[0] = GetLanguage(i);
                for (int j = 0; j < objects.Length; j++)
                {
                    var current = objects[j];
                    values[j + 1] = current[i];
                }
                yield return values.ToArray();
            }
        }
        public IEnumerable<string?[]> GetContents()
        {
            for (int i = 0; i < Utils.LanguageCount; i++)
            {
                yield return new string?[] { GetLanguage(0), this[i] };
            }
        }

        public static string GetLanguage(int i)
        {
            return Utils.AsString.All().ElementAt(i);
        }

        public string? Current()
        {
            return this[Utils.GetLangString()];
        }

        public override string? ToString()
        {
            return this.Current();
        }
        public string? this[int i]
        {
            get
            {
                return i switch
                {
                    0 => English,
                    1 => Japanese,
                    2 => Korean,
                    3 => ChineseSimplified,
                    4 => ChineseTraditional,
                    _ => throw new IndexOutOfRangeException(),
                };
            }
            private set
            {
                _ = i switch
                {
                    0 => English = value,
                    1 => Japanese = value,
                    2 => Korean = value,
                    3 => ChineseSimplified = value,
                    4 => ChineseTraditional = value,
                    _ => throw new IndexOutOfRangeException(),
                };
            }
        }
        public string? this[string lang]
        {
            get
            {
                return lang switch
                {
                    Utils.AsString.English => English,
                    Utils.AsString.Japanese => Japanese,
                    Utils.AsString.Korean => Korean,
                    Utils.AsString.ChineseSimplified => ChineseSimplified,
                    Utils.AsString.ChineseTraditional => ChineseTraditional,
                    _ => throw new IndexOutOfRangeException(),
                };
            }
        }

        private string? _chineseS;
        private string? _chineseT;
        private string? _korean;
        private string? _japanese;

        /// <summary>
        /// Localized string.
        /// The default if selected language is not set.
        /// </summary>
        public string? English { get; set; }

        /// <summary>
        /// Localized string.
        /// Returns English if null.
        /// </summary>
        public string? ChineseSimplified
        {
            get
            {
                return _chineseS ?? English;
            }
            set
            {
                _chineseS = value;
            }
        }

        /// <summary>
        /// Localized string.
        /// Returns English if null.
        /// </summary>
        public string? ChineseTraditional
        {
            get
            {
                return _chineseT ?? English;
            }
            set
            {
                _chineseT = value;
            }
        }

        /// <summary>
        /// Localized string.
        /// Returns English if null.
        /// </summary>
        public string? Korean
        {
            get
            {
                return _korean ?? English;
            }
            set
            {
                _korean = value;
            }
        }

        /// <summary>
        /// Localized string.
        /// Returns English if null.
        /// </summary>
        public string? Japanese
        {
            get
            {
                return _japanese ?? English;
            }
            set
            {
                _japanese = value;
            }
        }
        public LocalString()
        {

        }
        /// <summary>
        /// Single value constructor, sets all to passed value
        /// </summary>
        public LocalString(string setAll)
        {
            for (int i = 0; i < Utils.LanguageCount; i++)
            {
                this[i] = setAll;
            }
        }

        /// <summary>
        /// Creates a deep copy of the object
        /// </summary>
        public LocalString Copy()
        {
            var result = new LocalString();
            for (int i = 0; i < Utils.LanguageCount; i++)
            {
                result[i] = this[i];
            }
            return result;
        }
        /// <summary>
        /// Returns a deep copy of the object where all values are formatted with the given parameters
        /// </summary>
        public LocalString FormatAll(params object?[] args)
        {
            static object? Localizer(object? obj, int languageIdx)
            {
                if (obj is not LocalString s)
                {
                    return obj;
                }
                return s[languageIdx];
            }

            if (args == null || args.Length == 0)
            {
                return this.Copy();
            }
            var result = new LocalString();
            for (int i = 0; i < Utils.LanguageCount; i++)
            {
                var current = this[i];
                if (current is not null)
                {
                    result[i] = string.Format(current, args.Select(x => Localizer(x,i)).ToArray());
                }
            }
            return result;
        }


        public static explicit operator LocalString?(string s)
        {
            return s is null ? null : new LocalString(s);
        }
        /// <summary>
        /// Combines the supplied <see cref="LocalString"/> instances into
        /// a single instance using the given separator.
        /// </summary>
        public static LocalString Concat(LocalString? separator, params LocalString?[] strings)
        {
            if (strings is null || strings.Length == 0)
            {
                return new LocalString();
            }
            else if (strings.Length == 1)
            {
                return strings[0]?.Copy() ?? new LocalString();
            }
            separator ??= EmptyString;
            var lastIdx = strings.Length-1;
            var stringBuilders = new StringBuilder[Utils.LanguageCount];
            for (int i = 0; i < Utils.LanguageCount; i++)
            {
                stringBuilders[i] = new StringBuilder();
            }
            for (int stringIdx = 0; stringIdx < lastIdx; stringIdx++)
            {
                var current = strings[stringIdx];
                if (current is null)
                {
                    for (int languageIdx = 0; languageIdx < Utils.LanguageCount; languageIdx++)
                    {
                        stringBuilders[languageIdx].Append(separator[languageIdx]);
                    }
                    continue;
                }
                for (int languageIdx = 0; languageIdx < Utils.LanguageCount; languageIdx++)
                {
                    var builder = stringBuilders[languageIdx];
                    builder.Append(current[languageIdx]);
                    builder.Append(separator[languageIdx]);
                }
            }
            var result = new LocalString();
            for (int languageIdx = 0; languageIdx < Utils.LanguageCount; languageIdx++)
            {
                result[languageIdx] = stringBuilders[languageIdx].ToString();
            }
            return result;
        }
        public static LocalString Concat(LocalString? separator, IEnumerable<LocalString?> strings)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            return Concat(separator, strings?.ToArray());
#pragma warning restore CS8604 // Possible null reference argument.
        }
        public static LocalString Concat(IEnumerable<LocalString?> strings)
        {
            return Concat(null, strings);
        }
    }
}