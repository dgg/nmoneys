using System;
using NMoneys.Tests.CustomConstraints;
using NMoneys.Tests.Support;
using NUnit.Framework;

namespace NMoneys.Tests
{
	[TestFixture]
	public partial class MoneyTester
	{
		[Test]
		public void CanBe_BinarySerialized()
		{
			Assert.That(new Money(3.757m), Must.Be.BinarySerializable<Money>(m => Is.EqualTo(m)));
		}

		[Test]
		public void BinarySerialization_ObsoleteCurrency_RaisesEvent()
		{
			var obsolete = new Money(2m, "EEK");
			using (var serializer = new OneGoBinarySerializer<Money>())
			{
				serializer.Serialize(obsolete);
				Action deserializeObsolete = () => serializer.Deserialize();
				Assert.That(deserializeObsolete, Must.RaiseObsoleteEvent.Once());
			}
		}

		[Test]
		public void CanBe_DataContractJsonSerialized()
		{
			Assert.That(new Money(3.757m), Must.Be.DataContractJsonSerializable<Money>());
		}

		[Test]
		public void CanBe_DataContractJsonDeserializable()
		{
			string serializedMoney = "{\"amount\":3.757,\"currency\":{\"isoCode\":\"XXX\"}}";
			Assert.That(serializedMoney, Must.Be.DataContractJsonDeserializableInto(new Money(3.757m)));
		}

		[Test]
		public void DataContractJsonSerialization_ObsoleteCurrency_RaisesEvent()
		{
			var obsolete = new Money(2m, "EEK");
			using (var serializer = new OneGoDataContractJsonSerializer<Money>())
			{
				serializer.Serialize(obsolete);
				Action deserializeObsolete = () => serializer.Deserialize();
				Assert.That(deserializeObsolete, Must.RaiseObsoleteEvent.Once());
			}
		}

		[Test]
		public void DatacontractJsonSerialization_OfDefaultInstance_StoresAndDeserializesNoCurrency()
		{
			var @default = new Money();

			var serializer = new OneGoDataContractJsonSerializer<Money>();
			serializer.Serialize(@default);

			Money deserialized = serializer.Deserialize();

			Assert.That(deserialized, Must.Be.MoneyWith(0m, Currency.Xxx));
		}

		[Test]
		public void BinarySerialization_OfDefaultInstance_StoresAndDeserializesNoCurrency()
		{
			var @default = new Money();

			var serializer = new OneGoBinarySerializer<Money>();
			serializer.Serialize(@default);

			Money deserialized = serializer.Deserialize();

			Assert.That(deserialized, Must.Be.MoneyWith(0m, Currency.Xxx));
		}


	}
}