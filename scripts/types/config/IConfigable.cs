namespace SCECore
{
    using System.Xml;

    public interface IConfigable
    {
        string Name { get; }
        void Load(XmlNode node);
    }
}
