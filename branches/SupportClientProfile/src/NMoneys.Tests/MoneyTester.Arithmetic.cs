﻿using System;
using System.Globalization;
using NMoneys.Tests.CustomConstraints;
using NUnit.Framework;

namespace NMoneys.Tests
{
	[TestFixture]
	public partial class MoneyTester
	{
		[Test]
		public void Negate_AnotherMoneyWithNegativeAmount()
		{
			Money oweYouFive = fiver.Negate();
			Assert.That(oweYouFive, Is.Not.SameAs(fiver));
			Assert.That(oweYouFive.Amount, Is.EqualTo(-(fiver.Amount)));
			Assert.That(oweYouFive.CurrencyCode, Is.EqualTo(fiver.CurrencyCode));

			Money youOweMeFive = oweYouFive.Negate();
			Assert.That(youOweMeFive.Amount, Is.EqualTo(fiver.Amount));
		}

		[Test]
		public void Plus_SameCurrency_AnotherMoneyWithAddedUpamount()
		{
			Money fifteenQuid = fiver.Plus(tenner);

			Assert.That(fifteenQuid, Is.Not.SameAs(fiver).And.Not.SameAs(tenner));
			Assert.That(fifteenQuid.Amount, Is.EqualTo(15));
			Assert.That(fifteenQuid.CurrencyCode, Is.EqualTo(CurrencyIsoCode.GBP));
		}

		[Test]
		public void Plus_DifferentCurrency_Exception()
		{
			Money money;
			Assert.That(() => money = fiver.Plus(hund), Throws.InstanceOf<DifferentCurrencyException>());
		}

		[Test]
		public void Minus_SameCurrency_AnotherMoneyWithAddedUpamount()
		{
			Money anotherFiver = tenner.Minus(fiver);

			Assert.That(anotherFiver, Is.Not.SameAs(fiver).And.Not.SameAs(tenner));
			Assert.That(anotherFiver.Amount, Is.EqualTo(5));
			Assert.That(anotherFiver.CurrencyCode, Is.EqualTo(CurrencyIsoCode.GBP));
		}

		[Test]
		public void Minus_TennerMinusFiver_OweMeMoney()
		{
			Money oweFiver = fiver.Minus(tenner);
			Assert.That(oweFiver.Amount, Is.EqualTo(-5));
		}

		[Test]
		public void Minus_DifferentCurrency_Exception()
		{
			Money money;
			Assert.That(() => money = fiver.Minus(hund), Throws.InstanceOf<DifferentCurrencyException>());
		}

		[Test]
		public void Abs_GetsPositiveAmount()
		{
			Assert.That(fiver.Abs().Amount, Is.EqualTo(fiver.Amount));

			Assert.That((fiver - tenner).Abs().Amount, Is.EqualTo(5));
		}

		#region TruncateToSignificantDecimalDigits

		[Test]
		public void TruncateToSignificantDecimalDigits_CurrencyWith2Decimals_AmountHas2Decimals()
		{
			var subject = new Money(twoThirds, CurrencyIsoCode.XXX);

			Assert.That(subject.Amount, Is.Not.EqualTo(0.66m), "has more decimals");
			Assert.That(subject.TruncateToSignificantDecimalDigits().Amount, Is.EqualTo(0.66m));
		}

		[Test]
		public void TruncateToSignificantDecimalDigits_CultureWithSameCurrency_CultureApplied()
		{
			var subject = new Money(twoThirds, CurrencyIsoCode.EUR);
			NumberFormatInfo spanishFormatting = CultureInfo.GetCultureInfo("es-ES").NumberFormat;

			Assert.That(subject.Amount, Is.Not.EqualTo(0.66m),
						"the raw amount has more decimals");
			Assert.That(subject.TruncateToSignificantDecimalDigits(spanishFormatting).Amount, Is.EqualTo(0.66m),
						"in Spain, 2 decimals are used for currencies");
		}

		[Test]
		public void TruncateToSignificantDecimalDigits_CultureWithDifferentCurrency_CultureApplied()
		{
			var subject = new Money(twoThirds, CurrencyIsoCode.GBP);
			NumberFormatInfo spanishFormatting = CultureInfo.GetCultureInfo("es-ES").NumberFormat;

			Assert.That(subject.Amount, Is.Not.EqualTo(0.66m),
						"the raw amount has more decimals");
			Assert.That(subject.TruncateToSignificantDecimalDigits(spanishFormatting).Amount, Is.EqualTo(0.66m),
						"in Spain, 2 decimals are used for currencies");
		}

		#endregion

		#region Truncate

		[TestCaseSource("truncate")]
		public void Truncate_Spec(decimal amount, decimal truncatedAmount)
		{
			var subject = new Money(amount, CurrencyIsoCode.XTS);

			Money truncated = subject.Truncate();
			Assert.That(truncated.Amount, Is.EqualTo(truncatedAmount));
			Assert.That(truncated.CurrencyCode, Is.EqualTo(CurrencyIsoCode.XTS));
		}

#pragma warning disable 169
		private static readonly object truncate = new[]
		{
			new [] {0m, 0m},
			new []{123.456m, 123m},
			new[]{-123.456m, -123m},
			new[]{-123.0000000m, -123m},
			new[]{-9999999999.9999999999m, -9999999999m}
		};
#pragma warning restore 169

		#endregion

		#region Round

		[TestCaseSource("roundToNearestInt")]
		public void RoundToNearestInt_Spec(decimal amount, decimal roundedAmount)
		{
			var subject = new Money(amount, CurrencyIsoCode.XTS);

			Money rounded = subject.RoundToNearestInt();
			Assert.That(rounded, Is.Not.SameAs(subject));
			Assert.That(rounded.CurrencyCode, Is.EqualTo(CurrencyIsoCode.XTS));
			Assert.That(rounded.Amount, Is.EqualTo(roundedAmount));
		}

#pragma warning disable 169
		private static readonly TestCaseData[] roundToNearestInt = new[]
		{
			new TestCaseData(twoThirds, 1m),
			new TestCaseData(0.5m, 0m).SetName("the closest even number is 0"),
			new TestCaseData(1.5m, 2m).SetName("the closest even number is 2"),
			new TestCaseData(1.499999m, 1m).SetName("the closest number is 1")
		};
#pragma warning restore 169

		[TestCaseSource("roundToNearestInt_WithMode")]
		public void RoundToNearestInt_WithMode_Spec(decimal amount, MidpointRounding mode, decimal roundedAmount)
		{
			var subject = new Money(amount, CurrencyIsoCode.XTS);

			Money rounded = subject.RoundToNearestInt(mode);
			Assert.That(rounded, Is.Not.SameAs(subject));
			Assert.That(rounded.CurrencyCode, Is.EqualTo(CurrencyIsoCode.XTS));
			Assert.That(rounded.Amount, Is.EqualTo(roundedAmount));
		}

#pragma warning disable 169
		private static readonly TestCaseData[] roundToNearestInt_WithMode = new[]
		{
			new TestCaseData(twoThirds, MidpointRounding.ToEven, 1m),
			new TestCaseData(twoThirds, MidpointRounding.AwayFromZero, 1m),
			new TestCaseData(0.5m, MidpointRounding.ToEven, 0m).SetName("the closest even number is 0"),
			new TestCaseData(0.5m, MidpointRounding.AwayFromZero, 1m).SetName("the closest number away from zero is 1"),
			new TestCaseData(1.5m, MidpointRounding.ToEven, 2m).SetName("the closest number is 2"),
			new TestCaseData(1.5m, MidpointRounding.AwayFromZero, 2m),
			new TestCaseData(1.499999m, MidpointRounding.ToEven, 1m).SetName("the closest number is 1"),
			new TestCaseData(1.499999m, MidpointRounding.AwayFromZero, 1m)
		};
#pragma warning restore 169

		[TestCaseSource("round")]
		public void Round_Spec(CurrencyIsoCode currency, decimal amount, decimal roundedAmount)
		{
			var subject = new Money(amount, currency);

			Money rounded = subject.Round();
			Assert.That(rounded, Is.Not.SameAs(subject));
			Assert.That(rounded.CurrencyCode, Is.EqualTo(currency));
			Assert.That(rounded.Amount, Is.EqualTo(roundedAmount));
		}

#pragma warning disable 169
		private static readonly TestCaseData[] round = new[]
		{
			new TestCaseData(CurrencyIsoCode.XTS, 2.345m, 2.34m).SetName("2 decimal places"),
			new TestCaseData(CurrencyIsoCode.USD, 2.345m, 2.34m).SetName("2 decimal places"),
			new TestCaseData(CurrencyIsoCode.JPY, 2.345m, 2m).SetName("0 decimal places"),
			
			new TestCaseData(CurrencyIsoCode.XTS, 2.355m, 2.36m).SetName("2 decimal places"),
			new TestCaseData(CurrencyIsoCode.USD, 2.355m, 2.36m).SetName("2 decimal places"),
			new TestCaseData(CurrencyIsoCode.JPY, 2.355m, 2m).SetName("0 decimal places"),
			
			new TestCaseData(CurrencyIsoCode.XTS, twoThirds, 0.67m).SetName("2 decimal places"),
			new TestCaseData(CurrencyIsoCode.USD, twoThirds, 0.67m).SetName("2 decimal places"),
			new TestCaseData(CurrencyIsoCode.JPY, twoThirds, 1m).SetName("0 decimal places"),
			
			new TestCaseData(CurrencyIsoCode.XTS, 0.5m, 0.50m).SetName("2 decimal places"),
			new TestCaseData(CurrencyIsoCode.USD, 0.5m, 0.50m).SetName("2 decimal places"),
			new TestCaseData(CurrencyIsoCode.JPY, 0.5m, 0m).SetName("0 decimal places"),
			
			new TestCaseData(CurrencyIsoCode.XTS, 1.5m, 1.50m).SetName("2 decimal places"),
			new TestCaseData(CurrencyIsoCode.USD, 1.5m, 1.50m).SetName("2 decimal places"),
			new TestCaseData(CurrencyIsoCode.JPY, 1.5m, 2m).SetName("0 decimal places"),
			
			new TestCaseData(CurrencyIsoCode.XTS, 1.499999m, 1.50m).SetName("2 decimal places"),
			new TestCaseData(CurrencyIsoCode.USD, 1.499999m, 1.50m).SetName("2 decimal places"),
			new TestCaseData(CurrencyIsoCode.JPY, 1.499999m, 1m).SetName("0 decimal places"),
		};
#pragma warning restore 169

		[TestCaseSource("round_WithMode")]
		public void Round_WithMode_Spec(CurrencyIsoCode currency, MidpointRounding mode, decimal amount, decimal roundedAmount)
		{
			var subject = new Money(amount, currency);

			Money rounded = subject.Round(mode);
			Assert.That(rounded, Is.Not.SameAs(subject));
			Assert.That(rounded.CurrencyCode, Is.EqualTo(currency));
			Assert.That(rounded.Amount, Is.EqualTo(roundedAmount));
		}

#pragma warning disable 169
		private static readonly object[] round_WithMode = new object[]
		{
		    new object[] {CurrencyIsoCode.XTS, MidpointRounding.ToEven, 2.345m, 2.34m},
		    new object[] {CurrencyIsoCode.XTS, MidpointRounding.AwayFromZero, 2.345m, 2.35m},
		    new object[] {CurrencyIsoCode.USD, MidpointRounding.ToEven, 2.345m, 2.34m},
		    new object[] {CurrencyIsoCode.USD, MidpointRounding.AwayFromZero, 2.345m, 2.35m},
		    new object[] {CurrencyIsoCode.JPY, MidpointRounding.ToEven, 2.345m, 2m},
		    new object[] {CurrencyIsoCode.JPY, MidpointRounding.AwayFromZero, 2.345m, 2m},

		    new object[] {CurrencyIsoCode.XTS, MidpointRounding.ToEven, 2.355m, 2.36m},
		    new object[] {CurrencyIsoCode.XTS, MidpointRounding.AwayFromZero, 2.355m, 2.36m},
		    new object[] {CurrencyIsoCode.USD, MidpointRounding.ToEven, 2.355m, 2.36m},
		    new object[] {CurrencyIsoCode.USD, MidpointRounding.AwayFromZero, 2.355m, 2.36m},
		    new object[] {CurrencyIsoCode.JPY, MidpointRounding.ToEven, 2.355m, 2m},
		    new object[] {CurrencyIsoCode.JPY, MidpointRounding.AwayFromZero, 2.355m, 2m},

		    new object[] {CurrencyIsoCode.XTS, MidpointRounding.ToEven, twoThirds, 0.67m},	
		    new object[] {CurrencyIsoCode.XTS, MidpointRounding.AwayFromZero, twoThirds, 0.67m},	
		    new object[] {CurrencyIsoCode.USD, MidpointRounding.ToEven, twoThirds, 0.67m},	
		    new object[] {CurrencyIsoCode.USD, MidpointRounding.AwayFromZero, twoThirds, 0.67m},	
		    new object[] {CurrencyIsoCode.JPY, MidpointRounding.ToEven, twoThirds, 1m},		
		    new object[] {CurrencyIsoCode.JPY, MidpointRounding.AwayFromZero, twoThirds, 1m},		

		    new object[] {CurrencyIsoCode.XTS, MidpointRounding.ToEven, 0.5m, 0.50m},		
		    new object[] {CurrencyIsoCode.XTS, MidpointRounding.AwayFromZero, 0.5m, 0.50m},		
		    new object[] {CurrencyIsoCode.USD, MidpointRounding.ToEven, 0.5m, 0.50m},		
		    new object[] {CurrencyIsoCode.USD, MidpointRounding.AwayFromZero, 0.5m, 0.50m},		
		    new object[] {CurrencyIsoCode.JPY, MidpointRounding.ToEven, 0.5m, 0m},			
		    new object[] {CurrencyIsoCode.JPY, MidpointRounding.AwayFromZero, 0.5m, 1m},			

		    new object[] {CurrencyIsoCode.XTS, MidpointRounding.ToEven, 1.5m, 1.50m},		
		    new object[] {CurrencyIsoCode.XTS, MidpointRounding.AwayFromZero, 1.5m, 1.50m},		
		    new object[] {CurrencyIsoCode.USD, MidpointRounding.ToEven, 1.5m, 1.50m},		
		    new object[] {CurrencyIsoCode.USD, MidpointRounding.AwayFromZero, 1.5m, 1.50m},		
		    new object[] {CurrencyIsoCode.JPY, MidpointRounding.ToEven, 1.5m, 2m},			
		    new object[] {CurrencyIsoCode.JPY, MidpointRounding.AwayFromZero, 1.5m, 2m},			

		    new object[] {CurrencyIsoCode.XTS, MidpointRounding.ToEven, 1.499999m, 1.50m},	
		    new object[] {CurrencyIsoCode.XTS, MidpointRounding.AwayFromZero, 1.499999m, 1.50m},	
		    new object[] {CurrencyIsoCode.USD, MidpointRounding.ToEven, 1.499999m, 1.50m},	
		    new object[] {CurrencyIsoCode.USD, MidpointRounding.AwayFromZero, 1.499999m, 1.50m},	
		    new object[] {CurrencyIsoCode.JPY, MidpointRounding.ToEven, 1.499999m, 1m},		
		    new object[] {CurrencyIsoCode.JPY, MidpointRounding.AwayFromZero, 1.499999m, 1m},		
		};
#pragma warning restore 169

		[TestCaseSource("round_WithDecimals")]
		public void Round_WithDecimals_Spec(int decimals, decimal amount, decimal roundedAmount)
		{
			var subject = new Money(amount, CurrencyIsoCode.XTS);

			Money rounded = subject.Round(decimals);
			Assert.That(rounded, Is.Not.SameAs(subject));
			Assert.That(rounded.CurrencyCode, Is.EqualTo(CurrencyIsoCode.XTS));
			Assert.That(rounded.Amount, Is.EqualTo(roundedAmount));
		}

#pragma warning disable 169
		private static readonly object[] round_WithDecimals = new object[]
		{
		    new object[] {2, 2.345m, 2.34m},
		    new object[] {4, 2.345m, 2.345m},
		    new object[] {0, 2.345m, 2m},

		    new object[] {2, 2.355m, 2.36m},
		    new object[] {4, 2.355m, 2.355m},
		    new object[] {0, 2.355m, 2m},

		    new object[] {2, twoThirds, 0.67m},
		    new object[] {3, twoThirds, 0.667m},
		    new object[] {0, twoThirds, 1m},

		    new object[] {2, 0.5m, 0.50m},
		    new object[] {3, 0.5m, 0.5m},
		    new object[] {0, 0.5m, 0m},

		    new object[] {2, 1.5m, 1.50m},
		    new object[] {3, 1.5m, 1.50m},
		    new object[] {0, 1.5m, 2m},

		    new object[] {2, 1.499999m, 1.50m},
		    new object[] {3, 1.499999m, 1.5m},
		    new object[] {0, 1.499999m, 1m},
		};
#pragma warning restore 169

		[TestCaseSource("round_WithDecimalsAndMode")]
		public void Round_WithDecimalsAndMode_Spec(int decimals, MidpointRounding mode, decimal amount, decimal roundedAmount)
		{
			var subject = new Money(amount, CurrencyIsoCode.XTS);

			Money rounded = subject.Round(decimals, mode);
			Assert.That(rounded, Is.Not.SameAs(subject));
			Assert.That(rounded.CurrencyCode, Is.EqualTo(CurrencyIsoCode.XTS));
			Assert.That(rounded.Amount, Is.EqualTo(roundedAmount));
		}

#pragma warning disable 169
		private static readonly object[] round_WithDecimalsAndMode = new object[]
		{
			new object[] {2, MidpointRounding.ToEven, 2.345m, 2.34m},
		    new object[] {2, MidpointRounding.AwayFromZero, 2.345m, 2.35m},
		    new object[] {3, MidpointRounding.ToEven, 2.345m, 2.345m},
		    new object[] {3, MidpointRounding.AwayFromZero, 2.345m, 2.345m},
		    new object[] {0, MidpointRounding.ToEven, 2.345m, 2m},
		    new object[] {0, MidpointRounding.AwayFromZero, 2.345m, 2m},

		    new object[] {2, MidpointRounding.ToEven, 2.355m, 2.36m},
		    new object[] {2, MidpointRounding.AwayFromZero, 2.355m, 2.36m},
		    new object[] {3, MidpointRounding.ToEven, 2.355m, 2.355m},
		    new object[] {3, MidpointRounding.AwayFromZero, 2.355m, 2.355m},
		    new object[] {0, MidpointRounding.ToEven, 2.355m, 2m},
		    new object[] {0, MidpointRounding.AwayFromZero, 2.355m, 2m},

		    new object[] {2, MidpointRounding.ToEven, twoThirds, 0.67m},	
		    new object[] {2, MidpointRounding.AwayFromZero, twoThirds, 0.67m},	
		    new object[] {3, MidpointRounding.ToEven, twoThirds, 0.667m},	
		    new object[] {3, MidpointRounding.AwayFromZero, twoThirds, 0.667m},	
		    new object[] {0, MidpointRounding.ToEven, twoThirds, 1m},		
		    new object[] {0, MidpointRounding.AwayFromZero, twoThirds, 1m},		

		    new object[] {2, MidpointRounding.ToEven, 0.5m, 0.50m},		
		    new object[] {2, MidpointRounding.AwayFromZero, 0.5m, 0.50m},		
		    new object[] {3, MidpointRounding.ToEven, 0.5m, 0.50m},		
		    new object[] {3, MidpointRounding.AwayFromZero, 0.5m, 0.50m},		
		    new object[] {0, MidpointRounding.ToEven, 0.5m, 0m},			
		    new object[] {0, MidpointRounding.AwayFromZero, 0.5m, 1m},			

		    new object[] {2, MidpointRounding.ToEven, 1.5m, 1.50m},		
		    new object[] {2, MidpointRounding.AwayFromZero, 1.5m, 1.50m},		
		    new object[] {3, MidpointRounding.ToEven, 1.5m, 1.50m},		
		    new object[] {3, MidpointRounding.AwayFromZero, 1.5m, 1.50m},		
		    new object[] {0, MidpointRounding.ToEven, 1.5m, 2m},			
		    new object[] {0, MidpointRounding.AwayFromZero, 1.5m, 2m},			

		    new object[] {2, MidpointRounding.ToEven, 1.499999m, 1.50m},	
		    new object[] {2, MidpointRounding.AwayFromZero, 1.499999m, 1.50m},	
		    new object[] {3, MidpointRounding.ToEven, 1.499999m, 1.50m},	
		    new object[] {3, MidpointRounding.AwayFromZero, 1.499999m, 1.50m},	
		    new object[] {0, MidpointRounding.ToEven, 1.499999m, 1m},		
		    new object[] {0, MidpointRounding.AwayFromZero, 1.499999m, 1m},		
		};
#pragma warning restore 169

		#endregion

		#region Floor

		[TestCaseSource("floor")]
		public void Floor_Spec(decimal amount, decimal flooredAmount)
		{
			var subject = new Money(amount, CurrencyIsoCode.XTS);

			Money truncated = subject.Floor();
			Assert.That(truncated.Amount, Is.EqualTo(flooredAmount));
			Assert.That(truncated.CurrencyCode, Is.EqualTo(CurrencyIsoCode.XTS));
		}

#pragma warning disable 169
		private static readonly object floor = new[]
		{
			new[] {0m, 0m},
			new[] {123.456m, 123m},
			new[] {-123.456m, -124m},
			new[] {-123.0000000m, -123m},
			new[] {-9999999999.9999999999m, -10000000000m}
		};
#pragma warning restore 169

		#endregion

		#region Perform

		[Test]
		public void Perform_Unary_UnaryOperationPerformed()
		{
			bool performed = false;
			Func<decimal, decimal> unary = d =>
			{
				performed = true;
				return d;
			};

			decimal amount = 2m;
			var subject = new Money(amount, CurrencyIsoCode.XTS);
			Money result = subject.Perform(unary);

			Assert.That(performed, Is.True);
			Assert.That(result, Is.Not.SameAs(subject));
			Assert.That(result.Amount, Is.EqualTo(amount));
			Assert.That(result.CurrencyCode, Is.EqualTo(CurrencyIsoCode.XTS));
		}

		[Test]
		public void Perform_BinarySameCurrency_BinaryOperationPerformed()
		{
			bool performed = false;
			Func<decimal, decimal, decimal> binary = (x, y) =>
			{
				performed = true;
				return x * y;
			};

			Money subject = new Money(2m, CurrencyIsoCode.XTS),
				  operand = new Money(3m, CurrencyIsoCode.XTS);

			Money result = subject.Perform(operand, binary);

			Assert.That(performed, Is.True);
			Assert.That(result, Is.Not.SameAs(subject).And.Not.SameAs(operand));
			Assert.That(result.Amount, Is.EqualTo(6m));
			Assert.That(result.CurrencyCode, Is.EqualTo(CurrencyIsoCode.XTS));
		}

		[Test]
		public void Perform_BinaryDifferentCurrency_Exception()
		{
			bool performed = false;
			Func<decimal, decimal, decimal> binary = (x, y) =>
			{
				performed = true;
				return x * y;
			};

			Money subject = new Money(2m, CurrencyIsoCode.XTS),
				  operand = new Money(2m, CurrencyIsoCode.XXX);

			Assert.That(() => subject.Perform(operand, binary), Throws.InstanceOf<DifferentCurrencyException>());
			Assert.That(performed, Is.False);
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