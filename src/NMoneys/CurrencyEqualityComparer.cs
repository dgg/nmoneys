using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NMoneys.Support;

namespace NMoneys
{
	/// <summary>
	/// Exposes a method that compares two currencies in a "complete" manner.
	/// </summary>
	/// <remarks><see cref="Currency"/> already implements <see cref="IEquatable{Currency}"/>, but only certain members are compared.
	/// In this class all members are compared.</remarks>
	public class CurrencyEqualityComparer : IEqualityComparer<Currency>
	{
		/// <summary>Default string comparison for the <see cref="Currency.EnglishName"/> member.</summary>
		/// <remarks>A case-sensitive string comparison using the word comparison rules of a neutral-english culture (LCID 0009, <c>en</c>) is used.</remarks>
		public static IEqualityComparer<string> DefaultEnglishComparer => StringComparer.Create(Culture.Get("en"), false);

		/// <summary>Default string comparison for the <see cref="Currency.NativeName"/> member.</summary>
		/// <remarks>A case-sensitive string comparison using the word comparison rules of the invariant culture is used.</remarks>
		public static IEqualityComparer<string> DefaultNativeComparer => StringComparer.InvariantCulture;

		/// <summary>Default string comparison for the <see cref="Currency.Symbol"/> member.</summary>
		/// <remarks>A case-sensitive string comparison using the word comparison rules of the invariant culture is used.</remarks>
		public static IEqualityComparer<string> DefaultSymbolComparer => StringComparer.InvariantCulture;

		/// <summary>Default string comparison for the <see cref="Currency.DecimalSeparator"/> and <see cref="Currency.GroupSeparator"/> members.</summary>
		/// <remarks>An case-sensitive ordinal string comparison is used.</remarks>
		public static IEqualityComparer<string> DefaultSeparatorComparer => StringComparer.Ordinal;

		private readonly IEqualityComparer<string> _englishNameComparer, _nativeNameComparer, _symbolComparer, _separatorComparer;

		/// <summary>
		/// Creates an instance with the provided comparer.
		/// </summary>
		/// <remarks>This is a the preferred constructors as culture-specific comparisons should be favoured for culture sensitive strings.</remarks>
		/// <param name="nativeNameComparer">Comparer for the <see cref="Currency.NativeName"/> property.</param>
		public CurrencyEqualityComparer(IEqualityComparer<string> nativeNameComparer) : this(nativeNameComparer, DefaultSymbolComparer) { }

		/// <summary>
		/// Creates an instance with the provided comparers.
		/// </summary>
		/// <remarks>This is a preferred constructors as culture-specific comparisons should be favoured for culture sensitive strings.</remarks>
		/// <param name="nativeNameComparer">Comparer for the <see cref="Currency.NativeName"/> property.</param>
		/// <param name="symbolComparer">Comparer for the <see cref="Currency.Symbol"/> property.</param>
		public CurrencyEqualityComparer(IEqualityComparer<string> nativeNameComparer, IEqualityComparer<string> symbolComparer)
			: this(nativeNameComparer, symbolComparer, DefaultSeparatorComparer) { }

		/// <summary>
		/// Creates an instance with the provided comparers.
		/// </summary>
		/// <param name="nativeNameComparer">Comparer for the <see cref="Currency.NativeName"/> property.</param>
		/// <param name="symbolComparer">Comparer for the <see cref="Currency.Symbol"/> property.</param>
		/// <param name="separatorComparer">Comparer for the <see cref="Currency.DecimalSeparator"/> and <see cref="Currency.GroupSeparator"/> properties.</param>
		public CurrencyEqualityComparer(IEqualityComparer<string> nativeNameComparer, IEqualityComparer<string> symbolComparer, IEqualityComparer<string> separatorComparer)
			: this(nativeNameComparer, symbolComparer, separatorComparer, DefaultEnglishComparer) { }

		/// <summary>
		/// Creates an instance with the provided comparers.
		/// </summary>
		/// <remarks>Provides the maximum control over how string comparisons will be performed.</remarks>
		/// <param name="nativeNameComparer">Comparer for the <see cref="Currency.NativeName"/> property.</param>
		/// <param name="symbolComparer">Comparer for the <see cref="Currency.Symbol"/> property.</param>
		/// <param name="separatorComparer">Comparer for the <see cref="Currency.DecimalSeparator"/> and <see cref="Currency.GroupSeparator"/> properties.</param>
		/// <param name="englishNameComparer">Comparer for the <see cref="Currency.EnglishName"/> property.</param>
		public CurrencyEqualityComparer(IEqualityComparer<string> nativeNameComparer, IEqualityComparer<string> symbolComparer, IEqualityComparer<string> separatorComparer, IEqualityComparer<string> englishNameComparer)
		{
			_nativeNameComparer = nativeNameComparer;
			_symbolComparer = symbolComparer;
			_separatorComparer = separatorComparer;
			_englishNameComparer = englishNameComparer;
		}

		/// <summary>
		/// Gets a default implementation of the <see cref="CurrencyEqualityComparer"/> with default comparers.
		/// </summary>
		/// <seealso cref="DefaultNativeComparer" />
		/// <seealso cref="DefaultSymbolComparer" />
		/// <seealso cref="DefaultSeparatorComparer" />
		/// <seealso cref="DefaultEnglishComparer" />
		public static readonly CurrencyEqualityComparer Default = new CurrencyEqualityComparer(DefaultNativeComparer, DefaultSymbolComparer, DefaultSeparatorComparer, DefaultEnglishComparer);

		/// <summary>
		/// Determines whether the specified currencies are equal.
		/// </summary>
		/// <returns>true if the specified currencies are equal; otherwise, false.</returns>
		/// <param name="x">The first <see cref="Currency"/> to compare.</param>
		/// <param name="y">The second <see cref="Currency"/> to compare.</param>
		public bool Equals(Currency x, Currency y)
		{
			if (ReferenceEquals(x, y)) return true;
			if (x == null || y == null) return false;

			return x.IsoCode == y.IsoCode &&
				_englishNameComparer.Equals(x.EnglishName, y.EnglishName) &&
				_nativeNameComparer.Equals(x.NativeName, y.NativeName) &&
				_symbolComparer.Equals(x.Symbol, y.Symbol) &&
				x.SignificantDecimalDigits == y.SignificantDecimalDigits &&
				_separatorComparer.Equals(x.DecimalSeparator, y.DecimalSeparator) &&
				_separatorComparer.Equals(x.GroupSeparator, y.GroupSeparator) &&
				x.GroupSizes.SequenceEqual(y.GroupSizes) &&
				x.PositivePattern == y.PositivePattern &&
				x.NegativePattern == y.NegativePattern &&
				x.IsObsolete == y.IsObsolete &&
				x.Entity.Equals(y.Entity);
		}

		/// <summary>
		/// Returns a hash code for the specified currency.
		/// </summary>
		/// <returns>
		/// A hash code for the specified currency.
		/// </returns>
		/// <param name="obj">The <see cref="Currency"/> for which a hash code is to be returned.</param>
		/// <exception cref="ArgumentNullException"><paramref name="obj"/> is null.</exception>
		public int GetHashCode(Currency obj)
		{
			Guard.AgainstNullArgument("obj", obj);
			return obj.GetHashCode();
		}
	}
}
