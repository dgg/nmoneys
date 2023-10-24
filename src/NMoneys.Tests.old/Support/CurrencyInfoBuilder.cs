using System;
using System.Collections.Generic;
using System.Linq;
using NMoneys.Support;

namespace NMoneys.Tests.Support
{
	internal class CurrencyInfoBuilder
	{
		public CurrencyIsoCode Code { get; set; }
		public string EnglishName { get; set; }
		public string NativeName { get; set; }

		public string Symbol { get; set; }
		public int SignificantDecimalDigits { get; set; }
		public string DecimalSeparator { get; set; }
		public string GroupSeparator { get; set; }
		public int[] GroupSizes { get; set; }
		public int PositivePattern { get; set; }
		public int NegativePattern { get; set; }
		public bool Obsolete { get; set; }
		public string EntityName { get; set; }

		public CurrencyInfo Build()
		{
			return new CurrencyInfo(Code, EnglishName, NativeName,
				UnicodeSymbol.FromSymbol(Symbol).TokenizedCodePoints,
				SignificantDecimalDigits, DecimalSeparator, GroupSeparator,
				NMoneys.Support.GroupSizes.FromSizes(GroupSizes).TokenizedSizes,
				PositivePattern, NegativePattern, Obsolete,
				string.IsNullOrEmpty(EntityName) ? null : new CharacterReference(EntityName));
		}

		public IEqualityComparer<CurrencyInfo> Comparer
		{
			get { return new CurrencyInfoComparer(); }
		}

		class CurrencyInfoComparer : IEqualityComparer<CurrencyInfo>
		{
			public bool Equals(CurrencyInfo x, CurrencyInfo y)
			{
				if (ReferenceEquals(x, null) && ReferenceEquals(y, null)) return true;
				if (ReferenceEquals(null, x)) return false;
				if (ReferenceEquals(null, y)) return true;
				return Equals(x.Code, y.Code) &&
					string.Equals(x.EnglishName, y.EnglishName, StringComparison.Ordinal) &&
					string.Equals(x.NativeName, y.NativeName, StringComparison.Ordinal) &&
					string.Equals(x.Symbol, y.Symbol, StringComparison.Ordinal) &&
					x.SignificantDecimalDigits == y.SignificantDecimalDigits &&
					string.Equals(x.DecimalSeparator, y.DecimalSeparator, StringComparison.Ordinal) &&
					string.Equals(x.GroupSeparator, y.GroupSeparator, StringComparison.Ordinal) &&
					x.GroupSizes.SequenceEqual(y.GroupSizes) &&
					x.PositivePattern == y.PositivePattern &&
					x.NegativePattern == y.NegativePattern;
			}

			public int GetHashCode(CurrencyInfo obj)
			{
				return 0;
			}
		}
	}
}
