namespace NMoneys.Tests;

public partial class CurrencyTester
{
	[Test]
	public void CompareTo_NonGeneric_AccordingToSpec()
	{
		object to = null;
		Assert.That(Currency.Xts.CompareTo(to), Is.GreaterThan(0));

		to = Currency.Xts;
		Assert.That(Currency.Xts.CompareTo(to), Is.EqualTo(0));
		to = Currency.Xxx;
		Assert.That(Currency.Xts.CompareTo(to), Is.LessThan(0));
		to = Currency.Aud;
		Assert.That(Currency.Xts.CompareTo(to), Is.GreaterThan(0));

		var notACurrency = new Exception();
		Assert.That(() => Currency.Xts.CompareTo(notACurrency), Throws
			.ArgumentException
			.With.Message.Contains("Currency"));
	}

	[Test]
	public void CompareTo_Generic_AccordingToSpec()
	{
		Currency to = null;
		Assert.That(Currency.Xts.CompareTo(to), Is.GreaterThan(0));

		to = Currency.Xts;
		Assert.That(Currency.Xts.CompareTo(to), Is.EqualTo(0));
		to = Currency.Xxx;
		Assert.That(Currency.Xts.CompareTo(to), Is.LessThan(0));
		to = Currency.Aud;
		Assert.That(Currency.Xts.CompareTo(to), Is.GreaterThan(0));
	}

	[Test]
	public void GreaterThan_Generic_AccordingToSpec()
	{
		Currency to = null;
		Assert.That(Currency.Xts > to, Is.True);

		to = Currency.Xts;
		Assert.That(Currency.Xts > to, Is.False);
		to = Currency.Xxx;
		Assert.That(Currency.Xts > to, Is.False);
		to = Currency.Aud;
		Assert.That(Currency.Xts > to, Is.True);
	}

	[Test]
	public void GreaterOrEqualThan_Generic_AccordingToSpec()
	{
		Currency to = null;
		Assert.That(Currency.Xts >= to, Is.True);

		to = Currency.Xts;
		Assert.That(Currency.Xts >= to, Is.True);
		to = Currency.Xxx;
		Assert.That(Currency.Xts >= to, Is.False);
		to = Currency.Aud;
		Assert.That(Currency.Xts >= to, Is.True);
	}

	[Test]
	public void LessOrEqualThan_Generic_AccordingToSpec()
	{
		Currency to = null;
		Assert.That(Currency.Xts <= to, Is.False);

		to = Currency.Xts;
		Assert.That(Currency.Xts <= to, Is.True);
		to = Currency.Xxx;
		Assert.That(Currency.Xts <= to, Is.True);
		to = Currency.Aud;
		Assert.That(Currency.Xts <= to, Is.False);
	}
}
