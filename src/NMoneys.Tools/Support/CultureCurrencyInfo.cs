using System;
using System.Globalization;
using NMoneys.Support;

namespace NMoneys.Tools.Support
{
	internal class CultureCurrencyInfo
	{
		public CultureInfo Culture { get; private set; }
		public CurrencyInfo Info { get; private set; }

		private CultureCurrencyInfo(CultureInfo ci, RegionInfo ri)
		{
			Culture = ci;
			var nf = ci.NumberFormat;

			Info = new CurrencyInfo(
			Enumeration.Parse<CurrencyIsoCode>(ri.ISOCurrencySymbol),
			ri.CurrencyEnglishName,
			ri.CurrencyNativeName,
			UnicodeSymbol.FromSymbol(ri.CurrencySymbol).TokenizedCodePoints,
			nf.CurrencyDecimalDigits,
			nf.CurrencyDecimalSeparator,
			nf.CurrencyGroupSeparator,
			GroupSizes.FromSizes(nf.CurrencyGroupSizes).TokenizedSizes,
			nf.CurrencyPositivePattern,
			nf.CurrencyNegativePattern);
		}

		public static CultureCurrencyInfo TryBuild(CultureInfo ci)
		{
			var ri = new RegionInfo(ci.LCID);
			CurrencyIsoCode? code;
			Enumeration.TryParse(ri.ISOCurrencySymbol, out code);
			if (code != null)
			{
				return new CultureCurrencyInfo(ci, ri);
			}
			Console.WriteLine("Currency {0} not defined for Globalization Culture {1} [{2}]", ri.ISOCurrencySymbol, ci.Name, ci.EnglishName);
			return null;
		}
	}
}
