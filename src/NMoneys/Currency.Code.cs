using System.Diagnostics.Contracts;
using NMoneys.Support;

namespace NMoneys;

public sealed partial class Currency
{
	/// <summary>
	/// Contains factory and utility methods for <see cref="CurrencyIsoCode"/>
	/// </summary>
	public static class Code
	{
		/// <summary>
		/// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent <see cref="CurrencyIsoCode"/>.
		/// </summary>
		/// <remarks>The <paramref name="isoCode"/> can represent an alphabetic or a numeric currency code.
		/// <para>In the case of alphabetic codes, parsing is case-insensitive.</para>
		/// <para>Only defined numeric codes can be parsed.</para></remarks>
		/// <param name="isoCode">A string containing the name or value to convert.</param>
		/// <returns>An object of type <see cref="CurrencyIsoCode"/> whose value is represented by value.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="isoCode"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="isoCode"/> does not represent a defined alphabetic or numeric code.</exception>
		/// <seealso cref="IsoCodeExtensions.AlphabeticCode"/>
		/// <seealso cref="IsoCodeExtensions.NumericCode"/>
		[Pure]
		public static CurrencyIsoCode Parse(string isoCode)
		{
			ArgumentNullException.ThrowIfNull(isoCode, nameof(isoCode));
			CurrencyIsoCode parsed = ParseArgument(isoCode, nameof(isoCode));
			parsed.AssertDefined();
			return parsed;
		}

		/// <summary>
		/// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent <see cref="CurrencyIsoCode"/>.
		/// </summary>
		/// <remarks>The <paramref name="isoCode"/> can represent an alphabetic or a numeric currency code.
		/// <para>In the case of alphabetic codes, parsing is case-insensitive.</para>
		/// <para>Only defined numeric codes can be parsed.</para></remarks>
		/// <para>If the conversion fails, <paramref name="defaultValue"/> will be returned.</para>
		/// <param name="isoCode">A string containing the name or value to convert.</param>
		/// <param name="defaultValue">The value to return if the conversion fails.</param>
		/// <returns>An object of type <see cref="CurrencyIsoCode"/> whose value is represented by value.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="isoCode"/> is null.</exception>
		/// <exception cref="UndefinedCodeException"><paramref name="isoCode"/> does not represent a defined alphabetic or numeric code.</exception>
		/// <seealso cref="IsoCodeExtensions.AlphabeticCode"/>
		/// <seealso cref="IsoCodeExtensions.NumericCode"/>
		public static CurrencyIsoCode Parse(string isoCode, CurrencyIsoCode defaultValue)
		{
			if (!TryParse(isoCode, out var parsed))
			{
				throw UndefinedCodeException.ForCode(isoCode);
			}
			return parsed.GetValueOrDefault(defaultValue);
		}

		/// <summary>
		/// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent <see cref="CurrencyIsoCode"/>.
		/// The return value indicates whether the conversion succeeded.
		/// </summary>
		/// <remarks>The <paramref name="isoCode"/> can represent an alphabetic or a numeric currency code.
		/// <para>In the case of alphabetic codes, parsing is case-insensitive.</para>
		/// <para>Only defined numeric codes can be parsed.</para></remarks>
		/// <param name="isoCode">A string containing the name or value to convert.</param>
		/// <param name="parsed">When this method returns, contains the parsed <see cref="CurrencyIsoCode"/> if the parsing succeeded, or is null if the parsing failed.</param>
		/// <returns>true if the value <paramref name="isoCode"/> was parsed successfully; otherwise, false.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="isoCode"/> is null.</exception>;
		public static bool TryParse(string isoCode, out CurrencyIsoCode? parsed)
		{
			ArgumentNullException.ThrowIfNull(isoCode, nameof(isoCode));
			bool result = Enum.TryParse<CurrencyIsoCode>(isoCode.ToUpperInvariant(), out var code) &&
			              code.CheckDefined();
			parsed = result ? code : null;
			return result;
		}

		/// <summary>
		/// Used to parse the ISO codes arguments.
		/// </summary>
		/// <remarks>Is case-insensitive.</remarks>
		internal static CurrencyIsoCode ParseArgument(string isoCode, string argumentName)
		{
			return Enum.Parse<CurrencyIsoCode>(isoCode.ToUpperInvariant());
		}

		/// <summary>
		/// Converts the specified 16-bit signed integer to a <see cref="CurrencyIsoCode"/>.
		/// </summary>
		/// <para>The conversion is safe, in the sense that the value has to be defined within the values of the enumeration to be converted, but throws an exception when it cannot.</para>
		/// <param name="numericCode">The value to be converted.</param>
		/// <returns>An instance of the enumeration set to <paramref name="numericCode"/>.</returns>
		/// <exception cref="ArgumentException"><paramref name="numericCode"/> is not defined within the values of <see cref="CurrencyIsoCode"/>.</exception>
		public static CurrencyIsoCode Cast(ushort numericCode)
		{
			if (!TryCast(numericCode, out CurrencyIsoCode? code))
			{
				throw UndefinedCodeException.ForCode(numericCode);
			}

			return code.GetValueOrDefault();
		}

		/// <summary>
		/// Converts the the specified 16-bit signed integer to an equivalent <see cref="CurrencyIsoCode"/>. The return value indicates whether the conversion succeeded.
		/// </summary>
		/// <remarks>When the conversion is successful, the returned value is guaranteed to contain a value.</remarks>
		/// <param name="numericCode">The value to be converted.</param>
		/// <param name="converted">When this method returns, contains an object of type <see cref="Nullable{CurrencyIsoCode}"/> whose value is represented by <paramref name="numericCode"/>; otherwise, false.
		/// This parameter is passed uninitialized.</param>
		/// <returns>true if the <paramref name="numericCode"/> parameter was converted successfully; otherwise, false.</returns>
		public static bool TryCast(ushort numericCode, out CurrencyIsoCode? converted)
		{
			if (!Enum.IsDefined(typeof(CurrencyIsoCode), numericCode))
			{
				converted = null;
				return false;
			}

			converted = (CurrencyIsoCode)numericCode;
			return true;
		}

		/// <summary>
		/// Provides a performant implementation of <see cref="IEqualityComparer{T}"/> for instances of <see cref="CurrencyIsoCode"/>.
		/// </summary>
		/// <remarks><seealso href="http://www.codeproject.com/Articles/33528/Accelerating-Enum-Based-Dictionaries-with-Generic">Source</seealso></remarks>
		public static readonly IEqualityComparer<CurrencyIsoCode> Comparer = Enumeration.FastComparer;
	}
}
