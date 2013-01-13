using NUnit.Framework;

namespace NMoneys.Tests.Support
{
	internal class Obsolete
	{
		public static readonly TestCaseData[] ThreeLetterIsoCodes = new[]
		{
			new TestCaseData("EEK"), 
			new TestCaseData("ZMK")
		};

#pragma warning disable 612,618
		public static readonly TestCaseData[] IsoCodes = new[]
		{
			new TestCaseData(CurrencyIsoCode.EEK), 
			new TestCaseData(CurrencyIsoCode.ZMK)
		};
#pragma warning restore 612,618
	}
}
