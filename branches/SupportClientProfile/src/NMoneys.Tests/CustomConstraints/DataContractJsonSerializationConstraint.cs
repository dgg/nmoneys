using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using NUnit.Framework.Constraints;

namespace NMoneys.Tests.CustomConstraints
{
	internal class DataContractJsonSerializationConstraint<T> : CustomConstraint<T>
	{
		public DataContractJsonSerializationConstraint()
		{
			_inner = new AndConstraint(
				new SubstringConstraint("<"),
				new SubstringConstraint(">"));
		}

		protected override bool matches(T current)
		{
			return _inner.Matches(serialize(current));
		}

		private static string serialize(T toSerialize)
		{
			using (var str = new MemoryStream())
			{
				try
				{
					var serializer = new DataContractJsonSerializer(typeof(T));
					serializer.WriteObject(str, toSerialize);

					str.Flush();
					str.Seek(0, SeekOrigin.Begin);

					string json = HttpUtility.HtmlDecode(Encoding.Default.GetString(str.ToArray()));
					return json;
				}
				finally
				{
					str.Close();
				}
			}
		}
	}
}