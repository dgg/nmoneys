using System.Globalization;
using System.Text.Json;
using NMoneys.Serialization.Text_Json;

namespace NMoneys.Serialization.Tests.EFCore;

public class CompactMoneyConverter
{
	public static string ConvertTo(Money money)
	{
		return money.Format("{2} {0}", CultureInfo.InvariantCulture);
	}

	public static Money ConvertFrom(string str)
	{
		ReadOnlySpan<char> span = str.AsSpan();
		CurrencyIsoCode currency = Enum.Parse<CurrencyIsoCode>(span[..3]);
		currency.AssertDefined();
		decimal amount = decimal.Parse(span[4..], CultureInfo.InvariantCulture);
		return new Money(amount, currency);
	}
}

public class JsonMoneyConverter
{
	public static string ConvertTo(Money money)
	{
		return JsonSerializer.Serialize(money, new JsonSerializerOptions() { Converters = { new MoneyConverter() } });
	}

	public static Money ConvertFrom(string str)
	{
		return JsonSerializer.Deserialize<Money>(str,
			new JsonSerializerOptions() { Converters = { new MoneyConverter() } });
	}
}
