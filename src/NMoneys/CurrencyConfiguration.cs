using System.Globalization;

namespace NMoneys;

/// <summary>
/// Data to override a given <see cref="Currency"/>.
/// </summary>
public record CurrencyConfiguration()
{
	/// <summary>
	/// Override <see cref="Currency.EnglishName"/>.
	/// </summary>
	public string? EnglishName { get; init; }
	/// <summary>
	/// Override <see cref="Currency.NativeName"/>.
	/// </summary>
	public string? NativeName { get; init; }
	/// <summary>
	/// Override <see cref="Currency.Symbol"/>.
	/// </summary>
	public string? Symbol { get; init; }
	/// <summary>
	/// Override <see cref="Currency.SignificantDecimalDigits"/>.
	/// </summary>
	public byte? SignificantDecimalDigits { get; init; }
	/// <summary>
	/// Override <see cref="Currency.DecimalSeparator"/>.
	/// </summary>
	public string? DecimalSeparator { get; init; }
	/// <summary>
	/// Override <see cref="Currency.GroupSeparator"/>.
	/// </summary>
	public string? GroupSeparator { get; init; }
#pragma warning disable CA1819
	/// <summary>
	/// Override <see cref="Currency.GroupSizes"/>.
	/// </summary>
	public byte[]? GroupSizes { get; init; }
#pragma warning restore CA1819
	/// <summary>
	/// Override <see cref="Currency.NegativePattern"/>.
	/// </summary>
	public byte? NegativePattern { get; init; }
	/// <summary>
	/// Override <see cref="Currency.PositivePattern"/>.
	/// </summary>
	public byte? PositivePattern { get; init; }
	/// <summary>
	/// Override <see cref="Currency.IsObsolete"/>.
	/// </summary>
	public bool? IsObsolete { get; init; }

	/// <summary>
	/// Override <see cref="Currency.Entity"/>.
	/// </summary>
	public ValueTuple<ushort?, string?> Reference { get; init; } = (null, null);

	/// <summary>
	/// Creates a <see cref="Currency"/> configuration override with the data of the provided <paramref name="culture"/>
	/// and its corresponding <see cref="RegionInfo"/>.
	/// </summary>
	/// <param name="culture">Instance with the overriding information (<see cref="CultureInfo.NumberFormat"/> "currency" properties).</param>
	/// <param name="isObsolete">Whether the corresponding override is considered obsolete (<c>false</c> by default).</param>
	/// <returns>An instance with the overriding data.</returns>
	public static CurrencyConfiguration From(CultureInfo culture, bool isObsolete = false) =>
		From(culture, new RegionInfo(culture.Name), isObsolete);

	/// <summary>
	/// Creates a <see cref="Currency"/> configuration override with the data of the provided <paramref name="culture"/>
	/// and <paramref name="region"/>.
	/// </summary>
	/// <param name="culture">Instance with the overriding information (<see cref="CultureInfo.NumberFormat"/> "currency" properties).</param>
	/// <param name="region">Instance with the overriding information (currency English and native names).</param>
	/// <param name="isObsolete">Whether the corresponding override is considered obsolete (<c>false</c> by default).</param>
	/// <returns>An instance with the overriding data.</returns>
	public static CurrencyConfiguration From(CultureInfo culture, RegionInfo region, bool isObsolete = false)
	{
		NumberFormatInfo nf = culture.NumberFormat;
		CurrencyConfiguration configuration = new()
		{
			NativeName = region.CurrencyNativeName,
			EnglishName = region.CurrencyEnglishName,
			Symbol = nf.CurrencySymbol,
			SignificantDecimalDigits = (byte)nf.CurrencyDecimalDigits,
			DecimalSeparator = nf.CurrencyDecimalSeparator,
			GroupSeparator = nf.CurrencyGroupSeparator,
			GroupSizes = nf.CurrencyGroupSizes.Select(s => (byte)s).ToArray(),
			PositivePattern = (byte)nf.CurrencyPositivePattern,
			NegativePattern = (byte)nf.CurrencyNegativePattern,
			IsObsolete = isObsolete
		};
		return configuration;
	}
}
