using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using Excel;
using NMoneys.Tools.Support;

namespace NMoneys.Tools
{
	internal class ScrapeWebsite : Command
	{
		public static readonly Uri IsoCurrenciesUrl = new Uri("http://www.currency-iso.org/dl_iso_table_a1.xls");
		protected override void DoExecute()
		{
			var doc = new CurrenciesSheet(IsoCurrenciesUrl);

			if (doc.TryLoadFromWebSite())
			{
				ScrappedCurrenciesCollection scrappedCurrencies = doc.SelectCurrencies();
				Currency[] allCurrencies = Currency.FindAll().ToArray();
				showImplementedOnly(scrappedCurrencies, allCurrencies);
				showScrappedOnly(scrappedCurrencies, allCurrencies);
				showDiscrepancies(scrappedCurrencies, allCurrencies.ToDictionary(c => c.NumericCode));
			}
		}

		private void showImplementedOnly(IEnumerable<ScrappedCurrency> scrappedCurrencies, IEnumerable<Currency> allCurrencies)
		{
			WL("The following currencies are defined only in the implemented set:");
			IEnumerable<Currency> implementedOnly = allCurrencies.Except(
				scrappedCurrencies.Select(s => s.ToCurrency()).Where(c => c != null));
			foreach (var implemented in implementedOnly)
			{
				WL(ScrappedCurrency.ToString(implemented));
			}
		}

		private void showScrappedOnly(IEnumerable<ScrappedCurrency> scrappedCurrencies, IEnumerable<Currency> allCurrencies)
		{
			WL("The following currencies are defined only in the web:");
			IEnumerable<ScrappedCurrency> webOnly = scrappedCurrencies.Except(
				allCurrencies.Select(ScrappedCurrency.Create));
			foreach (var web in webOnly)
			{
				WL(web.ToString());
			}
		}

		private void showDiscrepancies(IEnumerable<ScrappedCurrency> scrappedCurrencies, IDictionary<short, Currency> allCurrencies)
		{
			//WL("The following currencies have different significant decimals froim the one defined in the web:");
			foreach (var scrapped in scrappedCurrencies)
			{
				foreach (var diff in scrapped.Diff(allCurrencies[scrapped.NumericCode]))
				{
					WL(diff);
				}
			}
			/*Dictionary<short, int> scrapped = scrappedCurrencies
				.Where(c => c.NumberOfDecimals.HasValue)
				.ToDictionary(c => c.NumericCode, c => c.NumberOfDecimals.GetValueOrDefault());
			Func<short, int?> tryGet = code =>
			                          	{
			                          		int i;
			                          		bool parsed = scrapped.TryGetValue(code, out i);
			                          		return parsed ? i : (default(int?));
			                          	};
			var discrepancies = allCurrencies
				.Where(c => c.SignificantDecimalDigits != tryGet(c.NumericCode))
				.Select(c => new{Code = c.IsoCode, NMoneys = c.SignificantDecimalDigits, Scrapped = tryGet(c.NumericCode)});

			foreach (var discrepancy in discrepancies)
			{
				WL("{0}: {1} (NMoneys) / {2} (Web)", discrepancy.Code, discrepancy.NMoneys, discrepancy.Scrapped);
			}*/
		}
	}

	internal class CurrenciesSheet
	{
		private readonly string _cachedFile;
		private readonly Uri _url;

		public CurrenciesSheet(Uri url)
		{
			_url = url;
			DirectoryInfo binDirectory = Directory.GetParent(Assembly.GetExecutingAssembly().Location);
			_cachedFile = Path.Combine(binDirectory.FullName, Path.GetFileName(url.PathAndQuery));
		}

		private bool _currenciesWereLoaded;
		public bool TryLoadFromWebSite()
		{
			try
			{
				WebClient client = new WebClient();
				client.DownloadFile(_url, _cachedFile);
			}
			catch
			{
				return false;
			}
			return _currenciesWereLoaded = true;
		}

		public ScrappedCurrenciesCollection SelectCurrencies()
		{
			var currencies = new ScrappedCurrenciesCollection();
			if (_currenciesWereLoaded)
			{
				IExcelDataReader dr = null;
				try
				{
					dr = ExcelReaderFactory.CreateBinaryReader(File.OpenRead(_cachedFile));
					var ds = dr.AsDataSet();

					currencies.Add(
						ds.Tables[0].AsEnumerable()
						.Skip(3) // first 3 rows do not contain any info
						.Select(r =>
						{
							string code = r[2].ToString(), numericCode = r[3].ToString(), name = r[1].ToString(), decimals = r[4].ToString();
							return ScrappedCurrency.IsCode(code) && ScrappedCurrency.IsNumericCode(numericCode) ?
								new ScrappedCurrency(code, numericCode, name, decimals) :
								null;
						})
						.ToArray());
				}

				finally
				{
					if (dr != null)
					{
						dr.Close();
						dr.Dispose();
					}
				}
			}
			return currencies;
		}
	}

	internal class ScrappedCurrenciesCollection : IEnumerable<ScrappedCurrency>
	{
		private readonly HashSet<ScrappedCurrency> _currencies;
		public ScrappedCurrenciesCollection()
		{
			_currencies = new HashSet<ScrappedCurrency>();
		}

		public void Add(ScrappedCurrency scrapped)
		{
			if (scrapped != null && scrapped.HasValue) _currencies.Add(scrapped);
		}

		public void Add(ScrappedCurrency[] scrapped)
		{
			for (int i = 0; i < scrapped.Length; i++)
			{
				Add(scrapped[i]);
			}
		}

		public int Count { get { return _currencies.Count; } }

		public IEnumerator<ScrappedCurrency> GetEnumerator()
		{
			return _currencies.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	internal class ScrappedCurrency : IEquatable<ScrappedCurrency>
	{
		public ScrappedCurrency(string code, string numericCode, string name, string numberOfDecimals)
			: this(
				stripSpecialCharacters(code),
				short.Parse(stripSpecialCharacters(numericCode)),
				stripSpecialCharacters(name),
			parse(numberOfDecimals)) { }

		private static int? parse(string s)
		{
			int i = 0;
			bool parsed = int.TryParse(s, out i);
			return parsed ? i : default(int?);
		}

		private ScrappedCurrency(string code, short numericCode, string name, int? numberOfDecimals)
		{
			Code = code;
			NumericCode = numericCode;
			Name = name;
			NumberOfDecimals = numberOfDecimals;
		}

		private static string stripSpecialCharacters(string s)
		{
			return s.Replace("\n", string.Empty)
				.Replace("\t", string.Empty)
				.Replace("\r", string.Empty)
				.Replace("&nbsp;", string.Empty);
		}

		public string Code { get; private set; }
		public short NumericCode { get; private set; }
		public string Name { get; private set; }
		public int? NumberOfDecimals { get; private set; }

		public bool HasValue { get { return Code != null && Name != null && NumericCode > 0; } }

		public static bool IsCode(string codeValue)
		{
			return !string.IsNullOrEmpty(stripSpecialCharacters(codeValue));
		}

		public bool Equals(ScrappedCurrency other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return string.Equals(other.Code, Code, StringComparison.Ordinal);
		}

		public IEnumerable<string> Diff(Currency currency)
		{
			if (!Name.Equals(currency.EnglishName, StringComparison.Ordinal))
				yield return string.Format("{0}: English name '{1}' (WEB) / '{2}' (CONFIG)", Code, Name, currency.EnglishName);

			if (!NumberOfDecimals.Equals(currency.SignificantDecimalDigits))
				yield return string.Format("{0}: Number of decimals '{1}' (WEB) / '{2}' (CONFIG)", Code, NumberOfDecimals, currency.SignificantDecimalDigits);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof(ScrappedCurrency)) return false;
			return Equals((ScrappedCurrency)obj);
		}

		public override int GetHashCode()
		{
			return (Code != null ? Code.GetHashCode() : 0);
		}

		internal static bool IsNumericCode(string numericCode)
		{
			short s;
			return short.TryParse(stripSpecialCharacters(numericCode), out s);
		}

		internal static ScrappedCurrency Create(Currency currency)
		{
			return new ScrappedCurrency(currency.IsoSymbol, currency.NumericCode, currency.EnglishName, currency.SignificantDecimalDigits);
		}

		public Currency ToCurrency()
		{
			Currency obtained;
			Currency.TryGet(Code, out obtained);
			return obtained;
		}

		public override string ToString()
		{
			return Code + " " + NumericCode + " " + Name + " " + NumberOfDecimals;
		}

		public static string ToString(Currency currency)
		{
			return currency.IsoSymbol + " " + currency.NumericCode + " " + currency.EnglishName + " " + currency.SignificantDecimalDigits;
		}
	}
}
