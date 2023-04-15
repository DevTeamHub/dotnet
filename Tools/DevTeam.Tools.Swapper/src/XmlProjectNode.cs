using System.Xml;

namespace DevTeam.Tools.Swapper;

public class XmlProjectNode
{
    public string Id { get; private set; }
    public string Version { get; private set; }
    public XmlNode Node { get; private set; }

    public XmlProjectNode(string id, string version, XmlNode node)
    {
        Id = id;
        Version = version;
        Node = node;
    }
}
