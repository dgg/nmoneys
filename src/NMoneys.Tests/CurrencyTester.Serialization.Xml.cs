using System;
using NMoneys.Tests.CustomConstraints;
using NMoneys.Tests.Support;
using NUnit.Framework;

namespace NMoneys.Tests
{
	[TestFixture]
	public partial class CurrencyTester
	{
		[Test]
		public void CanBe_XmlSerialized()
		{
			Assert.That(Currency.Dollar, Must.Be.XmlSerializable<Currency>());
		}

		[Test]
		public void CanBe_XmlDeserializable()
		{
			string serializedDollar =
				"<currency xmlns=\"urn:nmoneys\">" +
				"<isoCode>USD</isoCode>" +
				"</currency>";
			Assert.That(serializedDollar, Must.Be.XmlDeserializableInto(Currency.Dollar));
		}

		[Test, TestCaseSource(typeof(Obsolete), "ThreeLetterIsoCodes")]
		public void XmlDeserialization_OfObsoleteCurrency_RaisesEvent(string threeLetterIsoCode)
		{
			using (var serializer = new OneGoXmlSerializer<Currency>())
			{
				var obsolete = Currency.Get(threeLetterIsoCode);
				serializer.Serialize(obsolete);
				Action deserializeObsolete = () => serializer.Deserialize();
				Assert.That(deserializeObsolete, Must.RaiseObsoleteEvent.Once());
			}
		}

		[Test]
		public void XmlDeserialization_DoesNotPreserveInstanceUniqueness()
		{
			using (var serializer = new OneGoXmlSerializer<Currency>())
			{
				Currency usd = Currency.Get("USD");
				serializer.Serialize(usd);
				Assert.That(serializer.Deserialize(), Is.Not.SameAs(usd)
					.And.EqualTo(usd));
			}
		}

		[Test]
		public void CanBe_DataContractSerialized()
		{
			Assert.That(Currency.Dollar, Must.Be.DataContractSerializable<Currency>());
		}

		[Test]
		public void CanBe_DataContractDeserializable()
		{
			string serializedDollar =
				"<currency xmlns=\"urn:nmoneys\">" +
				"<isoCode>USD</isoCode>" +
				"</currency>";
			Assert.That(serializedDollar, Must.Be.DataContractDeserializableInto(Currency.Dollar));
		}

		[Test, TestCaseSource(typeof(Obsolete), "ThreeLetterIsoCodes")]
		public void DataContractDeserialization_OfObsoleteCurrency_RaisesEvent(string threeLetterIsoCode)
		{
			using (var serializer = new OneGoDataContractSerializer<Currency>())
			{
				var obsolete = Currency.Get(threeLetterIsoCode);
				serializer.Serialize(obsolete);
				Action deserializeObsolete = () => serializer.Deserialize();
				Assert.That(deserializeObsolete, Must.RaiseObsoleteEvent.Once());
			}
		}

		[Test]
		public void DataContractDeserialization_DoesNotPreserveInstanceUniqueness()
		{
			using (var serializer = new OneGoDataContractSerializer<Currency>())
			{
				Currency usd = Currency.Get("USD");
				serializer.Serialize(usd);
				Assert.That(serializer.Deserialize(), Is.Not.SameAs(usd)
					.And.EqualTo(usd));
			}
		}

		#region Issue 16. Case sensitivity. Currency instances can be obtained by any casing of the IsoCode (Alphbetic code)

		[Test]
		public void XmlDeserialization_IsCaseInsentive()
		{
			string serializedDollar =
				"<currency xmlns=\"urn:nmoneys\">" +
				"<isoCode>uSd</isoCode>" +
				"</currency>";
			Assert.That(serializedDollar, Must.Be.XmlDeserializableInto(Currency.Dollar));
		}

		[Test]
		public void DataContractDeserialization_IsCaseInsensitive()
		{
			string serializedDollar =
				"<currency xmlns=\"urn:nmoneys\">" +
				"<isoCode>uSd</isoCode>" +
				"</currency>";
			Assert.That(serializedDollar, Must.Be.DataContractDeserializableInto(Currency.Dollar));
		}

		#endregion
	}
}