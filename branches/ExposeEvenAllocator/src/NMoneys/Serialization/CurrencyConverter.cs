using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace NMoneys.Serialization
{
	/// <summary>
	/// Provides JavaScript Object Notation (JSON) format conversions for <see cref="Currency"/>.
	/// </summary>
	public class CurrencyConverter : JavaScriptConverter
	{
		/// <summary>
		/// Converts the provided dictionary into an object of the specified type.
		/// </summary>
		/// <remarks>Only <see cref="Currency"/> instances can be handled.</remarks>
		/// <returns>
		/// The deserialized object.
		/// </returns>
		/// <param name="dictionary">An <see cref="T:System.Collections.Generic.IDictionary`2"/> instance of property data stored as name/value pairs.</param>
		/// <param name="type">The type of the resulting object.</param>
		/// <param name="serializer">The <see cref="T:System.Web.Script.Serialization.JavaScriptSerializer"/> instance.</param>
		public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
		{
			if (type != typeof(Currency)) throw new NotSupportedException();
			CurrencyIsoCode isoCode = Currency.Code.ParseArgument((string)dictionary[Data.Currency.ISO_CODE], Data.Currency.ISO_CODE);
			return Currency.Get(isoCode);
		}

		/// <summary>
		/// Builds a dictionary of name/value pairs.
		/// </summary>
		/// <remaks>Only instances of <see cref="Currency"/> are supported.</remaks>
		/// <returns>
		/// An object that contains key/value pairs that represent the object’s data.
		/// </returns>
		/// <param name="obj">The object to serialize.</param>
		/// <param name="serializer">The object that is responsible for the serialization.</param>
		/// <exception cref="InvalidCastException">When the <paramref name="obj"/> cannot be casted to <see cref="CurrencyIsoCode"/>.</exception>
		public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
		{
			Currency currency = (Currency)obj;
			IDictionary<string, object> serialized = new Dictionary<string, object>(1)
			{
				{Data.Currency.ISO_CODE, currency.IsoSymbol}
			};
			return serialized;
		}

		/// <summary>
		/// Gets a collection of the supported types.
		/// </summary>
		/// <remarks>Only <see cref="CurrencyIsoCode"/> is supported.</remarks>
		/// <returns>
		/// An object that implements <see cref="T:System.Collections.Generic.IEnumerable`1"/> that represents the types supported by the converter.
		/// </returns>
		public override IEnumerable<Type> SupportedTypes
		{
			get { return new[] { typeof(Currency) }; }
		}
	}
}