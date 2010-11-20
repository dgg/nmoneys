using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace NMoneys.Serialization
{
	public class MoneyConverter : JavaScriptConverter
	{
		public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
		{
			if (type != typeof(Money)) throw new NotSupportedException();

			Currency currency = serializer.ConvertToType<Currency>(dictionary[Data.Money.CURRENCY]);
			decimal amount = (decimal)dictionary[Data.Money.AMOUNT];
			return new Money(amount, currency);
		}

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

		public override IEnumerable<Type> SupportedTypes
		{
			get { return new[] { typeof(Money) }; }
		}
	}
}
