# NMoneys.Exchange.Demo.CodeProject

Supporting code and HTML sources for the old [CodeProject article: NMoneys.Exchange, Companion for NMoneys](https://www.codeproject.com/Articles/248487/NMoneys-Exchange-companion-for-NMoneys).

## DefaultConversions

```csharp
[TestFixture]
public class DefaultConversions
{
	[Test]
	public void Default_Conversions_DoNotBlowUpButAreNotTerriblyUseful()
	{
		var tenEuro = new Money(10m, CurrencyIsoCode.EUR);
		var tenDollars = tenEuro.Convert().To(CurrencyIsoCode.USD);
		Assert.That(tenDollars.Amount, Is.EqualTo(10m));

		var tenPounds = tenEuro.Convert().To(Currency.Gbp);
		Assert.That(tenPounds.Amount, Is.EqualTo(10m));
	}
}
```

## ConfigureProvider

```csharp
[TestFixture]
public class ConfigureProvider
{
	[Test]
	public void Configuring_Provider()
	{
		var customProvider = new TabulatedExchangeRateProvider();
		customProvider.Add(CurrencyIsoCode.EUR, CurrencyIsoCode.USD, 0);
		ExchangeRateProvider.Factory = () => customProvider;
		var tenEuro = new Money(10m, CurrencyIsoCode.EUR);
		var zeroDollars = tenEuro.Convert().To(CurrencyIsoCode.USD);
		// go back to default
		ExchangeRateProvider.Factory = ExchangeRateProvider.Default;
	}
}
```

## CustomRateOperations

```csharp
[TestFixture]
public class CustomRateOperations
{
	public class CustomRateArithmetic : ExchangeRate
	{
		public CustomRateArithmetic(CurrencyIsoCode from, CurrencyIsoCode to, decimal rate) : base(from, to, rate) { }
		public override Money Apply(Money from)
		{
			// instead of this useless "return 0" policy one can implement rounding policies, for instance
			return new Money(0m, To);
		}
	}
	public class CustomArithmeticProvider : IExchangeRateProvider
	{
		public ExchangeRate Get(CurrencyIsoCode from, CurrencyIsoCode to)
		{
			return new CustomRateArithmetic(from, to, 1m);
		}
		public bool TryGet(CurrencyIsoCode from, CurrencyIsoCode to, out ExchangeRate rate)
		{
			rate = new CustomRateArithmetic(from, to, 1m);
			return true;
		}
	}
	[Test]
	public void Use_CustomArithmeticProvider()
	{
		var customProvider = new CustomArithmeticProvider();
		ExchangeRateProvider.Factory = () => customProvider;
		var zeroDollars = 10m.Eur().Convert().To(CurrencyIsoCode.USD);
		Assert.That(zeroDollars, Is.EqualTo(0m.Usd()));
		// go back to default
		ExchangeRateProvider.Factory = ExchangeRateProvider.Default;
	}
}
```

## RedefineAPI

```csharp
public static class ApiExtensions
{
	public static UsingImplementor Using(this IExchangeConversion conversion, decimal rate)
	{
		return new UsingImplementor(conversion.From, rate);
	}
}
public class UsingImplementor
{
	private readonly Money _from;
	private readonly decimal _rate;
	public UsingImplementor(Money from, decimal rate)
	{
		_from = from;
		_rate = rate;
	}
	public Money To(CurrencyIsoCode to)
	{
		var rateCalculator = new ExchangeRate(_from.CurrencyCode, to, _rate);
		return rateCalculator.Apply(_from);
	}
}
[TestFixture]
public class RedefineAPI
{
	[Test]
	public void Creating_New_ConversionOperations()
	{
		var hundredDollars = new Money(100m, CurrencyIsoCode.USD);
		var twoHundredEuros = hundredDollars.Convert().Using(2m).To(CurrencyIsoCode.EUR);
		Assert.That(twoHundredEuros, Is.EqualTo(200m.Eur()));
	}
}
```
