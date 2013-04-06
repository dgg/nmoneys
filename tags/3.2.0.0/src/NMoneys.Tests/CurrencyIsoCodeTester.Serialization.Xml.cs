using System;
using System.Runtime.Serialization;
using NMoneys.Support;
using NMoneys.Tests.CustomConstraints;
using NMoneys.Tests.Support;
using NUnit.Framework;

namespace NMoneys.Tests
{
	[TestFixture]
	public partial class CurrencyIsoCodeTester
	{
		[Test]
		public void CanBe_XmlSerialized()
		{
			Assert.That(CurrencyIsoCode.USD, Must.Be.XmlSerializable<CurrencyIsoCode>());
		}

		[Test]
		public void CanBe_XmlDeserializable()
		{
			string serializedDollar =
				"<isoCode xmlns=\"urn:nmoneys\">" +
				"USD" +
				"</isoCode>";
			Assert.That(serializedDollar, Must.Be.XmlDeserializableInto(CurrencyIsoCode.USD));
		}

		[Test]
		public void XmlSerialization_OfObsoleteCurrency_Exception()
		{
			using (var serializer = new OneGoXmlSerializer<CurrencyIsoCode>())
			{
				var obsolete = CurrencyIsoCode.EEK;
				Assert.That(() => serializer.Serialize(obsolete), Throws.InstanceOf<InvalidOperationException>());
			}
		}

		[Test]
		public void XmlDeserialization_DoesNotPreserveInstanceUniqueness()
		{
			using (var serializer = new OneGoXmlSerializer<CurrencyIsoCode>())
			{
				CurrencyIsoCode usd = CurrencyIsoCode.USD;
				serializer.Serialize(usd);
				Assert.That(serializer.Deserialize(), Is.Not.SameAs(usd)
					.And.EqualTo(usd));
			}
		}

		[Test]
		public void AllCurrencyCodes_DecoratedWithEnumMember()
		{
			Assert.That(Enum.GetValues(typeof(CurrencyIsoCode)), Has.All.Matches<CurrencyIsoCode>(
					Enumeration.HasAttribute<CurrencyIsoCode, EnumMemberAttribute>));
		}

		[Test]
		public void CanBe_DataContractSerialized()
		{
			Assert.That(CurrencyIsoCode.USD, Must.Be.DataContractSerializable<CurrencyIsoCode>());
		}

		[Test]
		public void CanBe_DataContractDeserializable()
		{
			string serializedEuro =
				"<isoCode xmlns=\"urn:nmoneys\">" +
				"EUR" +
				"</isoCode>";
			Assert.That(serializedEuro, Must.Be.DataContractDeserializableInto(CurrencyIsoCode.EUR));
		}

		[Test]
		public void DataContractDeserialization_OfObsoleteCurrency_DoesNotRaiseEvent()
		{
			using (var serializer = new OneGoDataContractSerializer<CurrencyIsoCode>())
			{
				var obsolete = CurrencyIsoCode.EEK;
				serializer.Serialize(obsolete);
				Action deserializeObsolete = () => serializer.Deserialize();
				Assert.That(deserializeObsolete, Must.Not.Raise.ObsoleteEvent());
			}
		}

		[Test]
		public void DataContractDeserialization_DoesNotPreserveInstanceUniqueness()
		{
			using (var serializer = new OneGoDataContractSerializer<CurrencyIsoCode>())
			{
				CurrencyIsoCode usd = CurrencyIsoCode.USD;
				serializer.Serialize(usd);
				Assert.That(serializer.Deserialize(), Is.Not.SameAs(usd)
					.And.EqualTo(usd));
			}
		}
	}
}
