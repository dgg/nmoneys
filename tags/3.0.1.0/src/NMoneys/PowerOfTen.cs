namespace NMoneys
{
	internal class PowerOfTen
	{
		private static readonly uint[] _positive = new[] { 1u, 10u, 100u, 1000u, 10000u };
		private static readonly decimal[] _negative = new[] { 1m, .1m, .01m, .001m, .0001m, .00001m };

		/// <summary>
		/// 10 ^ <see cref="Currency.SignificantDecimalDigits"/>.
		/// </summary>
		public static uint Positive(Currency currency)
		{
			return Positive(currency.SignificantDecimalDigits);
		}

		/// <summary>
		/// 10 ^ <paramref name="numberOfDecimals"/>.
		/// </summary>
		public static uint Positive(int numberOfDecimals)
		{
			return _positive[numberOfDecimals];
		}

		/// <summary>
		/// 10 ^ -<see cref="Currency.SignificantDecimalDigits"/>.
		/// </summary>
		public static decimal Negative(Currency currency)
		{
			return Negative(currency.SignificantDecimalDigits);
		}

		/// <summary>
		/// 10 ^ -<paramref name="numberOfDecimals"/>.
		/// </summary>
		public static decimal Negative(int numberOfDecimals)
		{
			return _negative[numberOfDecimals];
		}
	}
}
