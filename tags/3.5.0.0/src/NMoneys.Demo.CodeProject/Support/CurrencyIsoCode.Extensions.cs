using System.Reflection;

namespace NMoneys.Demo.CodeProject.Support
{
	internal static class CurrencyIsoCodeExtensions
	{
		internal static ICustomAttributeProvider AsAttributeProvider(this CurrencyIsoCode code)
		{
			return typeof(CurrencyIsoCode).GetField(code.ToString());
		}
	}
}
