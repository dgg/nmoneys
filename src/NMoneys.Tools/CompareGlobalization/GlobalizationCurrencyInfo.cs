using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NMoneys.Support;

namespace NMoneys.Tools.CompareGlobalization
{
	internal class GlobalizationCurrencyInfo : IEquatable<CurrencyInfo>
	{
		public CultureInfo Culture { get; }
		public CurrencyInfo Info { get; }

		private static readonly StringComparer _ordinal = StringComparer.Ordinal;
		private readonly StringComparer _native;

		private GlobalizationCurrencyInfo(CultureInfo ci, RegionInfo ri)
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
				nf.CurrencyNegativePattern,
				false,
				CharacterReference.Empty);

			_native = StringComparer.Create(ci, false);
		}

		private static GlobalizationCurrencyInfo tryBuild(CultureInfo ci)
		{
			var ri = new RegionInfo(ci.Name);
			CurrencyIsoCode? code;
			Enumeration.TryParse(ri.ISOCurrencySymbol, out code);
			if (code != null)
			{
				return new GlobalizationCurrencyInfo(ci, ri);
			}
			Console.WriteLine("Currency {0} not defined for Globalization Culture {1} [{2}]", ri.ISOCurrencySymbol, ci.Name,
				ci.EnglishName);
			return null;
		}

		public static IEnumerable<GlobalizationCurrencyInfo> LoadFromGlobalization()
		{
			var loaded = CultureInfo.GetCultures(CultureTypes.AllCultures)
				.Where(c => !c.IsNeutralCulture && !c.Equals(CultureInfo.InvariantCulture))
				.Select(tryBuild)
				.Where(ci => ci != null);

			return loaded;
		}

		public bool Equals(CurrencyInfo fromConfiguration)
		{
			if (ReferenceEquals(Info, null) && ReferenceEquals(fromConfiguration, null)) return true;
			if (ReferenceEquals(Info, null)) return false;
			if (ReferenceEquals(fromConfiguration, null)) return false;

			return Equals(fromConfiguration.Code, Info.Code) &&
			       _ordinal.Equals(fromConfiguration.EnglishName, Info.EnglishName) &&
			       _native.Equals(fromConfiguration.NativeName, Info.NativeName) &&
			       _native.Equals(fromConfiguration.Symbol, Info.Symbol) &&
			       fromConfiguration.SignificantDecimalDigits == Info.SignificantDecimalDigits &&
			       _ordinal.Equals(fromConfiguration.DecimalSeparator, Info.DecimalSeparator) &&
			       _ordinal.Equals(fromConfiguration.GroupSeparator, Info.GroupSeparator) &&
			       fromConfiguration.GroupSizes.SequenceEqual(Info.GroupSizes) &&
			       fromConfiguration.PositivePattern == Info.PositivePattern &&
			       fromConfiguration.NegativePattern == Info.NegativePattern;
		}

		public DiffCollection Compare(CurrencyInfo fromConfiguration)
		{
			DiffCollection diffs = new DiffCollection()
			{
				Diff.For(Info, fromConfiguration, c => c.Code, (x, y) => x == y),
				Diff.For(Info, fromConfiguration, c => c.EnglishName, (x, y) => _ordinal.Equals(x, y)),
				Diff.For(Info, fromConfiguration, c => c.NativeName, (x, y) => _native.Equals(x, y)),
				Diff.For(Info, fromConfiguration, c => c.Symbol, (x, y) => _native.Equals(x, y)),
				Diff.For(Info, fromConfiguration, c => c.SignificantDecimalDigits, (x, y) => x == y),
				Diff.For(Info, fromConfiguration, c => c.DecimalSeparator, (x, y) => _ordinal.Equals(x, y)),
				Diff.For(Info, fromConfiguration, c => c.GroupSeparator, (x, y) => _ordinal.Equals(x, y)),
				Diff.For(Info, fromConfiguration, c => c.GroupSizes, (x, y) => x.SequenceEqual(y)),
				Diff.For(Info, fromConfiguration, c => c.PositivePattern, (x, y) => x == y),
				Diff.For(Info, fromConfiguration, c => c.NegativePattern, (x, y) => x == y)
			};

			return diffs;
		}
	}
}