using NMoneys.Support;

namespace NMoneys.Tests.Support;

[TestFixture]
public class SmallPowerOfTenTester
{
	[TestCase(0, (ushort)1)]
	[TestCase(1, (ushort)10)]
	[TestCase(4, (ushort)10000)]
	public void Positive_Spec(byte exponent, ushort powerOfTen)
	{
		Assert.That(SmallPowerOfTen.Positive(exponent), Is.TypeOf<ushort>().And.EqualTo(powerOfTen));
	}

	[Test]
	public void Positive_TooBigOfAExponent_Exception()
	{
		Assert.That(()=> SmallPowerOfTen.Positive(5), Throws.InstanceOf<ArgumentOutOfRangeException>()
			.With.Message.Contains("5"));
	}

	[TestCase(0, 1f)]
	[TestCase(1, 0.1f)]
	[TestCase(4, 0.0001f)]
	// float used dues to decimals being annoying in attributes
	public void Negative_Spec(byte exponent, float powerOfTen)
	{
		Assert.That(SmallPowerOfTen.Negative(exponent), Is.TypeOf<decimal>().And.EqualTo(new decimal(powerOfTen)));
	}

	[Test]
	public void Negative_TooBigOfAExponent_Exception()
	{
		Assert.That(()=> SmallPowerOfTen.Negative(5), Throws.InstanceOf<ArgumentOutOfRangeException>()
			.With.Message.Contains("5"));
	}
}
