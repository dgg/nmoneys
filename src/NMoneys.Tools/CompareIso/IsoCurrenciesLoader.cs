using System;
using System.Net;
using System.Xml.Serialization;

namespace NMoneys.Tools.CompareIso
{
	internal class IsoCurrenciesLoader
	{
		public IsoCurrenciesCollection LoadFrom(Uri url)
		{
			using (var client = new WebClient())
			{
				XmlSerializer serializer = new XmlSerializer(typeof(Iso4217Element));
				var doc = (Iso4217Element)serializer.Deserialize(client.OpenRead(url));
				var currencies = new IsoCurrenciesCollection();
				currencies.AddRange(doc.CountryTable.Countries);
				return currencies;
			}
		}
	}
}