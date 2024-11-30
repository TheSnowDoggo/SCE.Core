namespace SCECore
{
    using System.Xml;

    internal class ConvConfigable<T> : IConfigable
        where T : IConvertible
    {
        private T? value;
        public ConvConfigable(string tagName)
        {
            TagName = tagName;
        }

        public string TagName { get; set; }

        public T Value { get => value ?? throw new NullReferenceException("Value is null."); }

        public bool HasValue { get => value != null; }

        public event EventHandler? OnLoadEvent;

        public void Load(XmlNode node)
        {
            if (node.Name == TagName)
            {
                object? value = Convert.ChangeType(node.Value, typeof(T));
                if (value is T t)
                {
                    value = t;
                    OnLoadEvent?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}
