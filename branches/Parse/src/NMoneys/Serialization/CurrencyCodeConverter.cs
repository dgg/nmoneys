using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using NMoneys.Support;

namespace NMoneys.Serialization
{
	public class CurrencyCodeConverter : JavaScriptConverter
	{
		public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
		{
			if (type != typeof(CurrencyIsoCode)) throw new NotSupportedException();
			CurrencyIsoCode isoCode = Enumeration.Parse<CurrencyIsoCode>((string)dictionary[Data.Currency.ISO_CODE]);
			return isoCode;
		}

		public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
		{
			CurrencyIsoCode currency = (CurrencyIsoCode)obj;
			IDictionary<string, object> serialized = new Dictionary<string, object>(1)
			{
				{Data.Currency.ISO_CODE, currency.ToString()}
			};
			return serialized;
		}

		public override IEnumerable<Type> SupportedTypes
		{
			get { return new[] { typeof(CurrencyIsoCode) }; }
		}
	}
}