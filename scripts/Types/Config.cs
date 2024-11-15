namespace SCECore.Types
{
    using System.Xml;

    using System.Collections;

    internal class Config : IEnumerable<IConfigable>
    {
        private readonly XmlDocument _document;

        private readonly List<IConfigable> _configableList = new();

        public Config(XmlDocument document)
        {
            _document = document;
        }

        public Config(string relativePath)
            : this(new XmlDocument())
        {
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);

            _document.Load(fullPath);
        }

        public IEnumerator<IConfigable> GetEnumerator()
        {
            return _configableList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Setup()
        {
            foreach (XmlNode node in _document)
            {
            }
        }

        public void Add(IConfigable configable)
        {
            _configableList.Add(configable);
        }

        public void Add(IConfigable[] configableArray)
        {
            foreach (IConfigable configable in configableArray)
            {
                Add(configable);
            }
        }

        public void Add(List<IConfigable> confibaleList)
        {
            Add(confibaleList.ToArray());
        }

        public bool Remove(IConfigable configable)
        {
            return _configableList.Remove(configable);
        }
    }
}
