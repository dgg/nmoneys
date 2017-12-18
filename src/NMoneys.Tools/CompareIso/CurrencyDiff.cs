using System;
using System.Globalization;

namespace NMoneys.Tools.CompareIso
{
	internal class CurrencyDiff
	{
		public string Name { get; private set; }
		public string MinorUnits { get; private set; }

		private CurrencyDiff() { }

		private static readonly StringComparer _cmp = StringComparer.Create(CultureInfo.GetCultureInfo("en"), false);

		public static CurrencyDiff Diff(Currency currency, IsoCurrency scrapped)
		{
			CurrencyDiff diff = null;
			if (!_cmp.Equals(currency.EnglishName, scrapped.Name))
			{
				diff = new CurrencyDiff
				{
					Name = $"'{currency.EnglishName}' / '{scrapped.Name}'"
				};
			}

			if (!scrapped.MinorUnits.Equals(currency.SignificantDecimalDigits))
			{
				diff = diff ?? new CurrencyDiff();
				diff.MinorUnits = $"{currency.SignificantDecimalDigits} / {scrapped.MinorUnits}";
			}
			return diff;
		}
	}
}