# NMoney's Vision
_NMoneys_ has a simple vision: providing a .Net implementation of the Money Value Object that abides to the ISO 4217 specification.

Converting monetary quantities from one currency into another was not in this vision.<br/>
It still isn't and probably ever will. But some people demanded such features.<br/>
And if it is helpful and reasonable I went and tried a couple of things and...

# NMoneys.Exchange

... _NMoneys.Exchange_ is born. This lesser twin project complements _NMoneys_ in such a way that allows conversions of monetary quantities in a given currency into another currency.
Not to disappoint anyone, _NMoneys.Exchange_ is a library. It is not a currency exchange service of any sort nor does it connect to one.

Instead, you can use the extensibility points provided by _NMoneys.Exchange_ to plug in your favorite provider of data.<br/>
Out-of-the-box it is uninteresting: a most than likely incorrect one to one exchange rate is applied in order to convert whichever pair of monetary quantities.

## How do I Use it Then?

Being a library, being some sort of .Net developer is a must.<br/>
Besides, it is not published as a package anymore, so you are bound to compile it and reference it from your projects.

## Quickstart

_NMoneys.Exchange_ targets (only) _netstandard 1.3_ and takes a package dependency on the latest legacy _NMoneys_ version (6.1.2).

### Build sources

As it is not published as a package anymore, building the library requires getting the source code and building the NMoneys.Exchange project that lives in
`/archive/NMoneys.Exchange` either via CLI `dotnet build` or via your favorite editor/IDE.

### Get the legacy binaries
Latest legacy binaries can be downloaded from the [Releases](https://github.com/dgg/nmoneys/releases/tag/5.0.0.0) section within the project's website.

For [NuGet](http://nuget.org/) users, the deprecated [NMoneys.Exchange package](http://nuget.org/List/Packages/NMoneys.Exchange) is still available in the official live feed.

[![NuGet version](https://badge.fury.io/nu/nmoneys.exchange.svg)](https://www.nuget.org/packages/nmoneys.exchange)

### Using the library

Tests are no longer included, so this is the only guidance provided.

#### Converting moneys
Performing conversion operations is really easy. Once the project has a reference to `NMoneys.dll` and to `NMoneys.Exchange.dll` assemblies one can convert from one currency to another using the `.Convert()` and `.TryConvert()` extensions methods on any `Money` instance. By default, that is without any further configuration, conversions are pretty useless as the ratewe get is alway one:

``` csharp
var tenEuro = new Money(10m, CurrencyIsoCode.EUR);

var tenDollar = tenEuro.Convert().To(CurrencyIsoCode.USD);
var tenPounds = tenEuro.Convert().To(Currency.Gbp);
```

#### Configure a provider
As stated, in order to use meaningful rates, one has to implement the `IExchangeRateProvider` interface, or use the already provided `TabulatedExchangeRateProvider` that eases the set up of conversion tables. Once implemented, the provider can be easily configured:

``` csharp
var customProvider = new TabulatedExchangeRateProvider();
customProvider.Add(CurrencyIsoCode.EUR, CurrencyIsoCode.USD, 0);

ExchangeRateProvider.Provider = () => customProvider;

var tenEuro = new Money(10m, CurrencyIsoCode.EUR);
var zeroDollars = tenEuro.Convert().To(CurrencyIsoCode.USD);

// go back to default (optional)
ExchangeRateProvider.Provider = ExchangeRateProvider.Default;
```

Being a static provider, all conversions will use the same configured provider. It is up to the developer to decide the life style of the provider instance to be used.

#### Custom arithmetic
One of tbe reasons for not implementing conversion between quantities of different currencies was the lack of knowledge on the internals of such operations.
Unfortunately this has not changed and the default conversion arithmetics is coded inside the `ExchangeRate` class, which uses standard `decimal` multiplication.
Fortunately, that default behavior can be changed easily by overriding some methods in that class:

``` csharp
public class CustomRateArithmetic : ExchangeRate
{
	public CustomRateArithmetic(CurrencyIsoCode from, CurrencyIsoCode to, decimal rate) : base(from, to, rate) { }

	public override Money Apply(Money from)
	{
		return new Money(0m, To);
	}
}
```
With our implementation of how to apply a rate a provider that uses that custom implementation needs to be implemented and configured:

``` csharp
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
```

#### Overpassing the Provider
If one does not want to use the whole provider architecture, one can go completely custom, implementing its own set of extension methods, which can have as simple or as complex semantics as desired. In this case we want to chain two calls so that it "reads" like English:

``` csharp
public static UsingImplementor Using(this IExchangeConversion conversion, decimal rate)
{
	return new UsingImplementor(conversion.From, rate);
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
```

And those custom extensions can be used on _Money_ instances:

``` csharp
var hundredDollars = new Money(100m, CurrencyIsoCode.USD);

var hundredEuros = hundredDollars.Convert().Using(1m).To(CurrencyIsoCode.EUR);
```
