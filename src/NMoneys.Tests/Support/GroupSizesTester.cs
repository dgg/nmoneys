using System;
using NMoneys.Support;
using NUnit.Framework;

namespace NMoneys.Tests.Support
{
	[TestFixture]
	public class GroupSizesTester
	{
		#region FromtokenizedSizes

		[TestCase("3", new[] { 3 })]
		[TestCase("3 2", new[] { 3, 2 })]
		[TestCase("3 0", new[] { 3, 0 })]
		[TestCase("0", new[] { 0 })]
		[TestCase("", new int[0])]
		public void FromTokenizedSizes_ValidTokenizedSizes_TranslatedToSizes(
			string tokenizedSizes,
			int[] sizes)
		{
			GroupSizes subject = GroupSizes.FromTokenizedSizes(tokenizedSizes);

			Assert.That(subject.TokenizedSizes, Is.EqualTo(tokenizedSizes));
			Assert.That(subject.Sizes, Is.EqualTo(sizes));
		}

		[TestCase("3,3")]
		[TestCase("3-1")]
		[TestCase("3:0")]
		public void FromTokenizedSizes_WrongTokenizer_Exception(string wrongTokenizer)
		{
			Assert.That(() => GroupSizes.FromTokenizedSizes(wrongTokenizer), Throws.InstanceOf<FormatException>());
		}

		[TestCase("0 3", typeof(ArgumentException))]
		[TestCase("-4 -3", typeof(ArgumentException))]
		[TestCase("a b", typeof(FormatException))]
		[TestCase(null, typeof(ArgumentNullException))]
		public void FromTokenizedSizes_NotASizeChain_Exception(string notAChainOfSizes, Type expectedException)
		{
			Assert.That(() => GroupSizes.FromTokenizedSizes(notAChainOfSizes), Throws.InstanceOf(expectedException));
		}

		#endregion


		#region FromSizes

		[TestCase(new[] { 3 }, "3")]
		[TestCase(new[] { 3, 2 }, "3 2")]
		[TestCase(new[] { 3, 0 }, "3 0")]
		[TestCase(new[] { 0 }, "0")]
		[TestCase(new int[0], "")]
		public void FromSizes_CollectionOfSizes_SizesTokenized(int[] sizes, string tokenizedSizes)
		{
			GroupSizes subject = GroupSizes.FromSizes(sizes);

			Assert.That(subject.Sizes, Is.EqualTo(sizes));
			Assert.That(subject.TokenizedSizes, Is.EqualTo(tokenizedSizes));
		}

		[TestCase(null, typeof(ArgumentNullException))]
		[TestCase(new[] { -3 }, typeof(ArgumentException))]
		[TestCase(new[] { 0, 3 }, typeof(ArgumentException))]
		public void FromSizes_IncorrectSizes_Exception(int[] incorrectSizes, Type expectedException)
		{
			Assert.That(()=> GroupSizes.FromSizes(incorrectSizes), Throws.InstanceOf(expectedException));
		}

		#endregion

	}
}