using System.Diagnostics.Contracts;
using System.Globalization;
using NMoneys.Support;

namespace NMoneys;

public sealed partial class Currency
{
	///<summary>
	/// Occurs when an obsolete currency is created.
	///</summary>
	/// <remarks>
	/// Do remember to unsubscribe from this event when you are no longer interested it its ocurrence.
	/// Failing to do so can prevent your objects from being garbage collected and result in a memory leak.
	/// <para>By its static nature, the notification is available even when no instance of the class is existing yet.
	/// This very same nature will cause that subscribers are notified for occurrences that are "far" from the code that is likely to raise an event in concurrent systems.
	/// For example, another thread could make the event to raise and a totally unrelated code will get the notification. This may well be the desired effect,
	/// but awareness need to be raised for when it is not the desired effect.</para>
	/// <para>Currencies are transient entities in the real world, getting deprecated and/or substituted.
	/// When a currency that is no longer current is created this event will be raised. This can happen in a number of cases:
	/// <list type="bullet">
	/// <item><description>A <see cref="Currency"/> factory method is used.</description></item>
	/// <item><description>A <see cref="Currency"/> instance gets deserialized.</description></item>
	/// <item><description>A <see cref="Money"/> instance gets created.</description></item>
	/// <item><description>A <see cref="Money"/> instance gets deserialized.</description></item>
	/// </list>
	/// </para>
	/// </remarks>
	/// <seealso cref="Get(CurrencyIsoCode)"/>
	/// <seealso cref="Get(string)"/>
	/// <seealso cref="Get(CultureInfo)"/>
	/// <seealso cref="Get(RegionInfo)"/>
	/// <seealso cref="TryGet(CurrencyIsoCode, out Currency)"/>
	/// <seealso cref="TryGet(string, out Currency)"/>
	/// <seealso cref="TryGet(CultureInfo, out Currency)"/>
	/// <seealso cref="TryGet(RegionInfo, out Currency)"/>
	[Pure]
	public static event EventHandler<ObsoleteCurrencyEventArgs>? ObsoleteCurrency;

	private static void onObsoleteCurrency(ObsoleteCurrencyEventArgs e)
	{
		EventHandler<ObsoleteCurrencyEventArgs>? handler = ObsoleteCurrency;
		handler?.Invoke(null, e);
	}

	internal static void RaiseIfObsolete(CurrencyIsoCode code)
	{
		if (ObsoleteCurrencies.IsObsolete(code))
		{
			onObsoleteCurrency(new ObsoleteCurrencyEventArgs(code));
		}
	}

	internal static void RaiseIfObsolete(Currency currency)
	{
		if (ObsoleteCurrencies.IsObsolete(currency))
		{
			onObsoleteCurrency(new ObsoleteCurrencyEventArgs(currency.IsoCode));
		}
	}
}
