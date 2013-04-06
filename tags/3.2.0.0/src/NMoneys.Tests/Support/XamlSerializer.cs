using System.Text;
using System.Windows.Markup;
using System.Xml;

namespace NMoneys.Tests.Support
{
	internal class XamlSerializer
	{
		internal string Serialize<T>(T toSerialize)
		{
			XmlWriterSettings settings = new XmlWriterSettings
			{
				Indent = true,
				NewLineOnAttributes = true,
				ConformanceLevel = ConformanceLevel.Fragment
			};
			StringBuilder sb = new StringBuilder();
			using (XmlWriter writer = XmlWriter.Create(sb, settings))
			{
				if (writer != null)
				{
					try
					{
						XamlDesignerSerializationManager manager = new XamlDesignerSerializationManager(writer)
						{
							XamlWriterMode = XamlWriterMode.Expression
						};
						XamlWriter.Save(toSerialize, manager);
					}
					finally
					{
						writer.Flush();
						writer.Close();
					}
				}
			}
			return sb.ToString();
		}

		internal T Deserialize<T>(string xamlText)
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xamlText);
			return (T)XamlReader.Load(new XmlNodeReader(doc));
		}
	}
}
