namespace NMoneys.Exchange.Tests
{
	internal static class ComplexProviderData
	{
		internal static object[] fromEUR = new object[]
		{
			new object[]{CurrencyIsoCode.EUR, 1m},
			new object[]{CurrencyIsoCode.USD, 1.42542m},
			new object[]{CurrencyIsoCode.GBP, 0.88176m}
		};

		internal static object[] fromEURInverse = new object[]
		{
			new object[]{CurrencyIsoCode.EUR, 1m},
			new object[]{CurrencyIsoCode.USD, 0.70155m},
			new object[]{CurrencyIsoCode.GBP, 1.13409m}
		};

		internal static object[] toEUR = new object[]
		{
			new object[]{CurrencyIsoCode.EUR, 1m},
			new object[]{CurrencyIsoCode.USD, 0.70155m},
			new object[]{CurrencyIsoCode.GBP, 1.13409m}
		};

		internal static object[] toEURInverse = new object[]
		{
			new object[]{CurrencyIsoCode.EUR, 1m},
			new object[]{CurrencyIsoCode.USD, 1.42542m},
			new object[]{CurrencyIsoCode.GBP, 0.88176m}
		};
	}
}