namespace NMoneys.Support;

internal static class CurrencyNegativePattern
{
	public static int TranslateToNumberNegativePattern(int currencyNegativePattern)
	{
		int numberNegativePattern = -1;
		switch (currencyNegativePattern)
		{
			case 0:
			case 4:
			case 14:
			case 15:
				numberNegativePattern = 0;
				break;
			case 1:
			case 2:
			case 5:
			case 8:
			case 12:
				numberNegativePattern = 1;
				break;
			case 9:
				numberNegativePattern = 2;
				break;
			case 3:
			case 6:
			case 7:
			case 11:
			case 13:
				numberNegativePattern = 3;
				break;
			case 10:
				numberNegativePattern = 4;
				break;
			default:
				new Range<int>(0.Close(), 15.Close()).AssertArgument(nameof(currencyNegativePattern), currencyNegativePattern);
				break;
		}

		return numberNegativePattern;
	}
}
