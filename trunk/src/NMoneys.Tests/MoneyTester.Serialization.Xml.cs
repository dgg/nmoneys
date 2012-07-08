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
		public void CanBe_XmlSerialized()
		{
			Assert.That(new Money(3.757m), Must.Be.XmlSerializable<Money>());
		}

		[Test]
		public void CanBe_XmlDeserializable()
		{
			string serializedMoney =
				"<money xmlns=\"urn:nmoneys\">" +
				"<amount>3.757</amount>" +
				"<currency><isoCode>XXX</isoCode></currency>" +
				"</money>";
			Assert.That(serializedMoney, Must.Be.XmlDeserializableInto(new Money(3.757m)));
		}

		[Test]
		public void XmlSerialization_ObsoleteCurrency_RaisesEvent()
		{
			Money obsolete = new Money(2m, "EEK");
			using (var serializer = new OneGoXmlSerializer<Money>())
			{
				serializer.Serialize(obsolete);
				Action deserializeObsolete = () => serializer.Deserialize();
				Assert.That(deserializeObsolete, Must.RaiseObsoleteEvent.Once());
			}
		}

		[Test]
		public void CanBe_XamlSerialized()
		{
			XamlSerializer serializer = new XamlSerializer();
			string xaml = serializer.Serialize(new Money(3.757m));
			Assert.DoesNotThrow(() => serializer.Deserialize<Money>(xaml));

			Assert.Inconclusive("Properties are not serialized, though.");
		}

		[Test]
		public void CanBe_DataContractSerialized()
		{
			Assert.That(new Money(3.757m), Must.Be.DataContractSerializable<Money>());
		}

		[Test]
		public void CanBe_DataContractDeserializable()
		{
			string serializedMoney =
				"<money xmlns=\"urn:nmoneys\">" +
				"<amount>3.757</amount>" +
				"<currency><isoCode>XXX</isoCode></currency>" +
				"</money>";
			Assert.That(serializedMoney, Must.Be.DataContractDeserializableInto(new Money(3.757m)));
		}

		[Test]
		public void DataContractSerialization_ObsoleteCurrency_RaisesEvent()
		{
			Money obsolete = new Money(2m, "EEK");
			using (var serializer = new OneGoDataContractSerializer<Money>())
			{
				serializer.Serialize(obsolete);
				Action deserializeObsolete = () => serializer.Deserialize();
				Assert.That(deserializeObsolete, Must.RaiseObsoleteEvent.Once());
			}
		}
	}
}