// instead of having a bunch of conditional compilations, the attributes are redefined, 
// since they are only metadata for framework code that will not be called

namespace System.Xml.Serialization
{
    public class XmlIgnoreAttribute : Attribute { }
}

namespace System.Runtime.Serialization
{
	public class EnumMemberAttribute : Attribute { }
}