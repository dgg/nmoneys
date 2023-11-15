using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Reflection;
using NMoneys.Support;

namespace NMoneys;

/// <summary>
/// Contains extension to the type <see cref="CurrencyIsoCode"/>
/// </summary>
public static class IsoCodeExtensions
{
	private const string EQUAL = " = ";

	/// <summary>
	/// Returns a combination of the ISO 4217 code and its numeric value, separated by the equals sign '<code>=</code>'.
	/// </summary>
	[Pure]
	public static string AsValuePair(this CurrencyIsoCode isoCode)
	{
		return isoCode + EQUAL + isoCode.NumericCode();
	}

	/// <summary>
	/// The numeric ISO 4217 code of the <see cref="CurrencyIsoCode"/>
	/// </summary>
	[Pure]
	public static short NumericCode(this CurrencyIsoCode isoCode)
	{
		return (short)isoCode;
	}

	/// <summary>
	/// Returns a padded three digit string representation of the <see cref="NumericCode"/>.
	/// </summary>
	[Pure]
	public static string PaddedNumericCode(this CurrencyIsoCode isoCode)
	{
		return isoCode.NumericCode().ToString("000", CultureInfo.InvariantCulture);
	}

	/// <summary>
	/// The alphabetic ISO 4217 code of the <see cref="CurrencyIsoCode"/>
	/// </summary>
	[Pure]
	public static string AlphabeticCode(this CurrencyIsoCode isoCode)
	{
		return isoCode.ToString();
	}

	/// <summary>
	/// Obtains the instance of Currency associated to the CurrencyIsoCode specified.
	/// </summary>
	/// <param name="isoCode">ISO 4217 code</param>
	/// <returns>The instance of <see cref="Currency"/> represented by the <paramref name="isoCode"/>.</returns>
	/// <seealso cref="Currency.Get(CurrencyIsoCode)"/>
	[Pure]
	public static Currency AsCurrency(this CurrencyIsoCode isoCode)
	{
		return Currency.Get(isoCode);
	}

	/// <summary>
	/// Determines whether the specified objects are equal.
	/// </summary>
	/// <remarks>Is checks equality in a performant manner, regardless of the framework version.</remarks>
	/// <param name="code">The first object of type <see cref="CurrencyIsoCode"/> to compare.</param>
	/// <param name="other">The second object of type <see cref="CurrencyIsoCode"/> to compare.</param>
	/// <returns>true if the specified objects are equal; otherwise, false.</returns>
	public static bool Equals(this CurrencyIsoCode code, CurrencyIsoCode other)
	{
		return Enumeration.FastComparer.Equals(code, other);
	}

	/// <summary>
	/// Asserts whether the code exists in the <see cref="CurrencyIsoCode"/> enumeration, throwing if it doesn't.
	/// </summary>
	/// <param name="code">The enumeration value to assert against.</param>
	/// <exception cref="ArgumentException"><paramref name="code"/> is not defined within <see cref="CurrencyIsoCode"/>.</exception>
	public static void AssertDefined(this CurrencyIsoCode code)
	{
		if (!CheckDefined(code))
		{
			throw UndefinedCodeException.ForCode(code);
		}
	}

	/// <summary>
	/// Returns whether the code exists in the <see cref="CurrencyIsoCode"/> enumeration.
	/// </summary>
	/// <param name="threeLetterIsoCode">The enumeration value to check.</param>
	/// <returns>true if the code is defined within the enum values; otherwise, false.</returns>
	public static bool CheckDefined(this string threeLetterIsoCode)
	{
		return Enum.TryParse<CurrencyIsoCode>(threeLetterIsoCode, ignoreCase: true, out _);
	}

	/// <summary>
	/// Asserts whether the code exists in the <see cref="CurrencyIsoCode"/> enumeration, throwing if it doesn't.
	/// </summary>
	/// <param name="threeLetterIsoCode">The enumeration value to assert against.</param>
	/// <exception cref="ArgumentException"><paramref name="threeLetterIsoCode"/> is not defined within <see cref="CurrencyIsoCode"/>.</exception>
	public static void AssertDefined(this string threeLetterIsoCode)
	{
		if (!CheckDefined(threeLetterIsoCode))
		{
			throw UndefinedCodeException.ForCode(threeLetterIsoCode);
		}
	}

	/// <summary>
	/// Returns whether the code exists in the <see cref="CurrencyIsoCode"/> enumeration.
	/// </summary>
	/// <param name="code">The value to check.</param>
	/// <returns>true if the code is defined within the enum values; otherwise, false.</returns>
	public static bool CheckDefined(this CurrencyIsoCode code)
	{
		return Enum.IsDefined(code);
	}

	internal static FieldInfo FieldOf(this CurrencyIsoCode code)
	{
		FieldInfo? field = typeof(CurrencyIsoCode).GetField(code.ToString());
		return field ?? throw UndefinedCodeException.ForCode(code);
	}

	internal static bool HasAttribute<TAttribute>(this CurrencyIsoCode code) where TAttribute : Attribute
	{
		bool has = code.FieldOf().GetCustomAttribute<TAttribute>() != null;
		return has;
	}

	/// <summary>
	/// Returns whether the code is marked obsolete. <see cref="ObsoleteAttribute"/>.
	/// </summary>
	/// <param name="code">The code to check.</param>
	/// <returns><c>true</c> is obsolete, <c>false</c> otherwise.</returns>
	public static bool IsObsolete(this CurrencyIsoCode code)
	{
		return ObsoleteCurrencies.IsObsolete(code);
	}
}
