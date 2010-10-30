namespace NMoneys.Serialization
{
	internal static class Data
	{
		internal class Currency
		{
			internal const string ISO_CODE = "isoCode";
			internal const string ROOT_NAME = "currency", DATA_TYPE = "currencyType";
		}

		internal class Money
		{
			internal const string AMOUNT = "amount", CURRENCY = "currency";
			internal const string ROOT_NAME = "money", DATA_TYPE = "moneyType";
		}
		internal const string NAMESPACE = "urn:nmoneys";
		internal static readonly string ResourceName = typeof(Data).Namespace + "." + "SerializationSchema.xsd";
	}
}
