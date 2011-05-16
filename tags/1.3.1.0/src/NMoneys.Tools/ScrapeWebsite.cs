using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HtmlAgilityPack;

namespace NMoneys.Tools
{
	internal class ScrapeWebsite : Command
	{
		public static readonly Uri IsoCurrenciesUrl = new Uri("http://www.iso.org/iso/support/currency_codes_list-1.htm");
		protected override void DoExecute()
		{
			var doc = new CurrenciesDocument();

			if (doc.TryLoadFromWebSite(IsoCurrenciesUrl))
			{
				ScrappedCurrenciesCollection scrappedCurrencies = doc.SelectCurrencies();
				Currency[] allCurrencies = Currency.FindAll().ToArray();
				showImplementedOnly(scrappedCurrencies, allCurrencies);
				showScrappedOnly(scrappedCurrencies, allCurrencies);
			}
		}

		private void showImplementedOnly(IEnumerable<ScrappedCurrency> scrappedCurrencies, IEnumerable<Currency> allCurrencies)
		{
			WL("The following currencies are defined only in the implemented set:");
			IEnumerable<Currency> implementedOnly = allCurrencies.Except(
				scrappedCurrencies.Select(s => s.ToCurrency()).Where(c => c!= null));
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
	}

	internal class CurrenciesDocument : HtmlDocument
	{
		private bool _currenciesWereLoaded;
		public bool TryLoadFromWebSite(Uri url)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			bool canBeLoaded = response.StatusCode == HttpStatusCode.OK;
			if (canBeLoaded) Load(response.GetResponseStream(), true);
			_currenciesWereLoaded = canBeLoaded;
			return canBeLoaded;
		}

		public ScrappedCurrenciesCollection SelectCurrencies()
		{
			ScrappedCurrenciesCollection currencies = new ScrappedCurrenciesCollection();
			if (_currenciesWereLoaded)
			{
				HtmlNodeCollection currencyNodes = DocumentNode.SelectNodes("//div[@class='colGroup']/div/table/tbody/tr");
				// start with one as headers are not important
				for (int i = 1; i < currencyNodes.Count; i++)
				{
					HtmlNode currencyNode = currencyNodes[i];
					var created = parse(currencyNode);
					currencies.Add(created);
				}
			}
			return currencies;
		}

		private static ScrappedCurrency[] parse(HtmlNode currencyNode)
		{
			List<ScrappedCurrency> built = new List<ScrappedCurrency>(3);
			var breaks = currencyNode.SelectNodes("td/br");
			if (containsOneCurrency(breaks))
			{
				built.Add(parseOneCurrecy(currencyNode));
			}
			else if (containsTwoCurrencies(breaks))
			{
				built.AddRange(parseTwoCurrencies(breaks));
			}

			else if (containsThreeCurrencies(breaks))
			{
				built.AddRange(parseThreeCurrencies(breaks));
			}
			return built.ToArray();
		}

		private static bool containsOneCurrency(HtmlNodeCollection breaks)
		{
			return breaks == null || breaks.Count == 0;
		}

		private static ScrappedCurrency parseOneCurrecy(HtmlNode currencyNode)
		{
			string code = currencyNode.SelectSingleNode("td[3]").InnerText;
			string numericCode = currencyNode.SelectSingleNode("td[4]").InnerText;

			return ScrappedCurrency.IsCode(code) && ScrappedCurrency.IsNumericCode(numericCode) ?
				new ScrappedCurrency(code, numericCode, currencyNode.SelectSingleNode("td[2]").InnerText) :
				null;
		}

		private static bool containsTwoCurrencies(HtmlNodeCollection breaks)
		{
			return breaks.Count == 6;
		}

		private static IEnumerable<ScrappedCurrency> parseTwoCurrencies(HtmlNodeCollection breaks)
		{
			return new[]
			{
				new ScrappedCurrency(
					breaks[2].PreviousSibling.InnerText,
					breaks[4].PreviousSibling.InnerText,
					breaks[0].PreviousSibling.InnerText),
				new ScrappedCurrency(
					breaks[3].NextSibling.InnerText,
					breaks[5].NextSibling.InnerText,
					breaks[1].NextSibling.InnerText)
			};
		}

		private static bool containsThreeCurrencies(HtmlNodeCollection breaks)
		{
			return breaks.Count == 12;
		}

		private static IEnumerable<ScrappedCurrency> parseThreeCurrencies(HtmlNodeCollection breaks)
		{
			return new[]
			{
			    new ScrappedCurrency(
					breaks[4].PreviousSibling.InnerText,
			       	breaks[8].PreviousSibling.InnerText,
			       	breaks[0].PreviousSibling.InnerText),
			    new ScrappedCurrency(
			       	breaks[5].NextSibling.InnerText,
			       	breaks[10].PreviousSibling.InnerText,
			       	breaks[2].PreviousSibling.InnerText),
			    new ScrappedCurrency(
			       	breaks[7].NextSibling.InnerText,
			       	breaks[11].NextSibling.InnerText,
			       	breaks[3].NextSibling.InnerText)
			};
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
		public ScrappedCurrency(string code, string numericCode, string name) : this(
			stripSpecialCharacters(code),
			short.Parse(stripSpecialCharacters(numericCode)), 
			stripSpecialCharacters(name)) { }

		private ScrappedCurrency(string code, short numericCode, string name)
		{
			Code = code;
			NumericCode = numericCode;
			Name = name;
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

		internal  static ScrappedCurrency Create(Currency currency)
		{
			return new ScrappedCurrency(currency.IsoSymbol, currency.NumericCode, currency.EnglishName);
		}

		public Currency ToCurrency()
		{
			Currency obtained;
			Currency.TryGet(Code, out obtained);
			return obtained;
		}

		public override string ToString()
		{
			return Code + " " + NumericCode + " " + Name;
		}

		public static string ToString(Currency currency)
		{
			return currency.IsoSymbol + " " + currency.NumericCode + " " + currency.EnglishName;
		}
	}
}
