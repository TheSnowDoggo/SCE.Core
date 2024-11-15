namespace SCECore
{
    using System.Xml;

    internal interface IConfigable
    {
        void Load(XmlNode node);
    }
}
