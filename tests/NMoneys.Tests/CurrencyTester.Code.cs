namespace NMoneys.Tests;

public partial class CurrencyTester
{
	#region Parse

	[Test]
	public void Parse_Defined_UpperCased_AlphabeticCode_CodeParsed()
	{
		Assert.That(Currency.Code.Parse("USD"), Is.EqualTo(CurrencyIsoCode.USD));
	}

	[Test]
	public void Parse_Defined_LowerCased_AlphabeticCode_CodeParsed()
	{
		Assert.That(Currency.Code.Parse("eur"), Is.EqualTo(CurrencyIsoCode.EUR));
	}

	[Test]
	public void Parse_Defined_MixedCased_AlphabeticCode_CodeParsed()
	{
		Assert.That(Currency.Code.Parse("NoK"), Is.EqualTo(CurrencyIsoCode.NOK));
	}

	[Test]
	public void Parse_Defined_NumericCode_CodeParsed()
	{
		Assert.That(Currency.Code.Parse("999"), Is.EqualTo(CurrencyIsoCode.XXX));
	}

	[Test]
	public void Parse_Defined_PadedNumericCode_CodeParsed()
	{
		Assert.That(Currency.Code.Parse("036"), Is.EqualTo(CurrencyIsoCode.AUD));
	}

	[Test]
	public void Parse_Null_Exception()
	{
		Assert.That(() => Currency.Code.Parse(null!), Throws.InstanceOf<ArgumentNullException>());
	}

	[Test]
	public void Parse_Undefined_AlphabeticCode_Exception()
	{
		Assert.That(() => Currency.Code.Parse("notAnIsoCode"), Throws.ArgumentException);
	}

	[Test]
	public void Parse_Undefined_NumericCode_Exception()
	{
		Assert.That(() => Currency.Code.Parse("0"), Throws.InstanceOf<UndefinedCodeException>());
	}

	[Test]
	public void Parse_Overflowing_NumericCode_Exception()
	{
		long overflowingCode = short.MinValue + 1L;
		Assert.That(() => Currency.Code.Parse(overflowingCode.ToString()), Throws.InstanceOf<OverflowException>());
	}

	[Test]
	public void DefaultParse_Defined_UpperCased_AlphabeticCode_CodeParsed()
	{
		Assert.That(Currency.Code.Parse("USD", CurrencyIsoCode.XTS), Is.EqualTo(CurrencyIsoCode.USD));
	}

	[Test]
	public void DefaultParse_Defined_LowerCased_AlphabeticCode_CodeParsed()
	{
		Assert.That(Currency.Code.Parse("eur", CurrencyIsoCode.XTS), Is.EqualTo(CurrencyIsoCode.EUR));
	}

	[Test]
	public void DefaultParse_Defined_MixedCased_AlphabeticCode_CodeParsed()
	{
		Assert.That(Currency.Code.Parse("NoK", CurrencyIsoCode.XTS), Is.EqualTo(CurrencyIsoCode.NOK));
	}

	[Test]
	public void DefaultParse_Defined_NumericCode_CodeParsed()
	{
		Assert.That(Currency.Code.Parse("999", CurrencyIsoCode.XTS), Is.EqualTo(CurrencyIsoCode.XXX));
	}

	[Test]
	public void DefaultParse_Defined_PadedNumericCode_CodeParsed()
	{
		Assert.That(Currency.Code.Parse("036", CurrencyIsoCode.XTS), Is.EqualTo(CurrencyIsoCode.AUD));
	}

	[Test]
	public void DefaultParse_Null_Exception()
	{
		Assert.That(() => Currency.Code.Parse(null!, CurrencyIsoCode.XTS), Throws.ArgumentNullException);
	}

	[Test]
	public void DefaultParse_Undefined_AlphabeticCode_Exception()
	{
		Assert.That(() => Currency.Code.Parse("notAnIsoCode", CurrencyIsoCode.XTS),
			Throws.InstanceOf<UndefinedCodeException>());
	}

	[Test]
	public void DefaultParse_Undefined_NumericCode_Default()
	{
		Assert.That(() => Currency.Code.Parse("0", CurrencyIsoCode.XTS), Throws.InstanceOf<UndefinedCodeException>());
	}

	[Test]
	public void DefaultParse_Overflowing_NumericCode_Exception()
	{
		long overflowingCode = short.MinValue + 1L;
		Assert.That(() => Currency.Code.Parse(overflowingCode.ToString(), CurrencyIsoCode.XTS),
			Throws.InstanceOf<UndefinedCodeException>());
	}

	#endregion

	#region TryParse

	[Test]
	public void TryParse_Defined_UpperCased_AlphabeticCode_CodeParsed()
	{
		CurrencyIsoCode? parsed;
		Assert.That(Currency.Code.TryParse("USD", out parsed), Is.True);
		Assert.That(parsed, Is.EqualTo(CurrencyIsoCode.USD));
	}

	[Test]
	public void TryParse_Defined_LowerCased_AlphabeticCode_CodeParsed()
	{
		CurrencyIsoCode? parsed;
		Assert.That(Currency.Code.TryParse("eur", out parsed), Is.True);
		Assert.That(parsed, Is.EqualTo(CurrencyIsoCode.EUR));
	}

	[Test]
	public void TryParse_Defined_MixedCased_AlphabeticCode_CodeParsed()
	{
		CurrencyIsoCode? parsed;
		Assert.That(Currency.Code.TryParse("NoK", out parsed), Is.True);
		Assert.That(parsed, Is.EqualTo(CurrencyIsoCode.NOK));
	}

	[Test]
	public void TryParse_Defined_NumericCode_CodeParsed()
	{
		CurrencyIsoCode? parsed;
		Assert.That(Currency.Code.TryParse("999", out parsed), Is.True);
		Assert.That(parsed, Is.EqualTo(CurrencyIsoCode.XXX));
	}

	[Test]
	public void TryParse_Defined_PaddedNumericCode_CodeParsed()
	{
		CurrencyIsoCode? parsed;
		Assert.That(Currency.Code.TryParse("036", out parsed), Is.True);
		Assert.That(parsed, Is.EqualTo(CurrencyIsoCode.AUD));
	}

	[Test]
	public void TryParse_Null_Exception()
	{
		Assert.That(() => Currency.Code.TryParse(null!, out var parsed), Throws.ArgumentNullException);
	}

	[Test]
	public void TryParse_Undefined_AlphabeticCode_Exception()
	{
		CurrencyIsoCode? parsed;
		Assert.That(Currency.Code.TryParse("notAnIsoCode", out parsed), Is.False);
		Assert.That(parsed, Is.Null);
	}

	[Test]
	public void TryParse_Undefined_NumericCode_Exception()
	{
		CurrencyIsoCode? parsed;
		Assert.That(Currency.Code.TryParse("0", out parsed), Is.False);
		Assert.That(parsed, Is.Null);
	}

	[Test]
	public void TryParse_Overflowing_NumericCode_Exception()
	{
		CurrencyIsoCode? parsed;
		long overflowingCode = short.MinValue + 1L;
		Assert.That(Currency.Code.TryParse(overflowingCode.ToString(), out parsed), Is.False);
		Assert.That(parsed, Is.Null);
	}

	#endregion

	#region Cast

	[Test]
	public void Cast_Defined_CastedIsoCode()
	{
		Assert.That(Currency.Code.Cast(36), Is.EqualTo(CurrencyIsoCode.AUD));
	}

	[Test]
	public void Cast_UndefinedValue_Exception()
	{
		Assert.That(() => Currency.Code.Cast(46),
			Throws.InstanceOf<UndefinedCodeException>()
				.With.Message.Contains("46")
				.And.Message.Contains("CurrencyIsoCode"));
	}

	#endregion

	#region TryCast

	[Test]
	public void TryCast_DefinedValue_CastedIsoCode()
	{
		CurrencyIsoCode? casted;
		Assert.That(Currency.Code.TryCast(36, out casted), Is.True);
		Assert.That(casted, Is.EqualTo(CurrencyIsoCode.AUD));
	}

	[Test]
	public void TryCast_UndefinedValue_False()
	{
		CurrencyIsoCode? casted;
		Assert.That(Currency.Code.TryCast(46, out casted), Is.False);
		Assert.That(casted, Is.Null);
	}

	#endregion
}
