namespace NMoneys;

public partial class Currency
{
	// random list of static accessors based on currency exchange most used currencies

	/*
		/// <summary>
		/// Australia Dollars
		/// </summary>
		public static readonly Currency Aud;
		/// <summary>
		/// Canada Dollars
		/// </summary>
		public static readonly Currency Cad;
		/// <summary>
		/// Switzerland Francs
		/// </summary>
		public static readonly Currency Chf;
		/// <summary>
		/// China Yuan Renminbi
		/// </summary>
		public static readonly Currency Cny;
		*/
	private static readonly Lazy<Currency> _dkk = new(() => Get(CurrencyIsoCode.DKK));

	/// <summary>
	/// Denmark Kroner
	/// </summary>
	public static Currency Dkk => _dkk.Value;

	private static readonly Lazy<Currency> _eur = new(() => Get(CurrencyIsoCode.EUR));

	/// <summary>
	/// Euro
	/// </summary>
	public static Currency Eur => _eur.Value;

	/*
	/// <summary>
	/// United Kingdom Pounds
	/// </summary>
	public static readonly Currency Gbp;

	/// <summary>
	/// Hong Kong Dollars
	/// </summary>
	public static readonly Currency Hkd;

	/// <summary>
	/// Hungary Forint
	/// </summary>
	public static readonly Currency Huf;

	/// <summary>
	/// India Rupees
	/// </summary>
	public static readonly Currency Inr;

	/// <summary>
	/// Japan Yen
	/// </summary>
	public static readonly Currency Jpy;

	/// <summary>
	/// Mexico Pesos
	/// </summary>
	public static readonly Currency Mxn;

	/// <summary>
	/// Malaysia Ringgits
	/// </summary>
	public static readonly Currency Myr;

	/// <summary>
	/// Norway Kroner
	/// </summary>
	public static readonly Currency Nok;

	/// <summary>
	/// New Zealand Dollars
	/// </summary>
	public static readonly Currency Nzd;

	/// <summary>
	/// Russia Rubles
	/// </summary>
	public static readonly Currency Rub;

	/// <summary>
	/// Sweden Kronor
	/// </summary>
	public static readonly Currency Sek;

	/// <summary>
	/// Singapore Dollars
	/// </summary>
	public static readonly Currency Sgd;

	/// <summary>
	/// Thailand Baht
	/// </summary>
	public static readonly Currency Thb;

	/// <summary>
	/// United States Dollars
	/// </summary>
	public static readonly Currency Usd;

	/// <summary>
	/// South Africa Rand
	/// </summary>
	public static readonly Currency Zar;
	*/

	private static readonly Lazy<Currency> _xxx = new(() => Get(CurrencyIsoCode.XXX));

	/// <summary>
	/// Non-Existing currency
	/// </summary>
	public static Currency Xxx => _xxx.Value;

	private static readonly Lazy<Currency> _xts = new(() => Get(CurrencyIsoCode.XTS));

	/// <summary>
	/// Testing currency
	/// </summary>
	public static Currency Xts => _xts.Value;

}
