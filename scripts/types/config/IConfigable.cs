namespace SCECore
{
    using System.Xml;

    public interface IConfigable
    {
        string Name { get; }
        bool Load(XmlNode node);
    }
}
