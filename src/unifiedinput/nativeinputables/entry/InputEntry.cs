using System.Text;

namespace SCE
{
    public class InputEntry : InputBase
    {
        private const ConsoleKey DEFAULT_ENTRYKEY = ConsoleKey.Enter;

        private const ConsoleKey DEFAULT_EXITKEY = ConsoleKey.Escape;

        private const ConsoleKey DEFAULT_OPENKEY = ConsoleKey.Enter;
       
        private readonly StringBuilder _strBuilder = new();

        private readonly HashSet<Intercept> _inbuiltIntecepters;

        private readonly HashSet<Intercept> _priorityIntercepters;

        public InputEntry()
        {
            _inbuiltIntecepters = new() { Backspace() };
            _priorityIntercepters = new() { OpenIntercepter, EntryIntercepter, ExitIntercepter };
        }

        public delegate bool Intercept(ConsoleKeyInfo keyInfo, StringBuilder strBuilder);

        public string Input { get => _strBuilder.ToString(); }

        #region Actions

        public Action? OnEntry { get; set; }

        public Action? OnExit { get; set; }

        public Action? OnOpen { get; set; }

        public Action? OnKey { get; set; }

        #endregion

        #region Settings

        public bool CheckForEntry { get; set; } = true;

        public bool CheckForExit { get; set; } = true;

        public bool CheckForOpen { get; set; } = true;

        public ConsoleKey ExitKey { get; set; } = DEFAULT_EXITKEY;

        public ConsoleKey EntryKey { get; set; } = DEFAULT_ENTRYKEY;

        public ConsoleKey OpenKey { get; set; } = DEFAULT_OPENKEY;

        public bool IsReceiving { get; set; } = false;

        public bool Intercepting { get; set; } = true;

        public bool AcceptEmptyEntry { get; set; } = false;

        public bool IgnoreInbuiltIntercepters { get; set; } = false;

        public bool CheckAllowedChars { get; set; } = true;

        public bool CheckDisallowedChars { get; set; } = true;

        public bool CheckAllowedInputs { get; set; } = true;

        public bool AllowNullTerminator { get; set; } = false;

        #endregion

        #region KeySettingData

        public List<ConsoleKey> EntryKeys { get; set; } = new() { ConsoleKey.Enter };

        public HashSet<Intercept> InputIntercepters { get; set; } = new();

        public Dictionary<ConsoleKey, char> CharacterMap { get; set; } = new();

        public List<Func<char, bool>> AllowedChars { get; set; } = new();

        public List<Func<char, bool>> DisallowedChars { get; set; } = new();

        public List<Func<string, bool>> AllowedInputs { get; set; } = new();

        #endregion

        public override void LoadKeyInfo(UISKeyInfo uisKeyInfo)
        {
            if (uisKeyInfo.InputMode != InputType.InputStream && uisKeyInfo.InputMode != InputType.ConsoleStream)
                return;
            if (IsInputIntercepted(uisKeyInfo.KeyInfo, _priorityIntercepters) || !IsReceiving)
                return;

            var key = uisKeyInfo.KeyInfo.Key;

            if ((IgnoreInbuiltIntercepters || !IsInputIntercepted(uisKeyInfo.KeyInfo, _inbuiltIntecepters)) 
                && (!Intercepting || !IsInputIntercepted(uisKeyInfo.KeyInfo, InputIntercepters)))
            {
                if (!CharacterMap.TryGetValue(key, out char chr))
                {
                    chr = uisKeyInfo.KeyInfo.KeyChar;
                }
                if ((AllowNullTerminator || chr != '\0') && (!CheckDisallowedChars || !IsCharacterDisallowed(chr)) 
                    && (!CheckAllowedChars || IsCharacterAllowed(chr) && (!CheckAllowedInputs || IsInputAllowed(chr))))
                {
                    _strBuilder.Append(chr);
                }    
            }

            OnKey?.Invoke();
        }

        public void SetInput(string str)
        {
            _strBuilder.Clear();
            _strBuilder.Append(str);
        }

        public void Clear()
        {
            _strBuilder.Clear();
        }

        #region InterceptFuncs

        #region KeyIntercepter

        public static Intercept KeyIntercepter(ConsoleKey key, Action<char, StringBuilder> action)
        {
            return (keyInfo, builder) =>
            {
                if (keyInfo.Key != key)
                    return false;
                action.Invoke(keyInfo.KeyChar, builder);
                return true;
            };
        }

        public static Intercept KeyIntercepter(ConsoleKey key, Action<StringBuilder> action)
        {
            return KeyIntercepter(key, (_, builder) => action.Invoke(builder));
        }

        public static Intercept KeyIntercepter(ConsoleKey key, Action action)
        {
            return KeyIntercepter(key, (_, _) => action.Invoke());
        }

        #endregion

        #region KeyCharIntercepter

        public static Intercept KeyCharIntercepter(ConsoleKey key, string str)
        {
            return KeyIntercepter(key, (builder) => builder.Append(str));
        }

        public static Intercept KeyCharIntercepter(ConsoleKey key, char chr)
        {
            return KeyIntercepter(key, (builder) => builder.Append(chr));
        }

        public static Intercept NewlineIntercepter(ConsoleKey key = ConsoleKey.Enter)
        {
            return KeyCharIntercepter(key, '\n');
        }

        #endregion

        #region Backspace

        public static Intercept Backspace(ConsoleKey key = ConsoleKey.Backspace)
        {
            return KeyIntercepter(key, (builder) =>
            {
                if (builder.Length > 0)
                    builder.Remove(builder.Length - 1, 1);
            });
        }

        #endregion

        #region PriorityIntercepters
        private bool OpenIntercepter(ConsoleKeyInfo keyInfo, StringBuilder _)
        {
            if (!(CheckForOpen && !IsReceiving && keyInfo.Key == OpenKey))
                return false;
            IsReceiving = true;
            OnOpen?.Invoke();
            return true;
        }

        private bool EntryIntercepter(ConsoleKeyInfo keyInfo, StringBuilder _)
        {
            if (!(CheckForEntry && IsReceiving && keyInfo.Key == EntryKey))
                return false;
            if (!AcceptEmptyEntry && Input == string.Empty)
                return true;
            IsReceiving = false;
            OnEntry?.Invoke();
            return true;
        }

        private bool ExitIntercepter(ConsoleKeyInfo keyInfo, StringBuilder _)
        {
            if (!(CheckForExit && IsReceiving && keyInfo.Key == ExitKey))
                return false;
            IsReceiving = false;
            _strBuilder.Clear();
            OnExit?.Invoke();
            return true;
        }
        #endregion

        #endregion

        #region AllowedFunc

        public static Func<string, bool> MaxLength(int maxLength)
        {
            return (str) => str.Length <= maxLength;
        }

        public static Func<string, bool> BoxLimit(Vector2Int dimensions)
        {
            var maxLines = MaxLines(dimensions.Y);
            var maxLineLength = MaxLineLength(dimensions.X);
            return (str) => maxLines(str) && maxLineLength(str);
        }

        public static Func<string, bool> MaxLines(int maxLines)
        {
            return str => StringUtils.ContainsLessThan(str, '\n', maxLines);
        }

        public static Func<string, bool> MaxLineLength(int maxLineLength)
        {
            return (str) => StringUtils.LongestSubstringBetween(str, '\n') <= maxLineLength;
        }

        public static bool Space(char chr)
        {
            return chr == ' ';
        }

        public static Func<char, bool> CharCheck(char check)
        {
            return (chr) => chr == check;
        }

        public static Func<char, bool> CharCheckRange(HashSet<char> checkSet)
        {
            return (chr) => checkSet.Contains(chr);
        }

        #endregion

        #region CheckAllowance

        private bool IsCharacterAllowed(char keyChar)
        {
            if (AllowedChars.Count == 0)
                return true;
            foreach (var func in AllowedChars)
                if (func.Invoke(keyChar))
                    return true;
            return false;
        }

        private bool IsCharacterDisallowed(char keyChar)
        {
            foreach (var func in DisallowedChars)
                if (!func.Invoke(keyChar))
                    return false;
            return true;
        }

        private bool IsInputAllowed(char keyChar)
        {
            string newInput = Input + keyChar;
            foreach (var func in AllowedInputs)
            {
                if (!func.Invoke(newInput))
                    return false;
            }
            return true;
        }

        #endregion

        private bool IsInputIntercepted(ConsoleKeyInfo keyInfo, IEnumerable<Intercept> intercepts)
        {
            foreach (var func in intercepts)
                if (func.Invoke(keyInfo, _strBuilder))
                    return true;
            return false;
        }
    }
}
