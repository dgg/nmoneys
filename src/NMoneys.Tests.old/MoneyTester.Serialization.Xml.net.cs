﻿using System;
using NMoneys.Tests.CustomConstraints;
using NMoneys.Tests.Support;
using NUnit.Framework;
using Testing.Commons;
using Testing.Commons.Serialization;

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

		[Test, TestCaseSource(typeof(Obsolete), "ThreeLetterIsoCodes")]
		public void XmlSerialization_ObsoleteCurrency_RaisesEvent(string threeLetterIsoCode)
		{
			var obsolete = new Money(2m, threeLetterIsoCode);
			using (var serializer = new XmlRoundtripSerializer<Money>())
			{
				serializer.Serialize(obsolete);
				Action deserializeObsolete = () => serializer.Deserialize();
				Assert.That(deserializeObsolete, Must.Raise.ObsoleteEvent());
			}
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

		[Test, TestCaseSource(typeof(Obsolete), "ThreeLetterIsoCodes")]
		public void DataContractSerialization_ObsoleteCurrency_RaisesEvent(string threeLetterIsoCode)
		{
			var obsolete = new Money(2m, threeLetterIsoCode);
			using (var serializer = new DataContractRoundtripSerializer<Money>())
			{
				serializer.Serialize(obsolete);
				Action deserializeObsolete = () => serializer.Deserialize();
				Assert.That(deserializeObsolete, Must.Raise.ObsoleteEvent());
			}
		}

		[Test]
		public void XmlSerialization_OfDefaultInstance_StoresAndDeserializesNoCurrency()
		{
			var @default = new Money();

			var serializer = new XmlRoundtripSerializer<Money>();
			serializer.Serialize(@default);

			Money deserialized = serializer.Deserialize();

			Assert.That(deserialized, Must.Be.MoneyWith(0m, Currency.Xxx));
		}

		[Test]
		public void DatacontractSerialization_OfDefaultInstance_StoresAndDeserializesNoCurrency()
		{
			var @default = new Money();

			var serializer = new DataContractRoundtripSerializer<Money>();
			serializer.Serialize(@default);

			Money deserialized = serializer.Deserialize();

			Assert.That(deserialized, Must.Be.MoneyWith(0m, Currency.Xxx));
		}

		#region Issue 16. Case sensitivity. Money instances can be obtained by any casing of the IsoCode (Alphbetic code)
	
		[Test]
		public void XmlDeserialization_IsCaseInsensitive()
		{
			string serializedMoney =
				"<money xmlns=\"urn:nmoneys\">" +
				"<amount>3.757</amount>" +
				"<currency><isoCode>xXx</isoCode></currency>" +
				"</money>";
			Assert.That(serializedMoney, Must.Be.XmlDeserializableInto(new Money(3.757m)));
		}

		[Test]
		public void DataContractDeserialization_IsCaseInsensitive()
		{
			string serializedMoney =
				"<money xmlns=\"urn:nmoneys\">" +
				"<amount>3.757</amount>" +
				"<currency><isoCode>xXx</isoCode></currency>" +
				"</money>";
			Assert.That(serializedMoney, Must.Be.DataContractDeserializableInto(new Money(3.757m)));
		}

		#endregion
	}
}