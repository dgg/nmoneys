using System;
using System.ComponentModel;
using System.Diagnostics;
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

		[Test]
		public void HasDecimals_Integer_False()
		{
			Assert.That(new Money(3m, Currency.Usd).HasDecimals, Is.False);
		}

		[Test]
		public void HasDecimals_NotInteger_False()
		{
			Assert.That(new Money(3.0000001m, Currency.Usd).HasDecimals, Is.True);
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

		#endregion

		[Test, Explicit("WARNING: THIS TEST MIGH FAIL. Rerun the test to solve it.")]
		public void ExploratoryTesting_OnPerformance()
		{
			var watch = new Stopwatch();
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
			var upper = new Money(100, "EUR");
			var mixed = new Money(100, "eUr");

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

		#endregion
	}
}
