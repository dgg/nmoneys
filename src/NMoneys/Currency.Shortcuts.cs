namespace NMoneys;

public partial class Currency
{
	// random list of static accessors based on currency exchange most used currencies


	private static readonly Lazy<Currency> _aud = new(() => Get(CurrencyIsoCode.AUD));

	/// <summary>
	/// Australia Dollars
	/// </summary>
	public static Currency Aud => _aud.Value;

	private static readonly Lazy<Currency> _cad = new(() => Get(CurrencyIsoCode.CAD));

	/// <summary>
	/// Canada Dollars
	/// </summary>
	public static Currency Cad => _cad.Value;

	private static readonly Lazy<Currency> _chf = new(() => Get(CurrencyIsoCode.CHF));

	/// <summary>
	/// Switzerland Francs
	/// </summary>
	public static Currency Chf => _chf.Value;

	private static readonly Lazy<Currency> _cny = new(() => Get(CurrencyIsoCode.CNY));

	/// <summary>
	/// China Yuan Renminbi
	/// </summary>
	public static Currency Cny => _cny.Value;

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

	private static readonly Lazy<Currency> _gbp = new(() => Get(CurrencyIsoCode.GBP));

	/// <summary>
	/// United Kingdom Pounds
	/// </summary>
	public static Currency Gbp => _gbp.Value;

	private static readonly Lazy<Currency> _hkd = new(() => Get(CurrencyIsoCode.HKD));

	/// <summary>
	/// Hong Kong Dollars
	/// </summary>
	public static Currency Hkd => _hkd.Value;

	private static readonly Lazy<Currency> _huf = new(() => Get(CurrencyIsoCode.HUF));

	/// <summary>
	/// Hungary Forint
	/// </summary>
	public static Currency Huf => _huf.Value;

	private static readonly Lazy<Currency> _inr = new(() => Get(CurrencyIsoCode.INR));

	/// <summary>
	/// India Rupees
	/// </summary>
	public static Currency Inr => _inr.Value;

	private static readonly Lazy<Currency> _jpy = new(() => Get(CurrencyIsoCode.JPY));

	/// <summary>
	/// Japan Yen
	/// </summary>
	public static Currency Jpy => _jpy.Value;

	private static readonly Lazy<Currency> _mxn = new(() => Get(CurrencyIsoCode.MXN));

	/// <summary>
	/// Mexico Pesos
	/// </summary>
	public static Currency Mxn => _mxn.Value;

	private static readonly Lazy<Currency> _myr = new(() => Get(CurrencyIsoCode.MYR));

	/// <summary>
	/// Malaysia Ringgits
	/// </summary>
	public static Currency Myr => _myr.Value;

	private static readonly Lazy<Currency> _nok = new(() => Get(CurrencyIsoCode.NOK));

	/// <summary>
	/// Norway Kroner
	/// </summary>
	public static Currency Nok => _nok.Value;

	private static readonly Lazy<Currency> _nzd = new(() => Get(CurrencyIsoCode.NZD));

	/// <summary>
	/// New Zealand Dollars
	/// </summary>
	public static Currency Nzd => _nzd.Value;

	private static readonly Lazy<Currency> _rub = new(() => Get(CurrencyIsoCode.RUB));

	/// <summary>
	/// Russia Rubles
	/// </summary>
	public static Currency Rub => _rub.Value;

	private static readonly Lazy<Currency> _sek = new(() => Get(CurrencyIsoCode.SEK));

	/// <summary>
	/// Sweden Kronor
	/// </summary>
	public static Currency Sek => _sek.Value;

	private static readonly Lazy<Currency> _sgd = new(() => Get(CurrencyIsoCode.SGD));
	/// <summary>
	/// Singapore Dollars
	/// </summary>
	public static Currency Sgd => _sgd.Value;

	private static readonly Lazy<Currency> _thb = new(() => Get(CurrencyIsoCode.THB));
	/// <summary>
	/// Thailand Baht
	/// </summary>
	public static Currency Thb => _thb.Value;

	private static readonly Lazy<Currency> _usd = new(() => Get(CurrencyIsoCode.USD));
	/// <summary>
	/// United States Dollars
	/// </summary>
	public static Currency Usd => _usd.Value;

	private static readonly Lazy<Currency> _zar = new(() => Get(CurrencyIsoCode.ZAR));
	/// <summary>
	/// South Africa Rand
	/// </summary>
	public static Currency Zar => _zar.Value;


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
