namespace SCE
{
    using System.Xml;

    public class ConvConfigable<T> : IConfigable
        where T : IConvertible
    {
        private T? value = default;

        public ConvConfigable(string name, string tagName)
        {
            Name = name;
            TagName = tagName;
        }

        public string Name { get; set; }

        public string TagName { get; set; }

        public T Value { get => value ?? throw new NullReferenceException("Value is null."); }

        public bool HasValue { get => value != null; }

        public event EventHandler? OnLoadEvent;

        public bool Load(XmlNode node)
        {
            if (node.Name != TagName)
                return false; 

            if (Convert.ChangeType(node.InnerText, typeof(T)) is T t)
            {
                value = t;
                OnLoadEvent?.Invoke(this, EventArgs.Empty);
                return true;
            }
            return false;
        }
    }
}
