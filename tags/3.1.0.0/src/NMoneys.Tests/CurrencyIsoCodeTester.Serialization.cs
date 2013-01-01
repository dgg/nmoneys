using System;
using NMoneys.Tests.CustomConstraints;
using NMoneys.Tests.Support;
using NUnit.Framework;

namespace NMoneys.Tests
{
	[TestFixture]
	public partial class CurrencyIsoCodeTester
	{
		[Test]
		public void CanBe_BinarySerialized()
		{
			Assert.That(CurrencyIsoCode.USD, Must.Be.BinarySerializable<CurrencyIsoCode>(c => Is.EqualTo(c)));
		}

		[Test, TestCaseSource(typeof(Obsolete), "IsoCodes")]
		public void BinaryDeserialization_OfObsoleteCurrency_DoesNotRaiseEvent(CurrencyIsoCode obsolete)
		{
			using (var serializer = new OneGoBinarySerializer<CurrencyIsoCode>())
			{
				serializer.Serialize(obsolete);
				Action deserializeObsolete = () => serializer.Deserialize();
				Assert.That(deserializeObsolete, Must.Not.Raise.ObsoleteEvent());
			}
		}

		[Test]
		public void BinaryDeserialization_DoesNotPreservesInstanceUniqueness()
		{
			using (var serializer = new OneGoBinarySerializer<CurrencyIsoCode>())
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
			using (var serializer = new OneGoDataContractJsonSerializer<CurrencyIsoCode>())
			{
				serializer.Serialize(obsolete);
				Action deserializeObsolete = () => serializer.Deserialize();
				Assert.That(deserializeObsolete, Must.Not.Raise.ObsoleteEvent());
			}
		}
	}
}
