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
		public void CanBe_BinarySerialized()
		{
			Assert.That(Currency.Dollar, Must.Be.BinarySerializable<Currency>(Is.EqualTo));
		}

		[Test]
		public void BinaryDeserialization_OfObsoleteCurrency_RaisesEvent()
		{
			using (var serializer = new OneGoBinarySerializer<Currency>())
			{
				var obsolete = Currency.Get("EEK");
				serializer.Serialize(obsolete);
				Action deserializeObsolete = () => serializer.Deserialize();
				Assert.That(deserializeObsolete, Must.RaiseObsoleteEvent.Once());
			}
		}

		[Test]
		public void BinaryDeserialization_DoesNotPreservesInstanceUniqueness()
		{
			using (var serializer = new OneGoBinarySerializer<Currency>())
			{
				Currency usd = Currency.Get("USD");
				serializer.Serialize(usd);
				Assert.That(serializer.Deserialize(), Is.EqualTo(usd));
			}
		}

		[Test]
		public void CanBe_DataContractJsonSerialized()
		{
			Assert.That(Currency.Dollar, Must.Be.DataContractJsonSerializable<Currency>());
		}

		[Test]
		public void CanBe_DataContractJsonDeserializable()
		{
			string serializedDollar = "{\"isoCode\":\"USD\"}";
			Assert.That(serializedDollar, Must.Be.DataContractJsonDeserializableInto(Currency.Dollar));
		}

		[Test]
		public void DataContractJsonDeserialization_OfObsoleteCurrency_RaisesEvent()
		{
			using (var serializer = new OneGoDataContractJsonSerializer<Currency>())
			{
				var obsolete = Currency.Get("EEK");
				serializer.Serialize(obsolete);
				Action deserializeObsolete = () => serializer.Deserialize();
				Assert.That(deserializeObsolete, Must.RaiseObsoleteEvent.Once());
			}
		}
	}
}