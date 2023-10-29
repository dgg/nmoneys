using System.Collections.ObjectModel;

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
	/// Override <see cref="PositivePattern"/>.
	/// </summary>
	public byte? PositivePattern { get; init; }
	/// <summary>
	/// Override <see cref="IsObsolete"/>.
	/// </summary>
	public bool? IsObsolete { get; init; }
}
