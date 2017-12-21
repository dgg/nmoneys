using System;
using System.IO;
using System.Net;
using System.Xml.Serialization;

namespace NMoneys.Tools.CompareIso
{
	internal class IsoCurrenciesLoader
	{
		internal static readonly Uri IsoUrl = new Uri("https://www.currency-iso.org/dam/downloads/lists/list_one.xml");

		public IsoCurrenciesCollection LoadFrom(Uri url)
		{
			using (var client = new WebClient())
			{
				IsoCurrenciesCollection collection = load(client.OpenRead(url));
				return collection;
			}
		}

		private IsoCurrenciesCollection load(Stream stream)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(Iso4217Element));
			var doc = (Iso4217Element)serializer.Deserialize(stream);
			var currencies = new IsoCurrenciesCollection();
			currencies.AddRange(doc.CountryTable.Countries);
			return currencies;
		}

		public IsoCurrenciesCollection LoadFrom(FileStream file)
		{
			IsoCurrenciesCollection collection = load(file);
			return collection;
		}
	}
}