using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace NMoneys.Tools.Support
{
	internal class CurrencyInfoComparer : IEqualityComparer<CurrencyInfo>
	{
		private readonly CultureInfo _nativeCulture;
		private readonly StringComparer _ordinal;
		private readonly StringComparer _native;

		public CurrencyInfoComparer(CultureInfo nativeCulture)
		{
			_nativeCulture = nativeCulture;
			_ordinal = StringComparer.Ordinal;
			_native = StringComparer.Create(_nativeCulture, false);
		}

		public bool Equals(CurrencyInfo fromGlobalization, CurrencyInfo fromConfiguration)
		{
			if (ReferenceEquals(fromGlobalization, null) && ReferenceEquals(fromConfiguration, null)) return true;
			if (ReferenceEquals(fromGlobalization, null)) return false;
			if (ReferenceEquals(fromConfiguration, null)) return false;

			return Equals(fromConfiguration.Code, fromGlobalization.Code) &&
				_ordinal.Equals(fromConfiguration.EnglishName, fromGlobalization.EnglishName) &&
				_native.Equals(fromConfiguration.NativeName, fromGlobalization.NativeName) &&
				_native.Equals(fromConfiguration.Symbol, fromGlobalization.Symbol) &&
				fromConfiguration.SignificantDecimalDigits == fromGlobalization.SignificantDecimalDigits &&
				_ordinal.Equals(fromConfiguration.DecimalSeparator, fromGlobalization.DecimalSeparator) &&
				_ordinal.Equals(fromConfiguration.GroupSeparator, fromGlobalization.GroupSeparator) &&
				fromConfiguration.GroupSizes.SequenceEqual(fromGlobalization.GroupSizes) &&
				fromConfiguration.PositivePattern == fromGlobalization.PositivePattern &&
				fromConfiguration.NegativePattern == fromGlobalization.NegativePattern;
		}

		public int GetHashCode(CurrencyInfo obj)
		{
			unchecked
			{
				int result = obj.Code.GetHashCode();
				result = (result * 397) ^ (obj.EnglishName != null ? obj.EnglishName.GetHashCode() : 0);
				result = (result * 397) ^ (obj.NativeName != null ? obj.NativeName.GetHashCode() : 0);
				result = (result * 397) ^ (obj.Symbol != null ? obj.Symbol.GetHashCode() : 0);
				result = (result * 397) ^ obj.SignificantDecimalDigits;
				result = (result * 397) ^ (obj.DecimalSeparator != null ? obj.DecimalSeparator.GetHashCode() : 0);
				result = (result * 397) ^ (obj.GroupSeparator != null ? obj.GroupSeparator.GetHashCode() : 0);
				result = (result * 397) ^ (obj.GroupSizes != null ? obj.GroupSizes.GetHashCode() : 0);
				result = (result * 397) ^ obj.PositivePattern;
				result = (result * 397) ^ obj.NegativePattern;
				return result;
			}
		}

		public IEnumerable<KeyValuePair<string, Pair>> ExtendedEquals(CurrencyInfo fromGlobalization, CurrencyInfo fromConfiguration)
		{
			ICollection<KeyValuePair<string, Pair>> results = new List<KeyValuePair<string, Pair>>(10);

			KeyValuePair<string, Pair>? partial = compare(fromGlobalization.Code.Equals(fromConfiguration.Code), fromGlobalization, fromConfiguration, ci => ci.Code);
			if (partial != null) results.Add(partial.Value);

			partial = compare(_ordinal.Equals(fromGlobalization.EnglishName, fromConfiguration.EnglishName), fromGlobalization, fromConfiguration, c => c.EnglishName);
			if (partial != null) results.Add(partial.Value);

			partial = compare(_native.Equals(fromGlobalization.NativeName, fromConfiguration.NativeName), fromGlobalization, fromConfiguration, c => c.NativeName);
			if (partial != null) results.Add(partial.Value);

			partial = compare(_native.Equals(fromGlobalization.Symbol, fromConfiguration.Symbol), fromGlobalization, fromConfiguration, c => c.Symbol);
			if (partial != null) results.Add(partial.Value);

			partial = compare(fromGlobalization.SignificantDecimalDigits == fromConfiguration.SignificantDecimalDigits, fromGlobalization, fromConfiguration, c => c.SignificantDecimalDigits);
			if (partial != null) results.Add(partial.Value);

			partial = compare(_ordinal.Equals(fromGlobalization.DecimalSeparator, fromConfiguration.DecimalSeparator), fromGlobalization, fromConfiguration, c => c.DecimalSeparator);
			if (partial != null) results.Add(partial.Value);

			partial = compare(_ordinal.Equals(fromGlobalization.GroupSeparator, fromConfiguration.GroupSeparator), fromGlobalization, fromConfiguration, c => c.GroupSeparator);
			if (partial != null) results.Add(partial.Value);

			partial = compare(fromGlobalization.GroupSizes.SequenceEqual(fromConfiguration.GroupSizes), fromGlobalization, fromConfiguration, c => c.GroupSizes);
			if (partial != null) results.Add(partial.Value);

			partial = compare(fromGlobalization.PositivePattern == fromConfiguration.PositivePattern, fromGlobalization, fromConfiguration, c => c.PositivePattern);
			if (partial != null) results.Add(partial.Value);

			partial = compare(fromGlobalization.NegativePattern == fromConfiguration.NegativePattern, fromGlobalization, fromConfiguration, c => c.NegativePattern);
			if (partial != null) results.Add(partial.Value);
			return results;
		}

		private static KeyValuePair<string, Pair>? compare(bool areEqual, CurrencyInfo fromGlobalization, CurrencyInfo fromConfiguration, Expression<Func<CurrencyInfo, object>> member)
		{
			KeyValuePair<string, Pair>? result = null;
			if (!areEqual)
			{
				var valueOf = member.Compile();
				result = new KeyValuePair<string, Pair>(Name.Of(member), new Pair(valueOf(fromGlobalization), valueOf(fromConfiguration)));
			}
			return result;
		}
	}
}
