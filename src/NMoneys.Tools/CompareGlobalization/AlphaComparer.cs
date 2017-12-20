using System;
using System.Collections.Generic;

namespace NMoneys.Tools.CompareGlobalization
{
	internal class AlphaComparer : IComparer<CurrencyIsoCode>
	{
		public int Compare(CurrencyIsoCode x, CurrencyIsoCode y)
		{
			return StringComparer.Ordinal.Compare(x.AlphabeticCode(), y.AlphabeticCode());
		}
	}
}