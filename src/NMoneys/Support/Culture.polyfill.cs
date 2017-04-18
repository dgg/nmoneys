using System.Globalization;

namespace NMoneys.Support
{
	internal class Culture
	{
		public static CultureInfo Get(string name)
		{
#if NET
			return CultureInfo.GetCultureInfo(name);
#else
			return new CultureInfo(name);
#endif
		}
	}
}