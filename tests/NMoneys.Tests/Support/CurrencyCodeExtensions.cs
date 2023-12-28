using System.Reflection;

namespace NMoneys.Tests.Support;

internal static class CurrencyCodeExtensions
{
	internal static ICustomAttributeProvider? AsAttributeProvider(this CurrencyIsoCode code)
	{
		return typeof(CurrencyIsoCode).GetField(code.ToString());
	}
}
