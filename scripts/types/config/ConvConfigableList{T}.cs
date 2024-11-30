namespace SCECore
{
    using System.Xml;

    internal class ConvConfigableList<T> : IConfigable
        where T : IConvertible
    {
        private const char DEF_SPLIT_CHAR = ',';

        private readonly List<T> valueList = new();

        public ConvConfigableList(string tagName)
        {
            TagName = tagName;
        }

        public string TagName { get; set; }

        public IList<T> ValueIList { get => valueList.AsReadOnly(); }

        public char[]? LeftBoundArray { get; set; }

        public char[]? RightBoundArray { get; set; }

        public char SplitChar { get; set; } = DEF_SPLIT_CHAR;

        public event EventHandler? OnLoadEvent;

        public void Load(XmlNode node)
        {
            if (node.Name != TagName || node.Value is null)
                return;

            string[] valueStrArray = SplitValueList(node.Value);

            valueList.Clear();
            foreach (string valueStr in valueStrArray)
            {
                valueList.Add(ConvertString(valueStr));
            }

            OnLoadEvent?.Invoke(this, EventArgs.Empty);
        }

        private string[] SplitValueList(string listStr)
        {
            if (LeftBoundArray is null)
                return listStr.Split(SplitChar);
            if (RightBoundArray is null)
                return StringUtils.SplitExcludingBounds(listStr, SplitChar, LeftBoundArray);
            return StringUtils.SplitExcludingBounds(listStr, SplitChar, LeftBoundArray, RightBoundArray);
        }

        private T ConvertString(string valueStr)
        {
            return (T)Convert.ChangeType(valueStr, typeof(T)) ?? throw new NullReferenceException("Result is null.");
        }
    }
}
