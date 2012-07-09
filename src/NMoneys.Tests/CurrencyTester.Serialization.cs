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
		public void CannotBe_DataContractJsonSerialized()
		{
			Assert.That(Currency.Dollar, Must.Not.Be.DataContractJsonSerializable<Currency>());
		}

		[Test]
		public void CanBe_JsonSerialized()
		{
			Assert.That(Currency.Dollar, Must.Be.JsonSerializable<Currency>(Is.SameAs));
		}

		[Test]
		public void CanBe_JsonDeserializable()
		{
			string serializedDollar = "{\"isoCode\":\"USD\"}";
			Assert.That(serializedDollar, Must.Be.JsonDeserializableInto(Currency.Dollar));
		}

		[Test]
		public void JsonDeserialization_OfObsoleteCurrency_RaisesEvent()
		{
			using (var serializer = new OneGoJsonSerializer<Currency>())
			{
				var obsolete = Currency.Get("EEK");
				serializer.Serialize(obsolete);
				Action deserializeObsolete = () => serializer.Deserialize();
				Assert.That(deserializeObsolete, Must.RaiseObsoleteEvent.Once());
			}
		}

		[Test]
		public void JsonDeserialization_DoesPreserveInstanceUniqueness()
		{
			using (var serializer = new OneGoJsonSerializer<Currency>())
			{
				Currency usd = Currency.Get("USD");
				serializer.Serialize(usd);
				Assert.That(serializer.Deserialize(), Is.SameAs(usd));
			}
		}

		#region Issue 16. Case sensitivity. Currency instances can be obtained by any casing of the IsoCode (Alphbetic code)

		[Test]
		public void JsonDeserialization_IsCaseInsensitive()
		{
			string serializedDollar = "{\"isoCode\":\"uSd\"}";
			Assert.That(serializedDollar, Must.Be.JsonDeserializableInto(Currency.Dollar));
		}

		#endregion
	}
}