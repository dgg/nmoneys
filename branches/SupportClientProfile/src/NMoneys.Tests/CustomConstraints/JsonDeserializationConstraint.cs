using System.Web.Script.Serialization;
using NMoneys.Serialization;
using NUnit.Framework.Constraints;

namespace NMoneys.Tests.CustomConstraints
{
	internal class JsonDeserializationConstraint<T> : CustomConstraint<string>
	{
		public JsonDeserializationConstraint(T expectedDeserializedObject)
		{
			_inner = new EqualConstraint(expectedDeserializedObject);
		}

		protected override bool matches(string current)
		{
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			serializer.RegisterConverters(new JavaScriptConverter[] { new MoneyConverter(), new CurrencyConverter(), new CurrencyCodeConverter() });
			T actualDeserialized = serializer.Deserialize<T>(current);

			return _inner.Matches(actualDeserialized);
		}
	}
}