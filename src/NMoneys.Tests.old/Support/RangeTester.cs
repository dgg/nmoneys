using System;
using NMoneys.Support;
using NUnit.Framework;

namespace NMoneys.Tests.Support
{
	[TestFixture]
	public class RangeTester
	{
		#region ctor

		[Test]
		public void Ctor_Closed_ConsistentBounds_PropertiesSet()
		{
			var subject = new Range<int>(1.Close(), 2.Close());
			Assert.That(subject.LowerBound, Is.EqualTo(1));
			Assert.That(subject.UpperBound, Is.EqualTo(2));
		}

		[Test]
		public void Ctor_Open_ConsistentBounds_PropertiesSet()
		{
			var subject = new Range<int>(1.Open(), 2.Open());
			Assert.That(subject.LowerBound, Is.EqualTo(1));
			Assert.That(subject.UpperBound, Is.EqualTo(2));
		}

		[Test]
		public void Ctor_HalfClosed_ConsistentBounds_PropertiesSet()
		{
			var subject = new Range<int>(1.Open(), 2.Close());
			Assert.That(subject.LowerBound, Is.EqualTo(1));
			Assert.That(subject.UpperBound, Is.EqualTo(2));
		}

		[Test]
		public void Ctor_HalfOpen_ConsistentBounds_PropertiesSet()
		{
			var subject = new Range<int>(1.Close(), 2.Open());
			Assert.That(subject.LowerBound, Is.EqualTo(1));
			Assert.That(subject.UpperBound, Is.EqualTo(2));
		}

		[Test]
		public void Ctor_Closed_InconsistentBounds_Exception()
		{
			Assert.That(() => new Range<int>(2.Close(), 1.Close()),
				Throws.InstanceOf<ArgumentOutOfRangeException>()
				.With.Property("ParamName").EqualTo("upperBound").And
				.With.Property("ActualValue").EqualTo(1));
		}

		[Test]
		public void Ctor_Open_InconsistentBounds_Exception()
		{
			Assert.That(() => new Range<int>(2.Open(), 1.Open()),
				Throws.InstanceOf<ArgumentOutOfRangeException>()
				.With.Property("ParamName").EqualTo("upperBound").And
				.With.Property("ActualValue").EqualTo(1));
		}

		[Test]
		public void Ctor_HalfClosed_InconsistentBounds_Exception()
		{
			Assert.That(() => new Range<int>(2.Open(), 1.Close()),
				Throws.InstanceOf<ArgumentOutOfRangeException>()
				.With.Property("ParamName").EqualTo("upperBound").And
				.With.Property("ActualValue").EqualTo(1));
		}

		[Test]
		public void Ctor_HalfOpen_InconsistentBounds_Exception()
		{
			Assert.That(() => new Range<int>(2.Close(), 1.Open()),
				Throws.InstanceOf<ArgumentOutOfRangeException>()
				.With.Property("ParamName").EqualTo("upperBound").And
				.With.Property("ActualValue").EqualTo(1));
		}

		#endregion

		#region well contained item

		[Test]
		public void Contains_Closed_WellContained_True()
		{
			var subject = new Range<int>(1.Close(), 5.Close());

			Assert.That(subject.Contains(3), Is.True);
		}

		[Test]
		public void Contains_Open_WellContained_True()
		{
			var subject = new Range<int>(1.Open(), 5.Open());

			Assert.That(subject.Contains(3), Is.True);
		}

		[Test]
		public void Contains_HalfOpen_WellContained_True()
		{
			var subject = new Range<int>(1.Close(), 5.Open());

			Assert.That(subject.Contains(3), Is.True);
		}

		[Test]
		public void Contains_HalfClosed_WellContained_True()
		{
			var subject = new Range<int>(1.Open(), 5.Close());

			Assert.That(subject.Contains(3), Is.True);
		}

		#endregion

		#region lower bound item

		[Test]
		public void Contains_Closed_LowerBound_True()
		{
			var subject = new Range<int>(1.Close(), 5.Close());

			Assert.That(subject.Contains(1), Is.True);
		}

		[Test]
		public void Contains_Open_LowerBound_False()
		{
			var subject = new Range<int>(1.Open(), 5.Open());

			Assert.That(subject.Contains(1), Is.False);
		}

		[Test]
		public void Contains_HalfOpen_LowerBound_True()
		{
			var subject = new Range<int>(1.Close(), 5.Open());

			Assert.That(subject.Contains(1), Is.True);
		}

		[Test]
		public void Contains_HalfClosed_LowerBound_False()
		{
			var subject = new Range<int>(1.Open(), 5.Close());

			Assert.That(subject.Contains(1), Is.False);
		}

		#endregion

		#region upper bound item

		[Test]
		public void Contains_Closed_UpperBound_True()
		{
			var subject = new Range<int>(1.Close(), 5.Close());

			Assert.That(subject.Contains(5), Is.True);
		}

		[Test]
		public void Contains_Open_UpperBound_False()
		{
			var subject = new Range<int>(1.Open(), 5.Open());

			Assert.That(subject.Contains(5), Is.False);
		}

		[Test]
		public void Contains_HalfOpen_UpperBound_False()
		{
			var subject = new Range<int>(1.Close(), 5.Open());

			Assert.That(subject.Contains(5), Is.False);
		}

		[Test]
		public void Contains_HalfClosed_UpperBound_True()
		{
			var subject = new Range<int>(1.Open(), 5.Close());

			Assert.That(subject.Contains(5), Is.True);
		}

		#endregion

		#region not contained item

		[Test]
		public void Contains_Closed_NotContained_False()
		{
			var subject = new Range<int>(1.Close(), 5.Close());

			Assert.That(subject.Contains(6), Is.False);
		}

		[Test]
		public void Contains_Open_NotContained_False()
		{
			var subject = new Range<int>(1.Open(), 5.Open());

			Assert.That(subject.Contains(6), Is.False);
		}

		[Test]
		public void Contains_HalfOpen_NotContained_False()
		{
			var subject = new Range<int>(1.Close(), 5.Open());

			Assert.That(subject.Contains(6), Is.False);
		}

		[Test]
		public void Contains_HalfClosed_NotContained_False()
		{
			var subject = new Range<int>(1.Open(), 5.Close());

			Assert.That(subject.Contains(6), Is.False);
		}

		#endregion

		#region ToString

		[Test]
		public void ToString_Closed_BetweenBrackets()
		{
			var subject = new Range<int>(1.Close(), 5.Close());

			Assert.That(subject.ToString(), Is.EqualTo("[1..5]"));
		}

		[Test]
		public void ToString_Open_BetweenParenthesis()
		{
			var subject = new Range<int>(1.Open(), 5.Open());

			Assert.That(subject.ToString(), Is.EqualTo("(1..5)"));
		}

		[Test]
		public void Contains_HalfOpen_ParenthesisAtTheEnd()
		{
			var subject = new Range<int>(1.Close(), 5.Open());

			Assert.That(subject.ToString(), Is.EqualTo("[1..5)"));
		}

		[Test]
		public void Contains_HalfClosed_ParenthesisAtTheBeginning()
		{
			var subject = new Range<int>(1.Open(), 5.Close());

			Assert.That(subject.ToString(), Is.EqualTo("(1..5]"));
		}

		#endregion

		#region AssertArgument

		[Test]
		public void AssertArgument_Closed_NotContained_Exception()
		{
			var subject = new Range<int>(1.Close(), 5.Close());

			Assert.That(() => subject.AssertArgument("arg", 6),
				Throws.InstanceOf<ArgumentOutOfRangeException>()
				.With.Message.Contains("[1..5]").And
				.With.Message.Contains("1 (inclusive)").And
				.With.Message.Contains("5 (inclusive)")
				.With.Property("ParamName").EqualTo("arg").And
				.With.Property("ActualValue").EqualTo(6));
		}

		[Test]
		public void AssertArgument_Closed_Contained_NoException()
		{
			var subject = new Range<int>(1.Close(), 5.Close());

			Assert.That(() => subject.AssertArgument("arg", 4), Throws.Nothing);
		}

		[Test]
		public void AssertArgument_Open_NotContained_Exception()
		{
			var subject = new Range<int>(1.Open(), 5.Open());

			Assert.That(() => subject.AssertArgument("arg", 6),
				Throws.InstanceOf<ArgumentOutOfRangeException>()
				.With.Message.Contains("(1..5)").And
				.With.Message.Contains("1 (not inclusive)").And
				.With.Message.Contains("5 (not inclusive)")
				.With.Property("ParamName").EqualTo("arg").And
				.With.Property("ActualValue").EqualTo(6));
		}

		[Test]
		public void AssertArgument_Open_Contained_NoException()
		{
			var subject = new Range<int>(1.Open(), 5.Open());

			Assert.That(() => subject.AssertArgument("arg", 3), Throws.Nothing);
		}

		[Test]
		public void AssertArgument_HalfOpen_NotContained_Exception()
		{
			var subject = new Range<int>(1.Close(), 5.Open());

			Assert.That(() => subject.AssertArgument("arg", 6),
				Throws.InstanceOf<ArgumentOutOfRangeException>()
				.With.Message.Contains("[1..5)").And
				.With.Message.Contains("1 (inclusive)").And
				.With.Message.Contains("5 (not inclusive)")
				.With.Property("ParamName").EqualTo("arg").And
				.With.Property("ActualValue").EqualTo(6));
		}

		[Test]
		public void AssertArgument_HalfOpen_Contained_NoException()
		{
			var subject = new Range<int>(1.Close(), 5.Open());

			Assert.That(() => subject.AssertArgument("arg", 4), Throws.Nothing);
		}

		[Test]
		public void AssertArgument_HalfClosed_NotContained_Exception()
		{
			var subject = new Range<int>(1.Open(), 5.Close());

			Assert.That(() => subject.AssertArgument("arg", 6),
				Throws.InstanceOf<ArgumentOutOfRangeException>()
				.With.Message.Contains("(1..5]").And
				.With.Message.Contains("1 (not inclusive)").And
				.With.Message.Contains("5 (inclusive)")
				.With.Property("ParamName").EqualTo("arg").And
				.With.Property("ActualValue").EqualTo(6));
		}

		[Test]
		public void AssertArgument_HalfClosed_Contained_NoException()
		{
			var subject = new Range<int>(1.Open(), 5.Close());

			Assert.That(() => subject.AssertArgument("arg", 4), Throws.Nothing);
		}

		[Test]
		public void AssertArgument_NullCollection_Exception()
		{
			Assert.That(() => new Range<int>(1.Close(), 5.Close()).AssertArgument("arg", null),
				Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void AssertArgument_AllContained_NoException()
		{
			Assert.That(() => new Range<int>(1.Close(), 5.Close()).AssertArgument("arg", new[]{2, 3, 4}),
				Throws.Nothing);
		}

		[Test]
		public void AssertArgument_SomeNotContained_ExceptionWithOffendingMember()
		{
			Assert.That(() => new Range<int>(1.Close(), 5.Close()).AssertArgument("arg", new[] { 2, 6, 4 }),
				Throws.InstanceOf<ArgumentOutOfRangeException>()
				.With.Message.Contains("[1..5]").And
				.With.Message.Contains("1 (inclusive)").And
				.With.Message.Contains("5 (inclusive)")
				.With.Property("ParamName").EqualTo("arg").And
				.With.Property("ActualValue").EqualTo(6)
				);
		}

		#endregion

		#region Equality

		[Test]
		public void Equals_SameBoundNatureSameValues_True()
		{
			Range<int> one = new Range<int>(1.Open(), 2.Open()),
				another = new Range<int>(1.Open(), 2.Open());

			Assert.That(one.Equals(another), Is.True);
		}

		[Test]
		public void Equals_SameBoundNatureDifferentValues_False()
		{
			Range<int> one = new Range<int>(1.Open(), 2.Open()),
				another = new Range<int>(1.Open(), 3.Open());

			Assert.That(one.Equals(another), Is.False);
		}

		[Test]
		public void Equals_DifferentBoundNatureSameValues_False()
		{
			Range<int> one = new Range<int>(1.Open(), 2.Open()),
				another = new Range<int>(1.Open(), 2.Close());

			Assert.That(one.Equals(another), Is.False);
		}

		#endregion
	}
}
