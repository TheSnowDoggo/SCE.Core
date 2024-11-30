namespace SCECore
{
    using System.Xml;

    using System.Collections;

    public class Config : IEnumerable<IConfigable>
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

        public int Count { get => _configableList.Count; }

        public IConfigable this[int index]
        {
            get => _configableList[index];
            set => _configableList[index] = value;
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
                Load(node);
            }
        }

        private void Load(XmlNode node)
        {
            foreach (IConfigable configable in this)
            {
                configable.Load(node);
            }
        }

        #region Add
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

        public void RemoveAt(int index)
        {
            _configableList.RemoveAt(index);
        }
        #endregion

        #region Contains
        public bool Contains(Func<IConfigable, bool> func, out int index)
        {
            for (int i = 0; i < Count; ++i)
            {
                if (func.Invoke(this[i]))
                {
                    index = i;
                    return true;
                }
            }
            index = -1;
            return false;
        }

        public bool Contains(string name, out int index)
        {
            return Contains((configable) => configable.Name == name, out index);
        }
        #endregion

        #region Get
        public IConfigable Get(Func<IConfigable, bool> func)
        {
            if (!Contains(func, out int index))
                throw new ArgumentException("No configable found.");
            return this[index];
        }
        public IConfigable Get(string name)
        {
            return Get((configable) => configable.Name == name);
        }

        public T Get<T>(Func<IConfigable, bool> func)
        {
            return (T)Get(func);
        }
        public T Get<T>(string name)
        {
            return (T)Get(name);
        }
        #endregion
    }
}
