namespace NMoneys;

public partial class Currency
{
	/// <summary>
	/// Euro
	/// </summary>
	public static Currency Euro => _eur.Value;

	/*
	/// <summary>
	/// United States Dollars
	/// </summary>
	public static readonly Currency Dollar;

	/// <summary>
	/// United Kingdom Pounds
	/// </summary>
	public static readonly Currency Pound;
	*/


	/// <summary>
	/// Non-Existing currency
	/// </summary>
	public static Currency None => _xxx.Value;

	/// <summary>
	/// Testing currency
	/// </summary>
	public static Currency Test => _xts.Value;
}
