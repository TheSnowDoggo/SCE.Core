namespace SCECore.Types
{
    using System.Xml;

    internal class ConvConfigable<T> : IConfigable
        where T : IConvertible
    {
        public ConvConfigable(string tagName)
        {
            TagName = tagName;
        }

        public string TagName { get; set; }

        public T? Value { get; private set; }

        public event EventHandler? OnLoadEvent;

        public void Load(XmlNode node)
        {
            if (node.Name == TagName)
            {
                object? value = Convert.ChangeType(node.Value, typeof(T));
                if (value is T t)
                {
                    Value = t;
                    OnLoadEvent?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}
