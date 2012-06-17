using NMoneys.Allocation;
using NUnit.Framework;

namespace NMoneys.Tests.Allocation
{
	[TestFixture]
	public partial class RatioTester
	{
		[Test]
		public void Equality_EqualsWhenSameValue()
		{
			var pointTwo = new Ratio(.2m);
			var pointOnePlusPointOne = new Ratio(.1m + .1m);

			Assert.That(pointTwo.Equals(pointOnePlusPointOne), Is.True);
		}

		[Test]
		public void Equality_DoesNotEqualsDifferentValues()
		{
			var pointTwo = new Ratio(.2m);
			var pointOnePlusPointTwo = new Ratio(.1m + .2m);

			Assert.That(pointTwo.Equals(pointOnePlusPointTwo), Is.False);
		}

		[Test]
		public void Equality_DifferentTypes_False()
		{
			var pointTwo = new Ratio(.2m);
			decimal onePlusOne = 1m + 1m;

			Assert.That(pointTwo.Equals(onePlusOne), Is.False);
		}

		[Test]
		public void GetHashCode_AsOfValue()
		{
			var pointTwo = new Ratio(.2m);

			Assert.That(pointTwo.GetHashCode(), Is.EqualTo(.2m.GetHashCode()));
		}
	}
}