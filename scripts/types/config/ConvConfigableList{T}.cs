namespace SCE
{
    using System.Xml;

    public class ConvConfigableList<T> : IConfigable
        where T : IConvertible
    {
        private const char DEF_SPLIT_CHAR = ',';

        private readonly List<T> valueList = new();

        public ConvConfigableList(string name, string tagName)
        {
            Name = name;
            TagName = tagName;
        }

        public string Name { get; set; }

        public string TagName { get; set; }

        public IList<T> Value { get => valueList.AsReadOnly(); }

        public char[]? LeftBoundArray { get; set; }

        public char[]? RightBoundArray { get; set; }

        public char SplitChar { get; set; } = DEF_SPLIT_CHAR;

        public event EventHandler? OnLoadEvent;

        public bool Load(XmlNode node)
        {
            if (node.Name != TagName)
                return false;

            string[] valueStrArray = SplitValueList(node.InnerText);

            valueList.Clear();
            foreach (string valueStr in valueStrArray)
            {
                if ((T)Convert.ChangeType(valueStr, typeof(T)) is T t)
                    valueList.Add(t);
            }

            OnLoadEvent?.Invoke(this, EventArgs.Empty);

            return true;
        }

        private string[] SplitValueList(string listStr)
        {
            if (LeftBoundArray is null)
                return listStr.Split(SplitChar);
            if (RightBoundArray is null)
                return StringUtils.SplitExcludingBounds(listStr, SplitChar, LeftBoundArray);
            return StringUtils.SplitExcludingBounds(listStr, SplitChar, LeftBoundArray, RightBoundArray);
        }
    }
}
