namespace NMoneys.Tests.Support;

public partial class Doez: Does
{
	public partial class Raise
	{
		public static ObsoleteCurrencyRaised ObsoleteEvent(byte times = 1)
		{
			return new ObsoleteCurrencyRaised(times);
		}
	}
}
