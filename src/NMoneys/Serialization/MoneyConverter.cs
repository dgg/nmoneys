using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace NMoneys.Serialization
{
	/// <summary>
	/// Provides JavaScript Object Notation (JSON) format conversions for <see cref="Money"/>.
	/// </summary>
	public class MoneyConverter : JavaScriptConverter
	{
		/// <summary>
		/// Converts the provided dictionary into an object of the specified type.
		/// </summary>
		/// <remarks>Only <see cref="Money"/> instances can be handled.</remarks>
		/// <returns>
		/// The deserialized object.
		/// </returns>
		/// <param name="dictionary">An <see cref="T:System.Collections.Generic.IDictionary`2"/> instance of property data stored as name/value pairs.</param>
		/// <param name="type">The type of the resulting object.</param>
		/// <param name="serializer">The <see cref="T:System.Web.Script.Serialization.JavaScriptSerializer"/> instance.</param>
		public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
		{
			if (type != typeof(Money)) throw new NotSupportedException();

			Currency currency = serializer.ConvertToType<Currency>(dictionary[Data.Money.CURRENCY]);
			decimal amount = (decimal)dictionary[Data.Money.AMOUNT];
			return new Money(amount, currency);
		}

		/// <summary>
		/// Builds a dictionary of name/value pairs.
		/// </summary>
		/// <remaks>Only instances of <see cref="Money"/> are supported.</remaks>
		/// <returns>
		/// An object that contains key/value pairs that represent the object’s data.
		/// </returns>
		/// <param name="obj">The object to serialize.</param>
		/// <param name="serializer">The object that is responsible for the serialization.</param>
		/// <exception cref="InvalidCastException">When the <paramref name="obj"/> cannot be casted to <see cref="Money"/>.</exception>
		public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
		{
			Money money = (Money)obj;
			IDictionary<string, object> serialized = new Dictionary<string, object>(3)
			{
				{Data.Money.AMOUNT, money.Amount},
				{Data.Money.CURRENCY, money.CurrencyCode}
			};

			return serialized;
		}

		/// <summary>
		/// Gets a collection of the supported types.
		/// </summary>
		/// <remarks>Only <see cref="Money"/> is supported.</remarks>
		/// <returns>
		/// An object that implements <see cref="T:System.Collections.Generic.IEnumerable`1"/> that represents the types supported by the converter.
		/// </returns>
		public override IEnumerable<Type> SupportedTypes
		{
			get { return new[] { typeof(Money) }; }
		}
	}
}
