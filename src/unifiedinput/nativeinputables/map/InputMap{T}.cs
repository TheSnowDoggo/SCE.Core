using System.Collections;

namespace SCE
{
    public class InputMap<T> : InputBase, IEnumerable<KeyValuePair<ConsoleKey, T>>
    {
        public InputMap(Dictionary<ConsoleKey, T>? dictionary = null, Action<T>? onKey = null)
            : base()
        {
            Dictionary = dictionary ?? new();
            OnKey = onKey;
        }

        public InputMap(Action<T>? onKey)
           : this(null, onKey)
        {
        }

        #region IEnumerable

        public IEnumerator<KeyValuePair<ConsoleKey, T>> GetEnumerator()
        {
            return Dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public Dictionary<ConsoleKey, T> Dictionary { get; set; }

        public Action<T>? OnKey;

        public HashSet<InputType> AllowedInputModes { get; set; } = new()
        {
            InputType.OnKeyDown,
            InputType.ConsoleStream
        };

        public override void LoadKeyInfo(UISKeyInfo uisKeyInfo)
        {
            if (!AllowedInputModes.Contains(uisKeyInfo.InputMode))
                return;
            if (OnKey is not null && Dictionary.TryGetValue(uisKeyInfo.KeyInfo.Key, out T? value))
            {
                OnKey.Invoke(value);
            }
        }

        public void Add(ConsoleKey key, T value)
        {
            Dictionary.Add(key, value);
        }
    }
}
