using System;
using System.Text;
using System.Web.Script.Serialization;
using NMoneys.Serialization;
using NUnit.Framework.Constraints;

namespace NMoneys.Tests.CustomConstraints
{
	internal class JsonSerializationConstraint<T> : CustomConstraint<T>
	{
		private readonly Func<T, Constraint> _constraintOverDeserialized;

		public JsonSerializationConstraint(Func<T, Constraint> constraintOverDeserialized)
		{
			_constraintOverDeserialized = constraintOverDeserialized;
		}

		protected override bool matches(T current)
		{
			_inner = _constraintOverDeserialized(getDeserializedObject(current));
			return _inner.Matches(current);
		}

		private static T getDeserializedObject(T toSerialize)
		{
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			serializer.RegisterConverters(new JavaScriptConverter[] { new MoneyConverter(), new CurrencyConverter(), new CurrencyCodeConverter() });
			StringBuilder sb = new StringBuilder();
			serializer.Serialize(toSerialize, sb);

			T deserialized = serializer.Deserialize<T>(sb.ToString());

			return deserialized;
		}
	}
}