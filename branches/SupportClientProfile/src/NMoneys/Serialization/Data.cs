using System.Runtime.Serialization;

namespace NMoneys.Serialization
{
	internal static class Data
	{
		[DataContract]
		public class Currency
		{
			internal const string ISO_CODE = "isoCode";
			internal const string ROOT_NAME = "currency", DATA_TYPE = "currencyType";

			[DataMember(Name = ISO_CODE)]
			public string IsoCode { get; set; }

			public Currency(NMoneys.Currency surrogated)
			{
				IsoCode = surrogated.IsoSymbol;
			}

			public Currency(NMoneys.CurrencyIsoCode surrogated)
			{
				IsoCode = surrogated.AlphabeticCode();
			}

			public NMoneys.Currency RevertSurrogation()
			{
				return NMoneys.Currency.Get(IsoCode);
			}
		}

		[DataContract]
		public class Money
		{
			internal const string AMOUNT = "amount", CURRENCY = "currency";
			internal const string ROOT_NAME = "money", DATA_TYPE = "moneyType";

			[DataMember(Name = AMOUNT)]
			public decimal Amount { get; set; }
			[DataMember(Name = CURRENCY)]
			public Currency Currency { get; set; }

			public Money(NMoneys.Money surrogated)
			{
				Amount = surrogated.Amount;
				Currency = new Currency(surrogated.CurrencyCode);
			}

			public NMoneys.Money RevertSurrogation()
			{
				return new NMoneys.Money(Amount, Currency.IsoCode);
			}
		}

		internal const string NAMESPACE = "urn:nmoneys";
		internal static readonly string ResourceName = typeof(Data).Namespace + "." + "SerializationSchema.xsd";
	}
}
