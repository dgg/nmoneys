using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using NMoneys.Support;

namespace NMoneys.Serialization
{
	public class CurrencyConverter : JavaScriptConverter
	{
		public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
		{
			if (type != typeof(Currency)) throw new NotSupportedException();
			CurrencyIsoCode isoCode = Enumeration.Parse<CurrencyIsoCode>((string)dictionary[Data.Currency.ISO_CODE]);
			return Currency.Get(isoCode);
		}

		public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
		{
			Currency currency = (Currency)obj;
			IDictionary<string, object> serialized = new Dictionary<string, object>(1)
			{
				{Data.Currency.ISO_CODE, currency.IsoSymbol}
			};
			return serialized;
		}

		public override IEnumerable<Type> SupportedTypes
		{
			get { return new[] { typeof(Currency) }; }
		}
	}
}