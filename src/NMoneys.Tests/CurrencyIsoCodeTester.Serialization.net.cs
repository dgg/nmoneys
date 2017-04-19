using System;
using NMoneys.Serialization;
using NMoneys.Tests.CustomConstraints;
using NMoneys.Tests.Support;
using NUnit.Framework;
using Testing.Commons;
using Testing.Commons.Serialization;

namespace NMoneys.Tests
{
	[TestFixture]
	public partial class CurrencyIsoCodeTester
	{
		[Test]
		public void CanBe_BinarySerialized()
		{
			Assert.That(CurrencyIsoCode.USD, Must.Be.BinarySerializable<CurrencyIsoCode>(Is.EqualTo(CurrencyIsoCode.USD)));
		}

		[Test, TestCaseSource(typeof(Obsolete), "IsoCodes")]
		public void BinaryDeserialization_OfObsoleteCurrency_DoesNotRaiseEvent(CurrencyIsoCode obsolete)
		{
			using (var serializer = new BinaryRoundtripSerializer<CurrencyIsoCode>())
			{
				serializer.Serialize(obsolete);
				Action deserializeObsolete = () => serializer.Deserialize();
				Assert.That(deserializeObsolete, Must.Not.Raise.ObsoleteEvent());
			}
		}

		[Test]
		public void BinaryDeserialization_DoesNotPreservesInstanceUniqueness()
		{
			using (var serializer = new BinaryRoundtripSerializer<CurrencyIsoCode>())
			{
				CurrencyIsoCode usd = CurrencyIsoCode.USD;
				serializer.Serialize(CurrencyIsoCode.USD);
				Assert.That(serializer.Deserialize(), Is.Not.SameAs(usd)
					.And.EqualTo(usd));
			}
		}

		[Test]
		public void CanBe_DataContractJsonSerialized()
		{
			Assert.That(CurrencyIsoCode.USD, Must.Be.DataContractJsonSerializable<CurrencyIsoCode>());
		}

		[Test]
		public void CanBe_DataContractJsonDeserializable()
		{
			// json serialization of enums cannot be customized :-(
			string serializedDollar = "840";
			Assert.That(serializedDollar, Must.Be.DataContractJsonDeserializableInto(CurrencyIsoCode.USD));
		}

		[Test, TestCaseSource(typeof(Obsolete), "IsoCodes")]
		public void DataContractJsonDeserialization_OfObsoleteCurrency_DoesNotRaiseEvent(CurrencyIsoCode obsolete)
		{
			using (var serializer = new DataContractJsonRoundtripSerializer<CurrencyIsoCode>(dataContractSurrogate: new DataContractSurrogate()))
			{
				serializer.Serialize(obsolete);
				Action deserializeObsolete = () => serializer.Deserialize();
				Assert.That(deserializeObsolete, Must.Not.Raise.ObsoleteEvent());
			}
		}
	}
}
