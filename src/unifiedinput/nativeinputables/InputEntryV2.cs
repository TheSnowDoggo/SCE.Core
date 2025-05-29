using System.Text;
using System.Collections;
namespace SCE
{
    public class InputEntryV2 : IInputReceiver
    {
        public class Intercept
        {
            public Intercept(InputEntryV2 entry, StringBuilder iStream, ConsoleKey key)
            {
                Entry = entry;
                IStream = iStream;
                Key = key;
            }

            public InputEntryV2 Entry { get; }

            public StringBuilder IStream { get; set; }

            public ConsoleKey Key { get; set; }
        }

        public enum VMode
        {
            And,
            Or,
            OrTrueEmpty,
        }

        public class Validator<T> : IEnumerable<Func<T, bool>>
        {
            private readonly List<Func<T, bool>> _validators;

            public Validator(IEnumerable<Func<T, bool>> collection, VMode mode = VMode.Or)
            {
                _validators = new(collection);
                Mode = mode;
            }

            public Validator(VMode mode = VMode.Or)
            {
                _validators = new();
                Mode = mode;
            }

            #region IEnumerable

            public IEnumerator<Func<T, bool>> GetEnumerator()
            {
                return _validators.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion

            public VMode Mode { get; set; }

            public void Add(Func<T, bool> func)
            {
                _validators.Add(func);
            }

            public bool Remove(Func<T, bool> func)
            {
                return _validators.Remove(func);
            }

            public bool Validate(T value)
            {
                if (Mode == VMode.OrTrueEmpty && _validators.Count == 0)
                    return true;
                return ValidateCollection(_validators, value, Mode == VMode.And);
            }

            public static bool ValidateCollection(IEnumerable<Func<T, bool>> collection, T value, bool and)
            {
                foreach (var func in collection)
                    if (and ? !func(value) : func(value))
                        return !and;
                return and;
            }

            public static Func<T, bool> Combine(bool and, params Func<T, bool>[] arr)
            {
                return (t) => ValidateCollection(arr, t, and);
            }

            public static Func<T, bool> CombineAnd(params Func<T, bool>[] arr)
            {
                return Combine(true, arr);
            }

            public static Func<T, bool> CombineOr(params Func<T, bool>[] arr)
            {
                return Combine(false, arr);
            }
        }

        public StringBuilder SBuilder { get; } = new();

        public string Input
        {
            get => SBuilder.ToString();
            set
            {
                SBuilder.Clear();
                SBuilder.Append(value);
            }
        }

        public Validator<char> AllowedChars { get; set; } = new(VMode.OrTrueEmpty);

        public Validator<char> DisallowedChars { get; set; } = new(VMode.Or)
        {
            DefaultDisallowance(),
        };

        public Validator<Intercept> Intercepters { get; set; } = new(VMode.Or)
        {
            Backspace(ConsoleKey.Backspace),
        };

        public bool IsActive { get; set; } = true;

        public bool IsReceiving { get; set; } = false;

        public int CharacterIndex { get; set; } = -1;

        public Action? OnKey;

        public Action? OnReceive;

        public void Start(string initial = "")
        {
            SBuilder.Clear();
            SBuilder.Append(initial);
            IsReceiving = true;
        }

        /// <inheritdoc/>
        public void LoadKeyInfo(UISKeyInfo uki)
        {
            if (!IsReceiving)
                return;
            if (uki.InputMode != InputType.InputStream && uki.InputMode != InputType.ConsoleStream)
                return;

            var cki = uki.KeyInfo;
            Intercept intercept = new(this, new(cki.KeyChar.ToString()), cki.Key);

            if (!Intercepters.Validate(intercept))
            {
                foreach (char c in intercept.IStream.ToString())
                {
                    if (AllowedChars.Validate(c) && !DisallowedChars.Validate(c))
                    {
                        if (CharacterIndex < 0 || CharacterIndex >= SBuilder.Length)
                        {
                            SBuilder.Append(c);
                            CharacterIndex = SBuilder.Length;
                        }
                        else
                        {
                            SBuilder.Insert(CharacterIndex, c);
                            ++CharacterIndex;
                        }
                    }
                }
            }
                
            OnKey?.Invoke();
            if (IsReceiving)
                OnReceive?.Invoke();
        }

        public void Clear()
        {
            SBuilder.Clear();
        }

        #region CharacterAllowance

        public static Func<char, bool> DefaultDisallowance()
        {
            return Validator<char>.CombineAnd(DirectF(' '), char.IsControl);
        }

        public static Func<char, bool> Direct(char chr, bool trueIfMatch = true)
        {
            return (c) => chr == c == trueIfMatch;
        }

        public static Func<char, bool> DirectF(char chr)
        {
            return Direct(chr, false);
        }

        public static Func<char, bool> DirectSet(HashSet<char> set, bool trueIfContains = true)
        {
            return (c) => set.Contains(c) == trueIfContains;
        }

        public static Func<char, bool> DirectSetF(HashSet<char> set)
        {
            return DirectSet(set, false);
        }

        public static Func<char, bool> AllowExcept(Func<char, bool> func, char allow)
        {
            return (c) => c != allow && func(c);
        }

        #endregion

        #region Intercepts

        public static Func<Intercept, bool> Exit(Action action, ConsoleKey key = ConsoleKey.Escape)
        {
            return (i) =>
            {
                if (i.Key != key)
                    return false;
                i.Entry.IsReceiving = false;
                action.Invoke();
                return true;
            };
        }

        public static Func<Intercept, bool> Exit(ConsoleKey key = ConsoleKey.Escape)
        {
            return Exit(() => { }, key);
        }

        public static Func<Intercept, bool> Backspace(ConsoleKey key = ConsoleKey.Backspace)
        {
            return (i) =>
            {
                var sb = i.Entry.SBuilder;
                if (i.Key != key || sb.Length <= 0)
                    return false;
                if (i.Entry.CharacterIndex >= sb.Length)
                    sb.Remove(sb.Length - 1, 1);
                else if (i.Entry.CharacterIndex <= 0)
                    sb.Remove(0, 1);
                else
                    sb.Remove(i.Entry.CharacterIndex - 1, 1);
                --i.Entry.CharacterIndex;
                return true;
            };
        }

        public static Func<Intercept, bool> Newline(ConsoleKey key = ConsoleKey.Enter)
        {
            return KeyMap(key, '\n');
        }

        public static Func<Intercept, bool> CharacterLimiter(int length)
        {
            return (i) => i.Entry.SBuilder.Length >= length;
        }

        public static Func<Intercept, bool> LineLimiter(int lines)
        {
            return (i) => i.IStream.ToString().Contains('\n') && i.Entry.Input.Count(c => c == '\n') >= lines - 1;
        }

        public static Func<Intercept, bool> LineLengthLimiter(int length)
        {
            return (i) =>
            {
                if (i.IStream[0] == '\n')
                    return false;
                var str = i.Entry.Input;
                int index = str.LastIndexOf('\n');
                return (index == -1 ? str.Length : str.Length - index - 1) >= length;
            };
        }

        public static Func<Intercept, bool> KeyMap(ConsoleKey key, char chr)
        {
            return (map) =>
            {
                if (map.Key == key)
                    map.IStream[0] = chr;
                return false;
            };
        }

        public static Func<Intercept, bool> Scroll(
            ConsoleKey left = ConsoleKey.LeftArrow, 
            ConsoleKey right = ConsoleKey.RightArrow)
        {
            return (i) =>
            {
                if (i.Key != left && i.Key != right)
                    return false;
                int move = i.Key == left ? -1 : 1;
                int next = i.Entry.CharacterIndex + move;
                if (next >= 0 && next <= i.Entry.SBuilder.Length)
                    i.Entry.CharacterIndex = next;
                return true;
            };
        }

        #endregion
    }
}
