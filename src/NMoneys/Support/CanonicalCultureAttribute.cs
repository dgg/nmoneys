using System.Globalization;

namespace NMoneys.Support;

/// <summary>
/// Represents that a .NET Framework culture is suitable for representing an ISO currency
/// </summary>
/// <remarks>For internal verification purposes, it does not have any effect on currency runtime.</remarks>
[AttributeUsage(AttributeTargets.Field)]
public sealed class CanonicalCultureAttribute : Attribute
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CanonicalCultureAttribute"/> class.
	/// </summary>
	/// <param name="cultureName">Name of the corresponding <see cref="CultureInfo"/> instance.</param>
	public CanonicalCultureAttribute(string cultureName)
	{
		CultureName = cultureName;
	}

	/// <summary>
	/// Name of the <see cref="CultureInfo"/> that is source of the information
	/// </summary>
	public string CultureName { get; }

	/// <summary>
	/// Indicates whether one or more values of the currency are different from ones specified by the <see cref="CultureInfo"/>.
	/// </summary>
	/// <remarks>
	/// Except <see cref="Currency.EnglishName"/>, which is controlled by the ISO currency list.
	/// </remarks>
	public bool Overwritten { get; set; }

	/// <summary>
	/// Source of the information
	/// </summary>
	/// <remarks>It is a delegate as we do not want to use their value within the program but just as a support
	/// to the tools that compare the information in the .NET Framework with the information in the Xml.</remarks>
	public Func<CultureInfo> Culture { get { return () => CultureInfo.GetCultureInfo(CultureName); } }
}
