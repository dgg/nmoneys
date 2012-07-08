using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using NMoneys.Extensions;
using NMoneys.Support;
using NMoneys.Tests.CustomConstraints;
using NMoneys.Tests.Support;
using NUnit.Framework;

namespace NMoneys.Tests
{
	[TestFixture]
	public partial class MoneyTester
	{
		private static readonly Money fiver = new Money(5, CurrencyIsoCode.GBP),
			tenner = new Money(10, CurrencyIsoCode.GBP),
			hund = new Money(100, CurrencyIsoCode.DKK),
			nought = new Money(0, CurrencyIsoCode.GBP),
			oweMeQuid = new Money(-1, Currency.Gbp);

		private static readonly decimal twoThirds = 2m / 3;

		#region Ctor

		[Test]
		public void Ctor_Default_CurrencyIsNotDefault_ButUndefined()
		{
			Money @default = new Money();

			Assert.That(@default.CurrencyCode, Is.Not.EqualTo(default(CurrencyIsoCode)).And.EqualTo(CurrencyIsoCode.XXX));
		}

		[Test]
		public void Ctor_Amount_AmountWithNoneCurrency()
		{
			Money defaultMoney = new Money(3m);
			Assert.That(defaultMoney.Amount, Is.EqualTo(3m));
			Assert.That(defaultMoney.CurrencyCode, Is.EqualTo(CurrencyIsoCode.XXX));
		}

		[Test]
		public void Ctor_ExistingIsoCode_PropertiesSet()
		{
			Money tenDollars = new Money(10, CurrencyIsoCode.USD);
			Assert.That(tenDollars.Amount, Is.EqualTo(10m));
			Assert.That(tenDollars.CurrencyCode, Is.EqualTo(CurrencyIsoCode.USD));
		}

		[Test]
		public void Ctor_ObsoleteIsoCode_EventRaised()
		{
			CurrencyIsoCode obsolete = Enumeration.Parse<CurrencyIsoCode>("EEK");
			Action moneyWithObsoleteCurrency = () => new Money(10, obsolete);
			Assert.That(moneyWithObsoleteCurrency, Must.RaiseObsoleteEvent.Once());
		}

		[Test]
		public void Ctor_ExistingIsoSymbol_PropertiesSet()
		{
			Money hundredLerus = new Money(100, "EUR");
			Assert.That(hundredLerus.Amount, Is.EqualTo(100m));
			Assert.That(hundredLerus.CurrencyCode, Is.EqualTo(CurrencyIsoCode.EUR));
		}

		[Test]
		public void Ctor_ObsoleteIsoSymbol_EventRaised()
		{
			Action moneyWithObsoleteCurrency = () => new Money(10, "EEK");
			Assert.That(moneyWithObsoleteCurrency, Must.RaiseObsoleteEvent.Once());
		}

		[Test]
		public void Ctor_NullSymbol_Exception()
		{
			Assert.That(() => new Money(decimal.Zero, (string)null), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void Ctor_ObsoleteCurrency_EventRaised()
		{
			Currency obsolete = Currency.Get("EEK");
			Action moneyWithObsoleteCurrency = () => new Money(10, obsolete);
			Assert.That(moneyWithObsoleteCurrency, Must.RaiseObsoleteEvent.Once());
		}

		[Test]
		public void Ctor_Currency_PropertiesSet()
		{
			Assert.That(tenner.Amount, Is.EqualTo(10m));
			Assert.That(tenner.CurrencyCode, Is.EqualTo(CurrencyIsoCode.GBP));
		}

		[Test]
		public void Ctor_NullCurrency_Exception()
		{
			Assert.That(() => new Money(decimal.Zero, (Currency)null), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void Ctor_NonExistingIsoCode_Exception()
		{
			CurrencyIsoCode nonExistingCode = (CurrencyIsoCode)(-7);

			Assert.That(() => new Money(decimal.Zero, nonExistingCode), Throws.InstanceOf<InvalidEnumArgumentException>().With.Message.StringContaining("-7"));
		}

		[Test]
		public void Ctor_NonExistingIsoSymbol_PropertiesSet()
		{
			string nonExistentIsoSymbol = "XYZ";
			Assert.That(() => new Money(decimal.Zero, nonExistentIsoSymbol), Throws.InstanceOf<InvalidEnumArgumentException>());
		}

		#endregion

		#region ForCulture

		[Test]
		public void ForCulture_DefinedCulture_PropertiesSet()
		{
			Money twentyBucks = Money.ForCulture(20, CultureInfo.GetCultureInfo("en-US"));
			Assert.That(twentyBucks.Amount, Is.EqualTo(20m));
			Assert.That(twentyBucks.CurrencyCode, Is.EqualTo(CurrencyIsoCode.USD));
		}

		[Test, Platform(Include = "Net-2.0")]
		public void ForCulture_OutdatedCulture_Exception()
		{
			Assert.That(() => Money.ForCulture(decimal.Zero, CultureInfo.GetCultureInfo("bg-BG")),
				Throws.InstanceOf<InvalidEnumArgumentException>().With.Message.StringContaining("BGL"),
				"Framework returns wrong ISOCurrencySymbol (BGL instead of BGN)");
		}

		[Test, Platform(Include = "Net-2.0")]
		public void ForCulture_CultureWithObsoleteCulture_EventRaised()
		{
			Action moneyWithObsoleteCurrency = () => Money.ForCulture(decimal.Zero, CultureInfo.GetCultureInfo("et-EE"));
			Assert.That(moneyWithObsoleteCurrency, Must.RaiseObsoleteEvent.Once());
		}

		[Test]
		public void ForCulture_NullDefaultCulture_Exception()
		{
			Assert.Throws<ArgumentNullException>(() => Money.ForCulture(decimal.Zero, null));
		}

		#endregion

		#region ForCurrentCulture

		[Test, SetCulture("da-DK")]
		public void ForCurrentCulture_DefinedCulture_PropertiesSet()
		{
			Money subject = Money.ForCurrentCulture(100);
			Assert.That(subject.Amount, Is.EqualTo(100m));
			Assert.That(subject.CurrencyCode, Is.EqualTo(CurrencyIsoCode.DKK));
		}

		[Test, Platform(Include = "Net-2.0"), SetCulture("bg-BG")]
		public void ForCurrentCulture_OutdatedCulture_Exception()
		{
			Assert.That(() => Money.ForCurrentCulture(decimal.Zero),
				Throws.InstanceOf<InvalidEnumArgumentException>().With.Message.StringContaining("BGL"),
				"Framework returns wrong ISOCurrencySymbol (BGL instead of BGN)");
		}

		[Test, Platform(Include = "Net-2.0"), SetCulture("et-EE")]
		public void ForCurrentCulture_CultureWithObsoleteCulture_EventRaised()
		{
			Action moneyWithObsoleteCurrency = () => Money.ForCurrentCulture(decimal.Zero);
			Assert.That(moneyWithObsoleteCurrency, Must.RaiseObsoleteEvent.Once());
		}

		#endregion

		#region Major/Minor

		[Test]
		public void ForMajor_Currency()
		{
			Assert.That(Currency.Gbp.SignificantDecimalDigits, Is.EqualTo(2));
			Assert.That(Money.ForMajor(234, Currency.Gbp), Must.Be.MoneyWith(234, Currency.Gbp));
		}

		[TestCaseSource("forMinorAllowingDecimals")]
		public void ForMinor_Currency_AllowingDecimals_AmountShiftedAsManyDigitsAsCurrencySpecifies(Currency currency, int decimalDigits, long minorAmount, decimal amount)
		{
			Assert.That(currency.SignificantDecimalDigits, Is.EqualTo(decimalDigits));
			Assert.That(Money.ForMinor(minorAmount, currency), Must.Be.MoneyWith(amount, currency));
		}

#pragma warning disable 169
		private static TestCaseData[] forMinorAllowingDecimals = new[]
		{
			new TestCaseData(Currency.Gbp, 2, 234, 2.34m).SetName("bigger than cent factor"),
			new TestCaseData(Currency.Gbp, 2, 34, .34m).SetName("same as cent factor"),
			new TestCaseData(Currency.Gbp, 2, 4, .04m).SetName("less than cent factor"),
			new TestCaseData(Currency.Get(CurrencyIsoCode.BHD), 3, 2345, 2.345m).SetName("bigger than cent factor"),
			new TestCaseData(Currency.Get(CurrencyIsoCode.BHD), 3, 234, .234m).SetName("same as cent factor"),
			new TestCaseData(Currency.Get(CurrencyIsoCode.BHD), 3, 34, .034m).SetName("less than cent factor"),
		};
#pragma warning restore 169

		[Test]
		public void ForMinor_Currency_NotAllowingDecimals_AmountStaysAtItis()
		{
			Currency notAllowingDecimals = Currency.Jpy;
			Assert.That(notAllowingDecimals.SignificantDecimalDigits, Is.EqualTo(0));
			Assert.That(Money.ForMinor(234, notAllowingDecimals), Must.Be.MoneyWith(234m, notAllowingDecimals));
		}

		[Test]
		public void MajorAmount_NotDecimalAmount_SameValueForBoth()
		{
			Assert.That(tenner.MajorAmount, Is.EqualTo(10m));
			Assert.That(oweMeQuid.MajorAmount, Is.EqualTo(-1m));
		}

		[Test]
		public void MajorAmount_DecimalAmount_Truncated()
		{
			Assert.That(2.5m.Eur().MajorAmount, Is.EqualTo(2m));
			Assert.That(-2.5m.Eur().MajorAmount, Is.EqualTo(-2m));
		}

		[Test]
		public void MajorIntegralAmount_NotDecimalAmount_SameValueForBoth()
		{
			Assert.That(tenner.MajorIntegralAmount, Is.EqualTo(10L));
			Assert.That(oweMeQuid.MajorIntegralAmount, Is.EqualTo(-1L));
		}

		[Test]
		public void MajorIntegralAmount_DecimalAmount_Truncated()
		{
			Assert.That(2.5m.Eur().MajorIntegralAmount, Is.EqualTo(2L));
			Assert.That(-2.5m.Eur().MajorIntegralAmount, Is.EqualTo(-2L));
		}

		[Test]
		public void MinorAmount_CurrencyWithDecimals_DecimalShifted()
		{
			Assert.That(new Money(2.34m, Currency.Gbp).MinorAmount, Is.EqualTo(234m));
			Assert.That(new Money(-2.34m, Currency.Gbp).MinorAmount, Is.EqualTo(-234m));
			Assert.That(new Money(-2.3m, Currency.Eur).MinorAmount, Is.EqualTo(-230m));
			Assert.That(new Money(12.378m, Currency.Eur).MinorAmount, Is.EqualTo(1237m));
			Assert.That(new Money(5m, Currency.Eur).MinorAmount, Is.EqualTo(500m));
		}

		[Test]
		public void MinorAmount_CurrencyWithoutDecimals_DecimalNotShifted()
		{
			Assert.That(new Money(2.34m, Currency.Jpy).MinorAmount, Is.EqualTo(2m));
			Assert.That(new Money(-2.34m, Currency.Jpy).MinorAmount, Is.EqualTo(-2m));
		}

		[Test]
		public void MinorIntegralAmount_CurrencyWithDecimals_DecimalShifted()
		{
			Assert.That(new Money(2.34m, Currency.Gbp).MinorIntegralAmount, Is.EqualTo(234L));
			Assert.That(new Money(-2.34m, Currency.Gbp).MinorIntegralAmount, Is.EqualTo(-234L));
			Assert.That(new Money(-2.3m, Currency.Eur).MinorIntegralAmount, Is.EqualTo(-230m));
			Assert.That(new Money(12.378m, Currency.Eur).MinorIntegralAmount, Is.EqualTo(1237L));
			Assert.That(new Money(5m, Currency.Eur).MinorIntegralAmount, Is.EqualTo(500L));
		}

		[Test]
		public void MinorIntegralAmount_CurrencyWithoutDecimals_DecimalNotShifted()
		{
			Assert.That(new Money(2.34m, Currency.Jpy).MinorIntegralAmount, Is.EqualTo(2L));
			Assert.That(new Money(-2.34m, Currency.Jpy).MinorIntegralAmount, Is.EqualTo(-2L));
		}

		#endregion

		#region property value checking

		[Test]
		public void IsZero_TrueWhenZero()
		{
			Assert.That(fiver.IsZero(), Is.False);
			Assert.That(nought.IsZero(), Is.True);
			Assert.That(oweMeQuid.IsZero(), Is.False);
		}

		[Test]
		public void IsPositive_TrueWhenPositive()
		{
			Assert.That(fiver.IsPositive(), Is.True);
			Assert.That(nought.IsPositive(), Is.False);
			Assert.That(oweMeQuid.IsPositive(), Is.False);
		}

		[Test]
		public void IsPositiveOrZero_TruenWhenPositiveOrZero()
		{
			Assert.That(fiver.IsPositiveOrZero(), Is.True);
			Assert.That(nought.IsPositiveOrZero(), Is.True);
			Assert.That(oweMeQuid.IsPositiveOrZero(), Is.False);
		}

		[Test]
		public void IsNegative_TrueWhenNegative()
		{
			Assert.That(fiver.IsNegative(), Is.False);
			Assert.That(nought.IsNegative(), Is.False);
			Assert.That(oweMeQuid.IsNegative(), Is.True);
		}

		[Test]
		public void IsNegativeOrZero_TrueWhenNegativeOrZero()
		{
			Assert.That(fiver.IsNegativeOrZero(), Is.False);
			Assert.That(nought.IsNegativeOrZero(), Is.True);
			Assert.That(oweMeQuid.IsNegativeOrZero(), Is.True);
		}

		[TestCaseSource("sameCurrency")]
		public void HasSameCurrency_SameCurrency_True(Money subject, Money argument)
		{
			Assert.That(subject.HasSameCurrencyAs(argument), Is.True);
		}

		[TestCaseSource("differentCurrency")]
		public void HasSameCurrency_DifferentCurrency_False(Money subject, Money argument)
		{
			Assert.That(subject.HasSameCurrencyAs(argument), Is.False);
		}

		[TestCaseSource("sameCurrency")]
		public void AssertSameCurrency_SameCurrency_NoException(Money subject, Money argument)
		{
			Assert.That(() => subject.AssertSameCurrency(argument), Throws.Nothing);
		}

		[TestCaseSource("differentCurrency")]
		public void AssertSameCurrency_DifferentCurrency_Exception(Money subject, Money argument)
		{
			Assert.That(() => subject.AssertSameCurrency(argument), Throws.InstanceOf<DifferentCurrencyException>());
		}

#pragma warning disable 169
		private static readonly object[] sameCurrency = new object[]
		{
			new object[] {fiver, fiver},
			new object[] {fiver, tenner},			
			new object[] {tenner, fiver},
		};

		private static readonly object[] differentCurrency = new object[]
		{
			new object[] {fiver, hund },
			new object[] {hund, fiver },
		};
#pragma warning restore 169

		#endregion

		#region serialization

		[Test]
		public void CanBe_BinarySerialized()
		{
			Assert.That(new Money(3.757m), Must.Be.BinarySerializable<Money>(m => Is.EqualTo(m)));
		}

		[Test]
		public void BinarySerialization_ObsoleteCurrency_RaisesEvent()
		{
			Money obsolete = new Money(2m, "EEK");
			using (var serializer = new OneGoBinarySerializer<Money>())
			{
				serializer.Serialize(obsolete);
				Action deserializeObsolete = () => serializer.Deserialize();
				Assert.That(deserializeObsolete, Must.RaiseObsoleteEvent.Once());
			}
		}

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

		[Test]
		public void CannotBe_DataContractJsonSerialized()
		{
			Assert.That(new Money(3.757m), Must.Not.Be.DataContractJsonSerializable<Money>());
		}

		[Test]
		public void CanBe_JsonSerialized()
		{
			Assert.That(new Money(3.757m), Must.Be.JsonSerializable<Money>(m => Is.EqualTo(m)));
		}

		[Test]
		public void CanBe_JsonDeserializable()
		{
			string serializedMoney = "{\"amount\":3.757,\"currency\":{\"isoCode\":\"XXX\"}}";
			Assert.That(serializedMoney, Must.Be.JsonDeserializableInto(new Money(3.757m)));
		}

		[Test]
		public void JsonSerialization_ObsoleteCurrency_RaisesEvent()
		{
			Money obsolete = new Money(2m, "EEK");
			using (var serializer = new OneGoJsonSerializer<Money>())
			{
				serializer.Serialize(obsolete);
				Action deserializeObsolete = () => serializer.Deserialize();
				Assert.That(deserializeObsolete, Must.RaiseObsoleteEvent.Once());
			}
		}

		#endregion

		#region non Zero-Based enumeration behaviors

		[Test]
		public void CurrencyDependentMethod_CanBeUsedWithDefaultInstance()
		{
			Money @default = new Money();

			Assert.That(() => @default.ToString(), Throws.Nothing);
			Assert.That(@default.CurrencyCode, Is.EqualTo(CurrencyIsoCode.XXX));
		}

		[Test]
		public void CannotBeCreated_WithDefaultCode_AsItIsUndefined()
		{
			Assert.That(() => new Money(2, default(CurrencyIsoCode)), Throws.InstanceOf<InvalidEnumArgumentException>());
		}

		[Test]
		public void DefaultInstances_HaveDefinedCode_InsteadOfUndefined()
		{
			Money? @null = null;

			Assert.That(@null.GetValueOrDefault().CurrencyCode, Is.EqualTo(CurrencyIsoCode.XXX));
		}

		[Test]
		public void Moneycontainer_CanHaveUnitializedMembers()
		{
			var p = new MoneyContainer();
			var someMoney = new Money(0m, Currency.Dkk);

			Assert.That(p.Money, Is.Not.EqualTo(someMoney));
		}

		public class MoneyContainer
		{
			public Money Money { get; set; }
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

		[Test]
		public void XmlSerialization_OfDefaultInstance_StoresAndDeserializesNoCurrency()
		{
			var @default = new Money();

			var serializer = new OneGoXmlSerializer<Money>();
			serializer.Serialize(@default);

			Money deserialized = serializer.Deserialize();

			Assert.That(deserialized, Must.Be.MoneyWith(0m, Currency.Xxx));
		}

		[Test]
		public void DatacontractSerialization_OfDefaultInstance_StoresAndDeserializesNoCurrency()
		{
			var @default = new Money();

			var serializer = new OneGoDataContractSerializer<Money>();
			serializer.Serialize(@default);

			Money deserialized = serializer.Deserialize();

			Assert.That(deserialized, Must.Be.MoneyWith(0m, Currency.Xxx));
		}

		[Test]
		public void JsonSerialization_OfDefaultInstance_StoresAndDeserializesNoCurrency()
		{
			var @default = new Money();

			var serializer = new OneGoJsonSerializer<Money>();
			serializer.Serialize(@default);

			Money deserialized = serializer.Deserialize();

			Assert.That(deserialized, Must.Be.MoneyWith(0m, Currency.Xxx));
		}

		#endregion

		[Test, Explicit("WARNING: THIS TEST MIGH FAIL. Rerun the test to solve it.")]
		public void ExploratoryTesting_OnPerformance()
		{
			Stopwatch watch = new Stopwatch();
			ActionTimer.Time(watch, () =>
			{
				var c = new WithoutEnsure().NonZero;
			},
			1000000);
			long withoutEnsure = watch.ElapsedTicks;
			Debug.WriteLine("withoutEnsure: " + withoutEnsure);

			watch.Reset();
			ActionTimer.Time(watch, () =>
			{
				var c = new WithEnsure().NonZero;
			},
			1000000);
			long withEnsure = watch.ElapsedTicks;
			Debug.WriteLine("withEnsure: " + withEnsure);

			watch.Reset();
			ActionTimer.Time(watch, () =>
			{
				var c = new WithNullable().NonZero;
			}, 1000000);
			long withNullable = watch.ElapsedTicks;
			Debug.WriteLine("withNullable: " + withNullable);

			Assert.That(withoutEnsure, Is.LessThan(withEnsure).And.LessThan(withNullable),
				"doing nothing is the fastest, but it does allow a undefined value in the enumeration.");
			Assert.That(withEnsure, Is.GreaterThan(withNullable),
				"having a custom method to ensure the defined value is more expensive than using a nullable private field");
		}

		private enum NonZero
		{
			One = 1,
			Two = 2
		}

		private struct WithoutEnsure
		{
			public WithoutEnsure(decimal number, NonZero nonZero)
				: this()
			{
				Number = number;
				NonZero = nonZero;
			}

			public NonZero NonZero { get; private set; }

			public decimal Number { get; private set; }
		}

		private struct WithEnsure
		{
			public WithEnsure(decimal number, NonZero nonZero)
				: this()
			{
				Number = number;
				NonZero = nonZero;
			}

			private NonZero _nonZero;
			public NonZero NonZero
			{
				get
				{
					ensureNoDefault();
					return _nonZero;
				}
				private set { _nonZero = value; }
			}

			private void ensureNoDefault()
			{
				if (_nonZero.Equals(default(NonZero)))
				{
					_nonZero = NonZero.One;
				}
			}

			private decimal Number { get; set; }
		}

		private struct WithNullable
		{
			public WithNullable(decimal number, NonZero nonZero)
				: this()
			{
				Number = number;
				NonZero = nonZero;
			}

			private NonZero? _nonZero;
			public NonZero NonZero { get { return _nonZero.GetValueOrDefault(NonZero.One); } private set { _nonZero = value; } }

			public decimal Number { get; private set; }

			public decimal Amount { get; private set; }
		}

		#region Issue 16. Case sensitivity. Money instances can be obtained by any casing of the IsoCode (Alphbetic code)

		[Test]
		public void ctor_IsoCode_IsCaseInsensitive()
		{
			Money upper = new Money(100, "EUR");
			Money mixed = new Money(100, "eUr");

			Assert.That(upper, Is.EqualTo(mixed));
		}

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

		[Test]
		public void JsonDeserialization_IsCaseInsensitive()
		{
			string serializedMoney = "{\"amount\":3.757,\"currency\":{\"isoCode\":\"xXx\"}}";
			Assert.That(serializedMoney, Must.Be.JsonDeserializableInto(new Money(3.757m)));
		}

		#endregion

		[Test]
		public void OperatingOnStrings_IsPossible_IfCurrenciesAreKnownUpfront()
		{
			Money poundQuantity = Money.Parse("£100.50", Currency.Gbp);
			Money yenQuantity = Money.Parse("¥ 1,000", Currency.Jpy);

			Func<decimal, decimal> halfDiscount = q => q * .5m;

			Assert.That(poundQuantity.Perform(halfDiscount), Must.Be.MoneyWith(50.25m, Currency.Gbp));
			Assert.That(yenQuantity.Perform(halfDiscount), Must.Be.MoneyWith(500, Currency.Jpy));
		}
	}
}
