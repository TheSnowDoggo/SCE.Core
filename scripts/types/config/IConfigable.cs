namespace SCECore
{
    using System.Xml;

    public interface IConfigable
    {
        void Load(XmlNode node);
    }
}
